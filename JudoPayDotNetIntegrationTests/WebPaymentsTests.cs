﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using HtmlAgilityPack;
using JudoPayDotNet.Models;
using NUnit.Framework;

namespace JudoPayDotNetIntegrationTests
{
    [TestFixture]
    public class WebPaymentsTests : IntegrationTestsBase
    {
        [Test]
        public void PaymentCreate()
        {
            var request = GetWebPaymentRequestModel();

            var result = JudoPayApiElevated.WebPayments.Payments.Create(request).Result;

            Assert.NotNull(result);
            Assert.IsFalse(result.HasError);
            Assert.NotNull(result.Response);
            Assert.NotNull(result.Response.Reference);
            Assert.NotNull(result.Response.PostUrl);
        }

        [Test]
        public void PaymentUpdate()
        {
            var request = GetWebPaymentRequestModel();

            var result = JudoPayApiElevated.WebPayments.Payments.Create(request).Result;

            Assert.NotNull(result);
            Assert.IsFalse(result.HasError);
            Assert.NotNull(result.Response);
            Assert.NotNull(result.Response.Reference);
            Assert.NotNull(result.Response.PostUrl);

            request.Status = WebPaymentStatus.Success;
            request.Reference = result.Response.Reference;

            var resultUpdate = JudoPayApiElevated.WebPayments.Payments.Update(request).Result;

            Assert.NotNull(resultUpdate);
            Assert.IsFalse(resultUpdate.HasError);
            Assert.NotNull(resultUpdate.Response); //todo pick a judoID that has permissions to do this
            Assert.NotNull(resultUpdate.Response.Reference);
            Assert.AreEqual(result.Response.Reference, resultUpdate.Response.Reference);
            Assert.AreEqual(resultUpdate.Response.Status, resultUpdate.Response.Status);
        }

        [Test]
        public void PreAuthCreate()
        {
            var request = GetWebPaymentRequestModel();

            var result = JudoPayApiElevated.WebPayments.PreAuths.Create(request).Result;

            Assert.NotNull(result);
            Assert.IsFalse(result.HasError);
            Assert.NotNull(result.Response);
            Assert.NotNull(result.Response.Reference);
            Assert.NotNull(result.Response.PostUrl);
        }

        [Test]
        public void PreAuthUpdate()
        {
            var request = GetWebPaymentRequestModel();

            var result = JudoPayApiElevated.WebPayments.PreAuths.Create(request).Result;

            Assert.NotNull(result);
            Assert.IsFalse(result.HasError);
            Assert.NotNull(result.Response);
            Assert.NotNull(result.Response.Reference);
            Assert.NotNull(result.Response.PostUrl);

            request.Status = WebPaymentStatus.Success;
            request.Reference = result.Response.Reference;

            var resultUpdate = JudoPayApiElevated.WebPayments.PreAuths.Update(request).Result;

            Assert.NotNull(resultUpdate);
            Assert.IsFalse(resultUpdate.HasError);
            Assert.NotNull(resultUpdate.Response);
            Assert.NotNull(resultUpdate.Response.Reference);
            Assert.AreEqual(result.Response.Reference, resultUpdate.Response.Reference);
            Assert.AreEqual(resultUpdate.Response.Status, resultUpdate.Response.Status);
        }

        [Test]
        public void TransactionsGetByReference()
        {
            var request = GetWebPaymentRequestModel();

            var result = JudoPayApiElevated.WebPayments.Payments.Create(request).Result;

            Assert.NotNull(result);
            Assert.IsFalse(result.HasError);
            Assert.NotNull(result.Response);
            Assert.NotNull(result.Response.Reference);
            Assert.NotNull(result.Response.PostUrl);

            var webRequest = JudoPayApiElevated.WebPayments.Transactions.Get(result.Response.Reference).Result;

            Assert.NotNull(webRequest);
            Assert.IsFalse(webRequest.HasError);
            Assert.NotNull(webRequest.Response);
            Assert.NotNull(webRequest.Response.Reference);
            Assert.AreEqual(result.Response.Reference, webRequest.Response.Reference);
            Assert.AreEqual(request.JudoId, webRequest.Response.JudoId);
        }

