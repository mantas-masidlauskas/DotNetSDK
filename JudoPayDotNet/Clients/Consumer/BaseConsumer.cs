﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JudoPayDotNet.Http;
using JudoPayDotNet.Logging;
using JudoPayDotNet.Models;

namespace JudoPayDotNet.Clients.Consumer
{
	internal abstract class BaseConsumers : JudoPayClient
    {
        private const string ADDRESS = "consumers";

        protected BaseConsumers(ILog logger, IClient client) : base(logger, client)
        {
        }

		/// <summary>
		/// Returns previous transactions for a consumer, paged and optionally filtered by transaction type
		/// </summary>
		/// <param name="consumerToken"></param>
		/// <param name="transactionType"></param>
		/// <param name="pageSize"></param>
		/// <param name="offset"></param>
		/// <param name="sort"></param>
		/// <returns></returns>
        protected Task<IResult<PaymentReceiptResults>> GetTransactions(string consumerToken, string transactionType = null,
                                                                    long? pageSize = null, long? offset = null, TransactionListSorts? sort = null)
        {
            var address = string.Format("{0}/{1}", ADDRESS, consumerToken);

            if (!String.IsNullOrWhiteSpace(transactionType))
            {
                address = String.Format("{0}/{1}", address, transactionType);
            }

            var parameters = new Dictionary<string, string>();

            AddParameter(parameters, "pageSize", pageSize);
            AddParameter(parameters, "offset", offset);
            AddParameter(parameters, "sort", sort);

            return GetInternal<PaymentReceiptResults>(address, parameters);
        }
    }
}
