using FrequencyManagerCommon.Extensions;
using FrequencyManagerCommon.Models;
using System.Xml;

namespace FrequencyManagerCommon.Helpers
{
    public class SdrSharpHelper : IDisposable
    {
        #region Public Methods

        public List<Channel> LoadFromFile(string fileName)
        {
            var result = new List<Channel>();

            if (!File.Exists(fileName)) return result;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var root = xmlDoc.DocumentElement;

            var items = root?.SelectNodes("//MemoryEntry");

            if (items == null || items.Count == 0) return result;

            foreach (XmlNode item in items)
            {
                if (items == null) continue;

                var name = item.SelectSingleNode(@".//Name")?.InnerText;
                var groupName = item.SelectSingleNode(@".//GroupName")?.InnerText;

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(groupName)) continue;

                var modulation = item.SelectSingleNode(@".//DetectorType")?.InnerText ?? "WFM";
                bool.TryParse(item.SelectSingleNode(@".//IsFavourite")?.InnerText, out bool isFavorite);
                long.TryParse(item.SelectSingleNode(@".//Frequency")?.InnerText, out long frequency);
                long.TryParse(item.SelectSingleNode(@".//Shift")?.InnerText, out long shift);
                long.TryParse(item.SelectSingleNode(@".//FilterBandwidth")?.InnerText, out long bandwidth);

                result.Merge(new Channel {
                    IsFavorite = isFavorite,
                    Name = name,
                    GroupName = groupName,
                    Frequency = frequency,
                    Modulation = modulation,
                    Shift = shift,
                    Bandwidth = bandwidth,
                });
            }

            return result; 
        }

        public void SaveToFile(IEnumerable<Channel> channels, string fileName)
        {
            var memoryEntries = ChannelsToMemoryEntries(channels);

            var xml = memoryEntries.Serialize();

            File.WriteAllText(fileName, xml);
        }

        public void Dispose() { }

        #endregion

        #region Private Methods

        private MemoryEntry[] ChannelsToMemoryEntries(IEnumerable<Channel> channels)
        {
            return channels.Select(o =>
                new MemoryEntry
                {
                    IsFavourite = o.IsFavorite,
                    Name = o.Name,
                    GroupName = o.GroupName,
                    Frequency = o.Frequency,
                    FilterBandwidth = o.Bandwidth,
                    DetectorType = o.Modulation,
                }
            ).ToArray();
        }

        #endregion
    }
}
