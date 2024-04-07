namespace FrequencyManagerCommon.Models
{
    public class MemoryEntry
    {
        public bool IsFavourite { get; set; }
        public string Name { get; set; }    
        public string GroupName { get; set; }
        public long Frequency { get; set; }
        public string DetectorType { get; set; }
        public long Shift { get; set; }
        public long FilterBandwidth { get; set; }
    }
}
