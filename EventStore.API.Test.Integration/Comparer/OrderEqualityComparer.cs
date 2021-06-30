using EventStore.API.Model.Response.Dto;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EventStore.API.Test.Integration.Comparer
{
    public class CustomerEqualityComparer : IEqualityComparer<CustomerDto>
    {
        #region Public Methods

        public bool Equals([AllowNull] CustomerDto x, [AllowNull] CustomerDto y)
        {
            if (x == null ^ y == null)
            {
                return false;
            }

            if (x == null && y == null)
            {
                return true;
            }

            return x.Id == y.Id
                   && x.Address == y.Address
                   && x.Name == y.Name
                   && x.Surname == y.Surname;
        }

        public int GetHashCode([DisallowNull] CustomerDto obj)
        {
            return obj.GetHashCode();
        }

        #endregion Public Methods
    }

    public class OrderEqualityComparer : IEqualityComparer<OrderDto>
    {
        #region Public Methods

        public bool Equals([AllowNull] OrderDto x, [AllowNull] OrderDto y)
        {
            if (x == null ^ y == null)
            {
                return false;
            }

            if (x == null && y == null)
            {
                return true;
            }

            return x.Id == y.Id
                   && x.Quantity == y.Quantity
                                && new CustomerEqualityComparer().Equals(x.Customer, y.Customer)
                                && new ProductEqualityComparer().Equals(x.Product, y.Product)
                                && y.TotalPrice.Currency == x.TotalPrice.Currency
                                && x.TotalPrice.Value == y.TotalPrice.Value;
        }

        public int GetHashCode([DisallowNull] OrderDto obj)
        {
            return obj.GetHashCode();
        }

        #endregion Public Methods
    }

    public class ProductEqualityComparer : IEqualityComparer<ProductDto>
    {
        #region Public Methods

        public bool Equals([AllowNull] ProductDto x, [AllowNull] ProductDto y)
        {
            if (x == null ^ y == null)
            {
                return false;
            }

            if (x == null && y == null)
            {
                return true;
            }

            return x.Id == y.Id
                   && x.ProductLocation.Url == y.ProductLocation.Url
                   && x.ProductMetadata.Title == y.ProductMetadata.Title
                   && y.Price.Currency == x.Price.Currency
                   && x.Price.Value == y.Price.Value;
        }

        public int GetHashCode([DisallowNull] ProductDto obj)
        {
            return obj.GetHashCode();
        }

        #endregion Public Methods
    }
}