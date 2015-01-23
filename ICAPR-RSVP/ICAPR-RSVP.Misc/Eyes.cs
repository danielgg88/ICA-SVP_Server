namespace ICAPR_RSVP.Misc
{
    public class Eyes
    {
        public long Timestamp { get; private set; }
        public Eye LeftEye { get; private set; }
        public Eye RightEye { get; private set; }

        public Eyes(long timestamp, Eye left, Eye right)
        {
            Timestamp = timestamp;
            LeftEye = left;
            RightEye = right;
        }
    }
}
