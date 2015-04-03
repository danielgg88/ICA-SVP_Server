using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICAPR_SVP.Misc.Calibration;
using ICAPR_SVP.Misc;

namespace ICAPR_SVP.DataCleaning
{
    public class FilterBlinkDetection : Filter
    {
        private List<Eyes> _pendingEyes;
        private int _indexLastLeftOkValue;
        private int _indexLastRightOkValue;

        #region Public methods
        public FilterBlinkDetection(String name)
            : base(name)
        {
            _pendingEyes = new List<Eyes>();
            _indexLastLeftOkValue = -1;
            _indexLastRightOkValue = -1;
        }

        public static void Interpolate(List<Eyes> eyes,int index_first_point,int index_last_point,bool is_left_eye,double avg_pupil_size)
        {
            //Check for at least one item to interpolate
            if(index_last_point - index_first_point - 1 >= 1)
            {
                //If not a blink, it is an error
                Eye.CleaningFlags flag = (index_last_point - index_first_point - 1 >= Config.Cleaning.BLINK_MIN_CONSEQUENT_SAMPLES) ?
                    Eye.CleaningFlags.Blink : Eye.CleaningFlags.PossibleBlink;

                if(index_last_point - index_first_point - 1 >= Config.Cleaning.BLINK_MIN_CONSEQUENT_SAMPLES)
                {
                    //The first and last elements of the array are used as extreme points to interpolate
                    double x0,y0,x1,y1;

                    //Set beggining reference coordinates
                    if(index_first_point == -1)
                    {
                        //-1 means the first item in the array is a Possible blink. 
                        //The Average pupil size is used as first point
                        x0 = 0 - (1000 / Config.EyeTribe.SAMPLING_FREQUENCY);
                        y0 = avg_pupil_size;
                    }
                    else
                    {
                        x0 = eyes.ElementAt(index_first_point).Timestamp;
                        y0 = (is_left_eye) ?
                            eyes.ElementAt(index_first_point).LeftEye.PupilSize :
                            eyes.ElementAt(index_first_point).RightEye.PupilSize;
                    }

                    //Set end reference coordinates
                    if(index_last_point == eyes.Count)
                    {
                        //index_last_point out of array boundaries means the last item in the array is a possible blink.
                        //The Average pupil size is used as last point
                        x1 = eyes.ElementAt(eyes.Count - 1).Timestamp + (1000 / Config.EyeTribe.SAMPLING_FREQUENCY);
                        y1 = avg_pupil_size;
                    }
                    else
                    {
                        x1 = eyes.ElementAt(index_last_point).Timestamp;
                        y1 = (is_left_eye) ?
                            eyes.ElementAt(index_last_point).LeftEye.PupilSize :
                            eyes.ElementAt(index_last_point).RightEye.PupilSize;
                    }

                    //Interpolate points and mark as blink
                    for(int i = index_first_point + 1;i < index_last_point;i++)
                    {
                        Eyes eyes_item = eyes.ElementAt(i);
                        Eye eye = (is_left_eye) ? eyes_item.LeftEye : eyes_item.RightEye;
                        Eye eye_processed = (is_left_eye) ? eyes_item.LeftEyeProcessed : eyes_item.RightEyeProcessed;
                        eye_processed.PupilSize = Misc.Utils.Utils.linear(eyes_item.Timestamp,x0,x1,y0,y1);
                        eye_processed.CleaningFlag = flag;
                        eye.CleaningFlag = flag;
                    }
                }
            }
        }
        #endregion

