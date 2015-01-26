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
        private Queue<Eyes> _listCurrentEyes;   //Temporal list to store EyeData values
        private Eyes _currentEyesData;          //Most recently EyeData read
        private Word<T> _currentWord;           //Most recently Word read
        private bool _isExpectingNewWord;       //Broker is expecting a new word

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
                //No word has been receieved
                if ((item = base._listInputPort[INDEX_SPRIYZ].GetItem()) != null)
                {
                    this._currentWord = (Word<T>)item.Value;
                }
            }
            else
            {
                //A word has been already receieved. If null, value has not been processed
                if (this._currentEyesData == null)
                {
                    item = base._listInputPort[INDEX_EYE_TRIBE].GetItem();
                    this._currentEyesData = (Eyes)item.Value;
                }

                if (this._currentEyesData.Timestamp >= this._currentWord.Timestamp
                    && this._currentEyesData.Timestamp < (this._currentWord.Timestamp + this._currentWord.Duration))
                {
                    //Eyes data belongs to a word. If new word has just been received. Clean idle time data
                    if (this._isExpectingNewWord)
                    {
                        sendToOutput(null);
                        this._isExpectingNewWord = false;
                    }
                }
                else if (this._currentEyesData.Timestamp >= (this._currentWord.Timestamp + this._currentWord.Duration))
                {
                    //Current word has finished
                    sendToOutput(_currentWord);
                    this._isExpectingNewWord = true;
                    this._currentWord = null;
                }

                //If word finished, wait for next iteration to process last value
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
            if (this._listCurrentEyes.Count > 0)
            {
                WordAndEyes<T> wordAndEyes = new WordAndEyes<T>(new Queue<Eyes>(this._listCurrentEyes), word);
                this._listCurrentEyes.Clear();
                base.sendToOutput(new Bundle<WordAndEyes<T>>(ItemTypes.WordAndEyes, wordAndEyes));
            }
        }
    }
}
