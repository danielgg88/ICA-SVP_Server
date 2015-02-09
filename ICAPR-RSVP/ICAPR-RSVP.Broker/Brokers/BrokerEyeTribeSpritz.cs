using ICAPR_RSVP.Misc;
using System;
using System.Collections.Generic;

namespace ICAPR_RSVP.Broker
{
    //<T> content message type
    public class BrokerEyeTribeSpritz<T> : Broker
    {
        private readonly int INDEX_EYE_TRIBE = 0;         //EyeTribe index in input list
        private readonly int INDEX_SPRIYZ = 1;           //Sprits index in input list
        private Queue<Eyes> _listCurrentEyes;           //Temporal list to store EyeData values
        private Eyes _currentEyesData;          //Most recently EyeData read
        private Word<T> _currentWord;           //Most recently Word read
        private bool _isExpectingNewWord;       //Broker is expecting a new word
        private long _delayStartTimestamp = 0;  //Delay start timestamp. Used for delays between displayer items.

        public BrokerEyeTribeSpritz()
            : base()
        {
            this._listCurrentEyes = new Queue<Eyes>();
            this._isExpectingNewWord = true;
        }

        protected override void Run()
        {
            //Merge input from Spritz and EyeTribe
            Item item;

            if (this._currentWord == null)
            {
                //No word has been receieved. Get a new word!
                if ((item = base._listInputPort[INDEX_SPRIYZ].GetItem()) != null)
                {
                    this._currentWord = (Word<T>)item.Value;
                }
            }
            else
            {
                //Get eye data if a word has been already receieved. 
                //If not null is because previous data belongs to the a new word an has not been processed, 
                //so add it to the new word.
                if (this._currentEyesData == null)
                {
                    item = base._listInputPort[INDEX_EYE_TRIBE].GetItem();
                    this._currentEyesData = (Eyes)item.Value;
                }
                
                //Eyes data belongs to the current word.
                if (this._currentEyesData.Timestamp >= this._currentWord.Timestamp
                    && this._currentEyesData.Timestamp < (this._currentWord.Timestamp + this._currentWord.Duration))
                {
                    //If word has just started, create delays
                    if (this._isExpectingNewWord)
                    {
                        sendToOutput(null);
                        this._isExpectingNewWord = false;
                    }
                }
                //Eye data does not belong to current word anymore. Attcach eye data to current word
                //and expect a new word.
                else if (this._currentEyesData.Timestamp >= (this._currentWord.Timestamp + this._currentWord.Duration))
                {
                    sendToOutput(_currentWord);
                    this._isExpectingNewWord = true;
                }

                //If word finished, wait for next iteration to process last eyes data
                //sendToOuptut sets it to null
                if (this._currentWord != null)
                {
                    this._listCurrentEyes.Enqueue(_currentEyesData);
                    _currentEyesData = null;
                }
            }
        }

        private void sendToOutput(Word<T> word)
        {
            //Create object to output

            Word<T> tmpWord;
            
            if (this._listCurrentEyes.Count > 0)
            {
                //Null = delay between words
                if (word == null)
                {
                    long start_timestamp;
                    //If no previous word has been receieved yet. Use the first eye data timestamp as start
                    if (this._delayStartTimestamp == 0)
                        start_timestamp = this._listCurrentEyes.Peek().Timestamp;
                    else
                    //Use the end of the previous word as start for delay.
                        start_timestamp = this._delayStartTimestamp;

                    //Calculate delay duration and create delay (word value = null)
                    tmpWord = new Word<T>(start_timestamp,
                            this._currentWord.Timestamp - start_timestamp, default(T));
                }
                else
                    tmpWord = word;

                //Sent to output pipe the created item
                WordAndEyes<T> wordAndEyes = new WordAndEyes<T>(new Queue<Eyes>(this._listCurrentEyes), tmpWord);
                this._listCurrentEyes.Clear();
                base.sendToOutput(new Bundle<WordAndEyes<T>>(ItemTypes.WordAndEyes, wordAndEyes));
            }

            //If not null (end of the word) set it to null and calculate new starting point for incoming delays
            if (word != null)
            {
                this._delayStartTimestamp = word.Timestamp + word.Duration;
                this._currentWord = null;
            }
        }
    }
}
