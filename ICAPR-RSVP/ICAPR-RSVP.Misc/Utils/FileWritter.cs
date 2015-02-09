using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAPR_RSVP.Misc.Utils
{
    public class FileWritter
    {
        private const String LOGS_FOLDER = @"\logs\";       //Folder to save logs
        private const String LOGS_FILE_EXTENTION = ".txt";  //Log file extension
        private ConcurrentQueue<Item> _itemQueue;           //List of items to save
        private String _basePath;                           //Base path (Desktop)
        private String _fileName;                           //Log file name

        #region Public methods

        public FileWritter(String fileName)
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

        public void WriteWordAndEyesToFile()
        {
            //Write all the WordAndEyes items into a file
            List<String> output = new List<string>();
            List<Item> cpyList = new List<Item>(_itemQueue);
            Clear();

            String tmp = String.Empty;

            foreach (Item item in cpyList)
            {
                if (item.Type == ItemTypes.WordAndEyes)
                {
                    WordAndEyes<String> wordAndEyes = (WordAndEyes<String>)item.Value;
                    Queue<Eyes> listEyes = wordAndEyes.Eyes;
                    Word<String> word = wordAndEyes.Word;

                    tmp = word.Timestamp + ",";

                    if (word.Value != null)
                        tmp += word.Value;
                    else
                        tmp += "---";

                    tmp += "," + word.Duration + "\n";

                    foreach (Eyes eyes in listEyes)
                        tmp += eyes.Timestamp + "," + eyes.LeftEye.PupilSize + "," + eyes.RightEye.PupilSize + "\n";
                }
                output.Add(tmp);
            }

            if (output.Count > 0)
                WriteFile(output);
        }
   
        public void Clear()
        {
            //Clear the queue to print
            _itemQueue = new ConcurrentQueue<Item>();
        }

        #endregion

        #region Private methods

        private void init()
        {
            //Create logs folder if it does not exist
           _basePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            bool exists = System.IO.Directory.Exists(_basePath + LOGS_FOLDER);
            if (!exists)
                System.IO.Directory.CreateDirectory(_basePath + LOGS_FOLDER);
        }

        private void WriteFile(List<String> content)
        {
            //Write content into file
            String fileName = _fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            System.IO.File.WriteAllLines(_basePath + LOGS_FOLDER + fileName + LOGS_FILE_EXTENTION, content);
        }

        #endregion
    }
}