        #region Private methods
        private void AddPossibleBlinkFlagToEyes(Item item)
        {
            Eyes eyes = (Eyes)item.Value;
            eyes.LeftEyeProcessed = new Eye();
            eyes.RightEyeProcessed = new Eye();

            //Validate LEFT pupil size againts threshold
            if(eyes.LeftEye.PupilSize < getCurrentAvgPupilSize()[0] - Config.Cleaning.BLINK_DIAMETER_THRESHOLD_LOW_MM)
                eyes.LeftEye.CleaningFlag = Eye.CleaningFlags.PossibleBlink;
            else
                eyes.LeftEye.CleaningFlag = Eye.CleaningFlags.Ok;
            //Validate RIGHT pupil size againts threshold
            if(eyes.RightEye.PupilSize < getCurrentAvgPupilSize()[1] - Config.Cleaning.BLINK_DIAMETER_THRESHOLD_LOW_MM)
                eyes.RightEye.CleaningFlag = Eye.CleaningFlags.PossibleBlink;
            else
                eyes.RightEye.CleaningFlag = Eye.CleaningFlags.Ok;
            //Enque Eyes for being processed
            _pendingEyes.Add(eyes);
        }

        private void ProcessPendingItems(Port output)
        {
            if(_pendingEyes.Count > 0)
            {
                Eye leftEye = _pendingEyes.Last().LeftEye;
                Eye rigthEye = _pendingEyes.Last().RightEye;
                int currentIndex = _pendingEyes.Count - 1;

                //Try to interpolate left eye
                if(leftEye.CleaningFlag == Eye.CleaningFlags.Ok)
                {
                    //while()
                    Interpolate(_pendingEyes,_indexLastLeftOkValue,currentIndex,true,getCurrentAvgPupilSize()[0]);
                    //Update last OK index
                    _indexLastLeftOkValue = currentIndex;
                }
                //Try to interpolate right eye
                if(rigthEye.CleaningFlag == Eye.CleaningFlags.Ok)
                {
                    Interpolate(_pendingEyes,_indexLastRightOkValue,currentIndex,false,getCurrentAvgPupilSize()[1]);
                    //Update last Ok index
                    _indexLastRightOkValue = currentIndex;
                }

                if(_indexLastLeftOkValue == currentIndex || _indexLastRightOkValue == currentIndex)
                {
                    //lowerIndex is the minimum index on which all previous items are already interpolated
                    int lowerIndex = (_indexLastLeftOkValue < _indexLastRightOkValue) ?
                        _indexLastLeftOkValue : _indexLastRightOkValue;

                    //If both indexes are in the current position, keep for late interpolation
                    if(lowerIndex == currentIndex)
                        lowerIndex--;

                    //Release all items until from the beggining to lowerIndex
                    int count  = sendItemsToOutput(output,_pendingEyes,lowerIndex);

                    //Update OK indexes
                    _indexLastLeftOkValue -= count;
                    _indexLastRightOkValue -= count;
                }
            }
        }

        private int  sendItemsToOutput(Port output,List<Eyes> eyes,int index_end)
        {
            //Send to output index_end item and all preceeding ones
            int i = 0;
            for(i = 0;i <= index_end;i++)
            {
                output.PushItem(new Bundle<Eyes>(ItemTypes.Eyes,eyes.ElementAt(0)));
                eyes.RemoveAt(0);
            }
            return i;
        }

        private double[] getCurrentAvgPupilSize()
        {
            //Wait in case is calibrating
            while(Calibrator.AvgPupilSize == null)
                ;
            double[] avg_pupil_size = new double[2];
            avg_pupil_size[0] = Calibrator.AvgPupilSize[0];
            avg_pupil_size[1] = Calibrator.AvgPupilSize[1];
            return avg_pupil_size;
        }
        #endregion

        #region Protected methods
        protected override void OnExecute(Port input,Port output)
        {
            ProcessPendingItems(output);
            Item item = input.GetItem();
            if(item.Type == ItemTypes.Eyes)
                AddPossibleBlinkFlagToEyes(item);
        }

        protected override void OnStop(Port output)
        {
            //In case of last item is blink, interpolate with averge from calibrator
            Interpolate(_pendingEyes,0,_pendingEyes.Count,true,getCurrentAvgPupilSize()[0]);
            Interpolate(_pendingEyes,0,_pendingEyes.Count,true,getCurrentAvgPupilSize()[1]);
            //Send to ouput all remaining items
            sendItemsToOutput(output,_pendingEyes,_pendingEyes.Count - 1);
        }
        #endregion
    }
}
