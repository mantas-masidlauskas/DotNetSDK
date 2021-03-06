﻿namespace JudoPayDotNet.Models
{
    /// <summary>
    /// A merchant
    /// </summary>
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
    public class MerchantModel
// ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// Gets or sets the judo identifier.
        /// </summary>
        /// <value>
        /// The judo identifier.
        /// </value>
        public string JudoId { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the trading.
        /// </summary>
        /// <value>
        /// The name of the trading.
        /// </value>
        public string TradingName { get; set; }

        /// <summary>
        /// Gets or sets the appears on statement as.
        /// </summary>
        /// <value>
        /// The appears on statement as.
        /// </value>
        public string AppearsOnStatementAs { get; set; }
    }
    // ReSharper restore UnusedMember.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
}
