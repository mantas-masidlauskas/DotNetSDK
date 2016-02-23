using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using JudoPayDotNet.Errors;

namespace JudoPayDotNet.Models
{
    /// <summary>
    /// A list of blacklisted items (see <see cref="BlacklistEntryModel"/>)
    /// </summary>
    [DataContract(Name = "SearchResults", Namespace = "")]
    public class BlacklistResults
    {
        /// <summary>
        /// Gets or sets the result count.
        /// </summary>
        /// <value>
        /// The result count.
        /// </value>
        [DataMember]
        public long ResultCount { get; set; }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        [DataMember]
        public long PageSize { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        [DataMember]
        public long Offset { get; set; }

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        [DataMember]
        public IEnumerable<DeviceBlacklistEntry> Results { get; set; }
    }
}
