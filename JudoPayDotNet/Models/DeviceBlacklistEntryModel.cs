using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudoPayDotNet.Models
{
    /// <summary>
    /// This is an API model representing an entry in a device blacklist
    /// </summary>
    public class DeviceBlacklistEntry : BlackListEntryModel
    {
        /// <summary>
        /// The consumer token for this consumer
        /// </summary>
        /// <remarks>Consumer tokens are URL safe</remarks>
        public string DeviceIdentifier { get; set; }
    }
}
