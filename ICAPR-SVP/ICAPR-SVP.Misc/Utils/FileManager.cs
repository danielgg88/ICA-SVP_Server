using ICAPR_SVP.Misc.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ICAPR_SVP.Misc.Utils
{
    public class FileManager<T>
    {
        private const String LOGS_FOLDER = @"\logs\";       //Folder to save logs
        private ConcurrentQueue<Item> _itemQueue;           //List of items to save
        private ExperimentConfig _currentConfig;            //Experiment current configuration
        private Port _inputPort;                            //Input port to read items from
        private Thread _workerThread;
        private bool _isRunning;

        #region Public writing methods

        public FileManager(Port inputPort)
        {
            _itemQueue = new ConcurrentQueue<Item>();
            _inputPort = inputPort;

            init();
        }

        public void Start()
        {
            _workerThread = new Thread(DoWork);
            _workerThread.Start();
            this._isRunning = true;
        }

        public void AddToFile(Item item)
        {
            //Add an item to print into the logs
            if(item != null)
            {
                //Create a new log if a new trial is started
                if(item.Type == ItemTypes.EndOfTrial)
                {
                    if(_itemQueue.Count > 0)
                    {
                        SaveLog();
                        Clear();
                    }
                }
                else if(item.Type == ItemTypes.Config)
                    _currentConfig = (ExperimentConfig)item.Value;
                else
                    _itemQueue.Enqueue(item);
            }
        }

        public void SaveLog()
        {
            if(_currentConfig != null && _currentConfig.SaveLog)
            {
                //Creates JSON and CSV files
                Console.WriteLine("FM: Saving logs..");
                String fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + _currentConfig.Trial;
                List<Item> items = new List<Item>(_itemQueue);

                //Save items in files
                if(Config.FileManager.CREATE_CSV_ORIGINAL_PROCESSED)
                    WriteCsvFile(fileName,items);
                if(Config.FileManager.CREATE_JSON)
                    WriteJsonFile(fileName,items);
                if(Config.FileManager.CREATE_CSV_PER_SECOND)
                    WriteCsvForTraining(fileName,items);

                Clear();
            }
        }

        public void Stop()
        {
            this._isRunning = false;
            _workerThread.Interrupt();
            SaveLog();
        }

        #endregion

        #region Public reading methods

        public static String getJsonFileList()
        {
            //Get the list of saved files in JSON format
            List<object> files = new List<object>();
            IEnumerable<String> file_paths = Directory.EnumerateFiles(Config.FileManager.BASE_PATH + LOGS_FOLDER,"*.json");

            foreach(String file in file_paths)
                files.Add(new
                {
                    file_name = Path.GetFileNameWithoutExtension(file)
                });

            return Newtonsoft.Json.JsonConvert.SerializeObject(files);
        }

        public static String getJsonFile(String fileName)
        {
            //Return JSON file content
            String path = Config.FileManager.BASE_PATH + LOGS_FOLDER + fileName + ".json";
            if(File.Exists(path))
                return File.ReadAllText(path);
            else
                return JsonConvert.SerializeObject(new List<object>());
        }

        #endregion

        #region Private methods

        private void init()
        {
            //Create logs folder if it does not exist 
            bool exists = Directory.Exists(Config.FileManager.BASE_PATH + LOGS_FOLDER);
            if(!exists)
                Directory.CreateDirectory(Config.FileManager.BASE_PATH + LOGS_FOLDER);
        }

        private void DoWork()
        {
            
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            try
            {
                while(this._isRunning)
                {
                    this.AddToFile(_inputPort.GetItem());
                }
            }
            catch(ThreadInterruptedException)
            {
            }
        }

        private void WriteCsvFile(String fileName,List<Item> items)
        {
            //Write all the WordAndEyes items into a .CSV file
            String output = String.Empty;
            String tmp = String.Empty;

            JObject json = Newtonsoft.Json.Linq.JObject.Parse(JsonConvert.SerializeObject(_currentConfig));

            int i = 1;
            foreach(var x in json)
            {
                output += x.Key + " : " + x.Value;
                if(i % 6 == 0 && i != json.Count - 1)
                    output += "\n";
                else
                    output += ",";
                i++;
            }

            output += " Sampling rate: " + Misc.Config.EyeTribe.SAMPLING_FREQUENCY + "\n";

            output = output.Remove(output.Length - 1);
            output += "\n\n Word time, Word, Duration, Eye time, L size, LP size, R size, RP size \n";

            foreach(Item item in items)
            {
                if(item.Type == ItemTypes.DisplayItemAndEyes)
                {
                    DisplayItemAndEyes<String> wordAndEyes = (DisplayItemAndEyes<String>)item.Value;
                    Queue<Eyes> listEyes = wordAndEyes.Eyes;
                    DisplayItem<String> word = wordAndEyes.DisplayItem;

                    tmp = word.Timestamp + ", ";

                    if(word.Value != null)
                        tmp += word.Value;
                    else
                        tmp += " ";

                    tmp += ", " + word.Duration + ", ";

                    foreach(Eyes eyes in listEyes)
                        output += tmp + eyes.Timestamp + ", "
                            + eyes.LeftEye.PupilSize + ", " + eyes.LeftEyeProcessed.PupilSize + ", "
                            + eyes.RightEye.PupilSize + ", " + eyes.RightEyeProcessed.PupilSize + "\n";
                }
            }
            WriteFile(fileName,output,".csv");
        }

        private void WriteJsonFile(String fileName,List<Item> items)
        {
            //Write all the WordAndEyes items into a .CSV file
            List<DisplayItemAndEyes<T>> trialData = new List<DisplayItemAndEyes<T>>();

            foreach(Item item in items)
            {
                if(item.Type == ItemTypes.DisplayItemAndEyes)
                    trialData.Add((DisplayItemAndEyes<T>)item.Value);
            }

            if(trialData.Count > 0)
            {
                String json = Newtonsoft.Json.JsonConvert.SerializeObject(new Trial<T>(_currentConfig,trialData));
                WriteFile(fileName,json,".json");
            }
        }

        private void WriteCsvForTraining(String fileName,List<Item> items)
        {
            //Write 1 second eye data per row into a .CSV file
            String output = "word, blinks, error, ica, left/right," ;
            for (int i = 0; i < Config.EyeTribe.SAMPLING_FREQUENCY; i++)
                output += "s" + i + ",";

            output += "label\n";
            foreach (Item item in items)
            {
                if (item.Type == ItemTypes.DisplayItemAndEyes)
                {
                    DisplayItemAndEyes<String> wordAndEyes = (DisplayItemAndEyes<String>)item.Value;
                    DisplayItem<String> word = wordAndEyes.DisplayItem;
                    Eyes[] eyesArray = wordAndEyes.Eyes.ToArray();

                    int iterations = eyesArray.Length / Config.EyeTribe.SAMPLING_FREQUENCY;
                    int modulus = eyesArray.Length % Config.EyeTribe.SAMPLING_FREQUENCY;

                    for (int i = 0; i < iterations; i++)
                        output += WriteCsvForTrainingRow
                            (word, wordAndEyes.SummaryItem, eyesArray, i, Config.EyeTribe.SAMPLING_FREQUENCY);

                    if (modulus > 0)
                        output += WriteCsvForTrainingRow(word, wordAndEyes.SummaryItem, eyesArray, iterations, modulus);
                }
            }
            WriteFile(fileName + "_training",output,".csv");
        }

        private String WriteCsvForTrainingRow(DisplayItem<String> word,SummaryItem summaryItem,Eyes[] eyes,
            int second,int samples)
        {
            String output = "",outputLeft = "",outputRight = "",metadata = "";

            metadata += (word.Value != null) ? word.Value + "," : "delay,";
            metadata += summaryItem.BlinkSamples[0] + ",";
            metadata += summaryItem.ErrorSamples[1] + ",";

            int index_start = second * Config.EyeTribe.SAMPLING_FREQUENCY;
            int index_end = index_start + samples;

            for(int j = index_start;j < index_end - 1;j++)
            {
                outputLeft += eyes[j].LeftEyeProcessed.PupilSize + ",";
                outputRight += eyes[j].RightEyeProcessed.PupilSize + ",";
            }

            if(Config.EyeTribe.SAMPLING_FREQUENCY > samples)
            {
                for(int j = 0;j < Config.EyeTribe.SAMPLING_FREQUENCY - samples;j++)
                {
                    outputLeft += Calibration.Calibrator.AvgPupilSize[0] + ",";
                    outputRight += Calibration.Calibrator.AvgPupilSize[1] + ",";
                }
                outputLeft += Calibration.Calibrator.AvgPupilSize[0] + "\n";
                outputRight += Calibration.Calibrator.AvgPupilSize[1] + "\n";
            }
            else
            {
                outputLeft += eyes[index_end].LeftEyeProcessed.PupilSize + "\n";
                outputRight += eyes[index_end].RightEyeProcessed.PupilSize + "\n";
            }
            output += metadata + summaryItem.Ica[0][second] + ", L," + outputLeft;
            output += metadata + summaryItem.Ica[1][second] + ", R," + outputRight;

            return output;
        }

        private void WriteFile(String fileName,String content,String fileExtension)
        {
            File.WriteAllText(Config.FileManager.BASE_PATH + LOGS_FOLDER + fileName + fileExtension,content);
        }

        private void Clear()
        {
            //Clear the queue to print
            _itemQueue = new ConcurrentQueue<Item>();
            _currentConfig = null;
        }

        #endregion
    }
}
