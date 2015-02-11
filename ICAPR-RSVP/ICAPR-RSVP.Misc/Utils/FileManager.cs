using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using ICAPR_RSVP.Misc.Items;

namespace ICAPR_RSVP.Misc.Utils
{
    public class FileManager<T>
    {
        private const String LOGS_FOLDER = @"\logs\";       //Folder to save logs
        private ConcurrentQueue<Item> _itemQueue;           //List of items to save
        private String _basePath;                           //Base path (Desktop)
        private String _fileName;                           //Log file name
        private ExperimentConfig _currentConfig;            //Experiment current configuration

        #region Public writing methods

        public FileManager(String fileName)
        {
            _itemQueue = new ConcurrentQueue<Item>();
            _fileName = fileName;
            init();
        }

        public void AddToFile(Item item)
        {
            //Add an item to print into the logs
            if (item != null)
                _itemQueue.Enqueue(item);
        }

        public void SaveLog()
        {
            //Creates JSON and CSV files
            String fileName = _fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            WriteCsvFile(fileName);
            WriteJsonFile(fileName);
            Clear();
        }

        #endregion

        #region Public reading methods

        public String getJsonFileList()
        {
            //Get the list of saved files in JSON format
            List<object> files = new List<object>();
            IEnumerable<String> file_paths = Directory.EnumerateFiles(_basePath + LOGS_FOLDER, "*.json");

            foreach (String file in file_paths)
                files.Add(new { file_name = Path.GetFileNameWithoutExtension(file) });

            return Newtonsoft.Json.JsonConvert.SerializeObject(files);
        }

        public String getJsonFile(String fileName)
        {
            //Return JSON file content
            String path = _basePath + LOGS_FOLDER + fileName + ".json";
            if (File.Exists(path))
                return File.ReadAllText(path);
            else
                return JsonConvert.SerializeObject(new List<object>());
        }

        #endregion

        #region Private methods

        private void init()
        {
            //Create logs folder if it does not exist
            _basePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            bool exists = Directory.Exists(_basePath + LOGS_FOLDER);
            if (!exists)
                Directory.CreateDirectory(_basePath + LOGS_FOLDER);
        }

        private void WriteCsvFile(String fileName)
        {
            //Write all the WordAndEyes items into a file
            List<String> outputList = new List<string>();
            List<Item> cpyList = new List<Item>(_itemQueue);
            String output = String.Empty;
            String tmp = String.Empty;
            _currentConfig = null;

            foreach (Item item in cpyList)
            {
                if (item.Type == ItemTypes.Config)
                {
                    //A config object is found the first time
                    if (_currentConfig != null)
                    {
                        //Write file and start new trial
                        outputList.Add(output);
                    }

                    ExperimentConfig trial = (ExperimentConfig)item.Value;
                    output = "Trial: " + trial.Trial + ", ";
                    output += "UserName: " + trial.UserName + ", ";
                    output += "UserAge: " + trial.UserAge + ", ";
                    output += "FileName: " + trial.FileName + ", ";
                    output += "ItemTime: " + trial.ItemTime + ", ";
                    output += "DelayTime: " + trial.DelayTime + ", ";
                    output += "FontSize: " + trial.FontSize + ", ";
                    output += "FontColor: " + trial.FontColor + ", ";
                    output += "AppBackground: " + trial.AppBackground + ", ";
                    output += "BoxBackground: " + trial.BoxBackground + "\n";

                    output += "Word time, Word, Duration, Eye time, L size, R size \n";
                    _currentConfig = trial;
                }
                else if (item.Type == ItemTypes.DisplayItemAndEyes)
                {
                    DisplayItemAndEyes<String> wordAndEyes = (DisplayItemAndEyes<String>)item.Value;
                    Queue<Eyes> listEyes = wordAndEyes.Eyes;
                    DisplayItem<String> word = wordAndEyes.DisplayItem;

                    tmp = word.Timestamp + ", ";

                    if (word.Value != null)
                        tmp += word.Value;
                    else
                        tmp += " ";

                    tmp += ", " + word.Duration + ", ";

                    foreach (Eyes eyes in listEyes)
                        output += tmp + eyes.Timestamp + ", "
                            + eyes.LeftEye.PupilSize + ", " + eyes.RightEye.PupilSize + "\n";
                }
            }
            outputList.Add(output);
            WriteFile(fileName, outputList, ".csv");
        }

        private void WriteJsonFile(String fileName)
        {
            //Returns the last file name written. Many files might be created if many config objects
            //are added to the printing queue
            List<Item> cpyList = new List<Item>(_itemQueue);
            List<DisplayItemAndEyes<T>> trialData = new List<DisplayItemAndEyes<T>>();
            List<String> json = new List<String>();
            _currentConfig = null;

            foreach (Item item in cpyList)
            {
                if (item.Type == ItemTypes.Config)
                {
                    //A config object is found the first time
                    if (_currentConfig != null)
                    {
                        //Add to file and start new trial
                        json.Add(Newtonsoft.Json.JsonConvert.SerializeObject(new Trial<T>(_currentConfig, trialData)));
                        trialData = new List<DisplayItemAndEyes<T>>();
                    }
                    _currentConfig = (ExperimentConfig)item.Value;

                }
                else if (item.Type == ItemTypes.DisplayItemAndEyes)
                    trialData.Add((DisplayItemAndEyes<T>)item.Value);
            }

            json.Add(Newtonsoft.Json.JsonConvert.SerializeObject(new Trial<T>(_currentConfig, trialData)));
            WriteFile(fileName, json, ".json");
        }

        private void WriteFile(String fileName, List<String> content, String fileExtension)
        {
            int fileCount = 0;
            foreach (String trial in content)
            {
                //Write content into file
                if (fileCount == 0)
                    File.WriteAllText(_basePath + LOGS_FOLDER + fileName + fileExtension, trial);
                else
                    File.WriteAllText(_basePath + LOGS_FOLDER + fileName + "_" + fileCount + fileExtension, trial);
                fileCount++;
            }
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
