using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JudoPayDotNet;
using JudoPayDotNet.Models;
using JudoPayDotNetDotNet;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JudoPayDotNetIntegrationTests
{
    [TestFixture]
    public class BlacklistTests
    {
        private JudoPayApi _judo;
        private readonly string _kDeviceId = Guid.NewGuid().ToString();
        private readonly string _vDeviceId = Guid.NewGuid().ToString();
        private readonly string _consumerReference = Guid.NewGuid().ToString();
        private string _deviceId;

        [OneTimeSetUp]
        public void Init()
        {
            _judo = JudoPaymentsFactory.Create(Configuration.Token, Configuration.Secret, Configuration.Baseaddress);

            var paymentWithCard = new CardPaymentModel
            {
                JudoId = Configuration.Judoid,
                YourConsumerReference = _consumerReference,
                Amount = 25,
                CardNumber = "4976000000003436",
                CV2 = "452",
                ExpiryDate = "12/20",
                CardAddress = new CardAddressModel
                {
                    Line1 = "Test Street",
                    PostCode = "W40 9AU",
                    Town = "Town"
                },
                ClientDetails = JObject.FromObject(new Dictionary<string, string>
                {
                    {"kdeviceid", _kDeviceId},
                    {"vdeviceid", _vDeviceId}
                })
            };
            var response = _judo.Payments.Create(paymentWithCard).Result;
            _deviceId = (response.Response as PaymentReceiptModel).DeviceIdentifier;
        }

        [Test]
        public async Task CreateBlacklistDevice()
        {
            var actual = await _judo.Blacklists.Devices.Create(_deviceId);

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Response, Is.Not.Null);
            Assert.That(actual.Response.DeviceIdentifier, Is.EqualTo(_deviceId));
        }

        [Test]
        public async Task GetBlacklistDevices()
        {
            var actual = await _judo.Blacklists.Devices.Get();

            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Response, Is.Not.Null);
            Assert.That(actual.Response.ResultCount, Is.GreaterThan(0));
            Assert.That(actual.Response.Results, Has.Count.GreaterThan(0));
        }
    }
}
