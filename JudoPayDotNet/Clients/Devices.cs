using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JudoPayDotNet.Http;
using JudoPayDotNet.Logging;
using JudoPayDotNet.Models;

namespace JudoPayDotNet.Clients
{
    public interface IDevices
    {
        /// <summary>
        /// Gets blacklisted devices
        /// </summary>
        /// <returns>blacklisted devices as requested</returns>
        Task<IResult<BlacklistResults>> Get();

        /// <summary>
        /// Adds a device to the blacklist
        /// </summary>
        /// <param name="deviceIdentifier">String that uniquely identifies the device you are blacklisting</param>
        /// <returns>blacklisted devices as requested</returns>
        Task<IResult<DeviceBlacklistEntry>> Create(string deviceIdentifier);
    }

    internal class Devices : JudoPayClient, IDevices
    {
        private const string ADDRESS = "blacklists/devices/";

        public Devices(ILog logger, IClient client) : base(logger, client)
        {
        }

        public Task<IResult<BlacklistResults>> Get()
        {
            return GetInternal<BlacklistResults>(ADDRESS);
        }

        public Task<IResult<DeviceBlacklistEntry>> Create(string deviceIdentifier)
        {
            return PostInternal<object, DeviceBlacklistEntry>(ADDRESS + deviceIdentifier, null);
        }
    }
}
