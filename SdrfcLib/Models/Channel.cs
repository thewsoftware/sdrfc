namespace FrequencyManagerCommon.Models
{
    public class Channel
    {
        public bool IsFavorite { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public long Frequency { get; set; }
        public string Modulation { get; set; }
        public long Shift { get; set; }
        public long Bandwidth { get; set; }
    }
}
