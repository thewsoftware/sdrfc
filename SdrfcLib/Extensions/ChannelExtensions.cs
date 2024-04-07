using FrequencyManagerCommon.Models;
using System.Xml.Linq;

namespace FrequencyManagerCommon.Extensions
{
    public static class ChannelExtensions
    {
        public static void MergeTo(this IEnumerable<Channel> src, IEnumerable<Channel> dst)
        {
            if (src == null || dst == null) return;

            foreach (var channel in src)
            { 
            }
        }

        public static Channel? FindChannel(this IEnumerable<Channel> channels, Channel channel)
        {
            if (channels == null || channel == null) return null;

            return channels.FirstOrDefault(o => 
                o.Frequency == channel.Frequency &&
                o.GroupName.Equals(channel.GroupName, StringComparison.CurrentCultureIgnoreCase));
        }

        public static bool SameAs(this Channel src, Channel dst)
        {
            if (src == null || dst == null) return false;
            
            return src.GroupName.Equals(dst.GroupName, StringComparison.CurrentCultureIgnoreCase) &&
                src.Name.Equals(dst.Name, StringComparison.CurrentCultureIgnoreCase) &&
                src.Modulation.Equals(dst.Modulation, StringComparison.CurrentCultureIgnoreCase) &&
                src.Frequency == dst.Frequency &&
                src.Bandwidth == dst.Bandwidth &&
                src.Shift == dst.Shift;
        }

        public static void Update(this Channel dst, Channel src)
        {
            dst.Name = src.Name;
            dst.Modulation = src.Modulation;
            dst.Bandwidth = src.Bandwidth;
            dst.Shift = src.Shift;
        }

        public static Channel Clone(this Channel channel)
        {
            return new Channel
            {
                IsFavorite = channel.IsFavorite,
                Name = channel.Name,
                GroupName = channel.GroupName,
                Frequency = channel.Frequency,
                Bandwidth = channel.Bandwidth,
                Shift = channel.Shift,
                Modulation = channel.Modulation,
            };
        }

        public static (bool, bool) Merge(this List<Channel> channels, Channel channel)
        {
            var isUpdated = false;
            var isAdded = false;

            var existing = channels.FindChannel(channel);

            if (existing == null)
            {
                channels.Add(channel);
                isAdded = true;
            }
            else
            {
                if (!existing.SameAs(channel))
                {
                    existing.Update(channel);
                    isUpdated = true;
                }
            }

            return (isUpdated, isAdded);
        }

    }
}