        [Test]
        public void TransactionsGetByReceiptId()
        {
            var request = GetWebPaymentRequestModel();

            var result = JudoPayApi.WebPayments.Payments.Create(request).Result;

            var reference = result.Response.Reference;

            // Forms - Post a form with credentials to post url from the webpayment response passing form parameter Reference

            var httpClient = new HttpClient();
            var formContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("Reference", reference)
            });

            var formRequest = CreateJudoApiRequest(result.Response.PostUrl, HttpMethod.Post, "5.3.0.0", Configuration.ElevatedPrivilegesToken,
                Configuration.ElevatedPrivilegesSecret);

            formContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            formRequest.Content = formContent;

            var paymentPage = httpClient.SendAsync(formRequest).Result;

            var resultBody = paymentPage.Content.ReadAsStringAsync().Result;

            /* ok this next bit is a hack. I know on Iridium's ACS simulator the PaRes value is lurking in the HTML (It's a simulator after all!) */
            var doc = new HtmlDocument();
            doc.LoadHtml(resultBody);

            // Forms - Post a form with credentials and cookie and form following variables:
            // url= /v1/Pay variables: CardNumber, ExpiryDate, Cv2, form variable: __RequestVerificationToken

            var formField = doc.DocumentNode.SelectSingleNode("//input[@name='__RequestVerificationToken']");

            var requestVerificationToken = formField.GetAttributeValue("value", "");

                // CardNumber = "4976000000003436",
                // CV2 = "452",
                // ExpiryDate = "12/20",

            formContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("__RequestVerificationToken", requestVerificationToken),
                new KeyValuePair<string, string>("CardNumber", "4976000000003436"),
                new KeyValuePair<string, string>("Cv2", "452"), 
                new KeyValuePair<string, string>("CardAddress.CountryCode", "826"), 
                new KeyValuePair<string, string>("CardAddress.PostCode", "TR14 8PA"), 
                new KeyValuePair<string, string>("ExpiryDate", "12/20"), 
                new KeyValuePair<string, string>("Reference", reference)
            });

            formRequest = CreateJudoApiRequest(Configuration.WebpaymentsUrl, HttpMethod.Post, "5.3.0.0", Configuration.ElevatedPrivilegesToken,
                Configuration.ElevatedPrivilegesSecret);

            formContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            formRequest.Content = formContent;

            var resultPage = httpClient.SendAsync(formRequest).Result;

            resultBody = resultPage.Content.ReadAsStringAsync().Result;

            // Retrieve from the response the receipt id

            doc = new HtmlDocument();
            doc.LoadHtml(resultBody);

            formField = doc.DocumentNode.SelectSingleNode("//input[@name='ReceiptId']");

            var receiptId = formField.GetAttributeValue("value", "");

            var webRequest = JudoPayApi.WebPayments.Transactions.GetByReceipt(receiptId).Result;

            Assert.NotNull(webRequest);
            Assert.IsFalse(webRequest.HasError);
            Assert.NotNull(webRequest.Response);
            Assert.NotNull(webRequest.Response.Reference);
            Assert.AreEqual(request.JudoId, webRequest.Response.JudoId);
        }

        private HttpRequestMessage CreateJudoApiRequest(string url, HttpMethod method, string apiVersion, string apiToken, string apiSecret)
        {
            var request = new HttpRequestMessage(method, url);

            var full = string.Format("{0}:{1}", apiToken, apiSecret);

            var authDetails = Encoding.GetEncoding("iso-8859-1").GetBytes(full);
            var parameter = Convert.ToBase64String(authDetails);

            request.Headers.Add("Accept", "application/json");

            request.Headers.Add("Api-Version", apiVersion);

            request.Headers.Add("User-Agent", "Mozilla");

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", parameter);

            return request;
        }
    }
}
