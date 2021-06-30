using System;

namespace EventStore.Domain.ValueObject
{
    public class Price
    {
        #region Public Properties

        public string Currency { get; set; }
        public decimal Value { get; set; }

        #endregion Public Properties

    }
}