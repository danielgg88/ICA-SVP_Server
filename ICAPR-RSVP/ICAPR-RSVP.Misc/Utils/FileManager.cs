using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace ICAPR_RSVP.Misc.Utils
{
    public class FileManager<T>
    {
        private const String LOGS_FOLDER = @"\logs\";       //Folder to save logs
        private ConcurrentQueue<Item> _itemQueue;           //List of items to save
        private String _basePath;                           //Base path (Desktop)
        private String _fileName;                           //Log file name

        #region Public writing methods

        public FileManager(String fileName)
        {
            _itemQueue = new ConcurrentQueue<Item>();
            _fileName = fileName;
            init();
        }

        public void AddToFile(Item item) {
            //Add an item to print into the logs
            if (item != null)
                _itemQueue.Enqueue(item);
        }

        public String WriteCsvFile()
        {
            //Write all the WordAndEyes items into a file
            List<String> output = new List<string>();
            List<Item> cpyList = new List<Item>(_itemQueue);

            output.Add("Word time, Word, Duration, Eye time, L size, R size");
            String tmp = String.Empty;

            foreach (Item item in cpyList)
            {
                if (item.Type == ItemTypes.WordAndEyes)
                {
                    WordAndEyes<String> wordAndEyes = (WordAndEyes<String>)item.Value;
                    Queue<Eyes> listEyes = wordAndEyes.Eyes;
                    Word<String> word = wordAndEyes.Word;

                    tmp = word.Timestamp + ", ";

                    if (word.Value != null)
                        tmp += word.Value;
                    else
                        tmp += " ";

                    tmp += ", " + word.Duration + ", ";

                    foreach (Eyes eyes in listEyes)
                        output.Add(tmp + eyes.Timestamp + ", " + eyes.LeftEye.PupilSize + ", " + eyes.RightEye.PupilSize);
                }
            }
            return WriteFile(output,".csv");
        }

        public String WriteJsonFile()
        {
            List<Item> cpyList = new List<Item>(_itemQueue);
            List<WordAndEyes<T>> tmp = new List<WordAndEyes<T>>();
            
            foreach (Item item in cpyList)
            {
                if (item.Type == ItemTypes.WordAndEyes)
                    tmp.Add((WordAndEyes<T>)item.Value);
            }
            if (tmp.Count > 0)
            {
                List<String> json = new List<String>();
                json.Add(Newtonsoft.Json.JsonConvert.SerializeObject(tmp));
                return WriteFile(json,".json");
            }
            else
                return null;
        }

        public void Clear()
        {
            //Clear the queue to print
            _itemQueue = new ConcurrentQueue<Item>();
        }

        #endregion

        #region Public reading methods

        public String getJsonFileList()
        {
            //Get the list of saved files in JSON format
            List<object> files = new List<object>();
            IEnumerable<String> file_paths = Directory.EnumerateFiles(_basePath + LOGS_FOLDER, "*.json"); 

            foreach(String file in file_paths)
                files.Add (new {file_name = Path.GetFileNameWithoutExtension(file)});
                
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

        private String WriteFile(List<String> content, String fileExtension)
        {
            //Write content into file
            String fileName = _fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            File.WriteAllLines(_basePath + LOGS_FOLDER + fileName + fileExtension, content);
            return fileName;
        }

        #endregion
    }
}
