using FrequencyManagerCommon.Extensions;
using FrequencyManagerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyManagerCommon
{
    public class FrequencyManager : IDisposable
    {
        #region Private Members

        private readonly List<Channel> _channels;

        #endregion

        #region Public Members

        public Channel[] Channels { get { return _channels.ToArray();  } }

        #endregion

        #region Constructor

        public FrequencyManager(List<Channel> channels) 
        {
            _channels = channels;
        }

        #endregion

        #region Public Methods

        public (long, long) Merge(IEnumerable<Channel> inputChannels)
        {
            var updated = 0;
            var added = 0;
            
            foreach(var channel in inputChannels
                .OrderBy(o => o.GroupName)
                .ThenBy(o => o.Frequency)
                .ThenBy(o => o.Name)) 
            {
                (bool isUpdated, bool isAdded) = _channels.Merge(channel);

                updated = updated + (isUpdated ? 1 : 0);
                added = added + (isAdded ? 1 : 0);
            }

            return (updated, added);
        }

        public (long, long) Copy(IEnumerable<Channel> inputChannels)
        {
            _channels.Clear();
            _channels.AddRange(inputChannels.Select(o => o.Clone()));
            return (0, _channels.Count());
        }

        public void Dispose() { }

        #endregion

        #region Private Methods



        #endregion
    }
}
