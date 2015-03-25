﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using ICAPR_SVP.Misc.Items;
using Newtonsoft.Json.Linq;

namespace ICAPR_SVP.Misc.Utils
{
    public class FileManager<T>
    {
        private const String LOGS_FOLDER = @"\logs\";       //Folder to save logs
        private ConcurrentQueue<Item> _itemQueue;           //List of items to save
        private String _basePath;                           //Base path (Desktop)
        private ExperimentConfig _currentConfig;            //Experiment current configuration

        #region Public writing methods

        public FileManager()
        {
            _itemQueue = new ConcurrentQueue<Item>();
            init();
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
                WriteCsvFile(fileName,items);
                WriteJsonFile(fileName,items);
                Clear();
            }
        }

        #endregion

        #region Public reading methods

        public String getJsonFileList()
        {
            //Get the list of saved files in JSON format
            List<object> files = new List<object>();
            IEnumerable<String> file_paths = Directory.EnumerateFiles(_basePath + LOGS_FOLDER,"*.json");

            foreach(String file in file_paths)
                files.Add(new
                {
                    file_name = Path.GetFileNameWithoutExtension(file)
                });

            return Newtonsoft.Json.JsonConvert.SerializeObject(files);
        }

        public String getJsonFile(String fileName)
        {
            //Return JSON file content
            String path = _basePath + LOGS_FOLDER + fileName + ".json";
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
            _basePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            bool exists = Directory.Exists(_basePath + LOGS_FOLDER);
            if(!exists)
                Directory.CreateDirectory(_basePath + LOGS_FOLDER);
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
            output = output.Remove(output.Length - 1);
            output += "\n\n Word time, Word, Duration, Eye time, L size, R size \n";

            foreach(Item item in items)
            {
                if(item.Type == ItemTypes.DisplayItemAndEyes)
                {
                    DisplayItemAndEyes<String> wordAndEyes = (DisplayItemAndEyes<String>)item.Value;
                    Queue<Eyes> listEyes = wordAndEyes.EyesOriginal;
                    DisplayItem<String> word = wordAndEyes.DisplayItem;

                    tmp = word.Timestamp + ", ";

                    if(word.Value != null)
                        tmp += word.Value;
                    else
                        tmp += " ";

                    tmp += ", " + word.Duration + ", ";

                    foreach(Eyes eyes in listEyes)
                        output += tmp + eyes.Timestamp + ", "
                            + eyes.LeftEye.PupilSize + ", " + eyes.RightEye.PupilSize + "\n";
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

        private void WriteFile(String fileName,String content,String fileExtension)
        {
            File.WriteAllText(_basePath + LOGS_FOLDER + fileName + fileExtension,content);
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