using Newtonsoft.Json;

namespace ICAPR_RSVP.Misc
{
    public class DisplayItem<T>
    {
        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; private set; }
        [JsonProperty(PropertyName = "duration")]
        public long Duration { get; private set; }
        [JsonProperty(PropertyName = "value")]
        public T Value { get; private set; }

        public DisplayItem(long timestamp, long duration, T value)
        {
            Timestamp = timestamp;
            Duration = duration;
            Value = value;
        }
    }
}
