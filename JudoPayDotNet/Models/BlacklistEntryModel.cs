using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudoPayDotNet.Models
{
    /// <summary>
    /// An entry in one of our blacklists
    /// </summary>
    public abstract class BlackListEntryModel
    {
        /// <summary>
        /// The date and time this list entry was created
        /// </summary>
        public DateTime BlacklistedAt { get; set; }
    }
}
