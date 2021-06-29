using System;
using System.Runtime.CompilerServices;

namespace EventStore.Domain.ValueObject
{
    public class ProductLocation
    {
        #region Public Properties

        public string Url { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public ProductLocation(string url)
        {
            Url = url;
        }


        public ProductLocation() : this(String.Empty)
        {
        }



        #endregion Public Constructors
    }
}