using FrequencyManagerCommon.Extensions;
using FrequencyManagerCommon.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace FrequencyManagerCommon.Helpers
{
    public class SdrPlusPlusHelper : IDisposable
    {
        #region Public Methods

        public List<Channel> LoadFromFile(string fileName)
        {
            var result = new List<Channel>();

            if (!File.Exists(fileName)) return result;

            var json = File.ReadAllText(fileName);

            var data = JsonConvert.DeserializeObject<JObject>(json);

            var groups = data?["lists"]?.Children();

            if (groups == null) return result;

            foreach (JProperty group in groups)
            {
                var channels = data?["lists"]?[group.Name]?["bookmarks"];

                if (channels == null) continue;

                foreach (JProperty channel in channels)
                {
                    var name = channel.Name;
                    long.TryParse(channel.Value["frequency"]?.ToString(), out long frequency);
                    long.TryParse(channel.Value["bandwidth"]?.ToString(), out long bandwidth);
                    int.TryParse(channel.Value["mode"]?.ToString(), out int mode);

                    result.Merge(new Channel 
                    {                       
                        Name = name,
                        GroupName = group.Name,
                        Frequency = frequency,
                        Bandwidth = bandwidth,
                        Modulation = GetModulation(mode),
                        IsFavorite = true,
                    });
                }
            }

            return result;
        }

        public void SaveToFile(IEnumerable<Channel> channels, string fileName)
        {
            File.WriteAllText(fileName, ChannelsToJson(channels));
        }

        public void Dispose() { }

        #endregion

        #region Private Methods

        private string ChannelToJson(Channel channel, bool addComma)
        {
            var sb = new StringBuilder();

            sb.AppendLine("\t\t\t\t\"" + channel.Name.Replace("'", "`").Replace("\"", "`").Replace(" ", "") + "\": {");
            sb.AppendLine("\t\t\t\t\t\"bandwidth\": " + channel.Bandwidth + ".0,");
            sb.AppendLine("\t\t\t\t\t\"frequency\": " + channel.Frequency + ".0,");
            sb.AppendLine("\t\t\t\t\t\"mode\": " + GetMode(channel.Modulation));
            sb.AppendLine("\t\t\t\t}" + (addComma ? "," : ""));

            return sb.ToString();
        }

        private string ChannelsToJson(IEnumerable<Channel> channels)
        {
            var groups = channels.Select(o => o.GroupName).Distinct().OrderBy(o => o).ToArray();

            var sb = new StringBuilder();

            sb.AppendLine("{");
            sb.AppendLine("\t\"bookmarkDisplayMode\": 1,");
            sb.AppendLine("\t\"lists\": {");

            var itemNo = 1;

            foreach (var group in groups)
            {
                sb.AppendLine("\t\t\"" + group + "\": {");
                sb.AppendLine("\t\t\t\"bookmarks\": {");


                var sbChannels = new StringBuilder();

                var groupEntries = channels.Where(o => o.GroupName == group);
                var lineNo = 1;

                foreach (var entry in groupEntries)
                {
                    sbChannels.Append(ChannelToJson(entry, lineNo++ < groupEntries.Count()));
                }

                sb.Append(sbChannels.ToString());
                sb.AppendLine("\t\t\t},");
                sb.AppendLine("\t\t\t\"showOnWaterfall\": true");
                sb.AppendLine("\t\t}" + (itemNo++ < groups.Count() ? "," : ""));
            }

            sb.AppendLine("\t},");
            sb.AppendLine("\t\"selectedList\": \"" + groups.First() + "\"");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string GetModulation(int mode)
        {
            switch (mode)
            {
                case 0: return "NFM";
                case 1: return "WFM";
                case 2: return "AM";
                case 3: return "DSB";
                case 4: return "USB";
                case 5: return "CW";
                case 6: return "LSB";
                case 7: return "RAW";
                default: return "WFM";
            }
        }

        private static int GetMode(string modulation)
        {
            switch (modulation)
            {
                case "NFM": return 0;
                case "WFM": return 1;
                case "AM": return 2;
                case "DSB": return 3;
                case "USB": return 4;
                case "CW": return 5;
                case "LSB": return 6;
                case "RAW": return 7;
                default: return 1;
            }
        }

        #endregion
    }
}
