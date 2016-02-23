using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JudoPayDotNet.Clients.Consumer;
using JudoPayDotNet.Http;
using JudoPayDotNet.Logging;
using JudoPayDotNet.Models;

namespace JudoPayDotNet.Clients
{
    /// <summary>
    /// The entity responsible for blacklisting entities in order to prevent payments
    /// </summary>
    public interface IBlacklists
    {
        /// <summary>
		/// Allows you to create / delete and list blacklisted devices
		/// </summary>
		IDevices Devices { get; set; }

        /// <summary>
        /// Allows you to create / delete and list blacklisted consumers
        /// </summary>
        IConsumers Consumers { get; set; }
    }

    internal class Blacklists : IBlacklists
    {
        /// <summary>
		/// Allows you to create / delete and list blacklisted devices
		/// </summary>
		public IDevices Devices { get; set; }

        /// <summary>
        /// Allows you to create / delete and list blacklisted consumers
        /// </summary>
        public IConsumers Consumers { get; set; }
    }
}
