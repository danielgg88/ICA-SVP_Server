namespace ICAPR_RSVP.Misc
{
    public interface Item
    {
        ItemTypes Type { get; }
        object Value { get; }
    }

    public class Bundle<T> : Item
    {
        public ItemTypes Type { get; set; }    
        private T Value { get; set; }

        public Bundle(ItemTypes type, T value)
        {
            this.Type = type;
            this.Value = value;
        }

        // Explicit implementation of Item.Value
        object Item.Value
        {
            get {return this.Value; }
        }
    }
}


