using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Domain.Entity;

namespace EventStore.API.Test.Integration.Comparer
{
    public class OrderEqualityComparer : IEqualityComparer<Order>
    {
        public bool Equals([AllowNull] Order x, [AllowNull] Order y)
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
                                && x.OrderCustomerId == y.OrderCustomerId
                                && x.OrderProductId == y.OrderProductId
                                && y.TotalPrice.Currency == x.TotalPrice.Currency
                                && x.TotalPrice.Value == y.TotalPrice.Value;
        }

        public int GetHashCode([DisallowNull] Order obj)
        {
            return obj.GetHashCode();
        }
    }


    public class ProductEqualityComparer : IEqualityComparer<Product>
    {
        public bool Equals([AllowNull] Product x, [AllowNull] Product y)
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
                   && x.Stock == y.Stock
                   && x.ProductLocation.Url == y.ProductLocation.Url
                   && x.ProductMetadata.Title == y.ProductMetadata.Title
                   && y.Price.Currency == x.Price.Currency
                   && x.Price.Value == y.Price.Value;
        }

        public int GetHashCode([DisallowNull] Product obj)
        {
            return obj.GetHashCode();
        }
    }


    public class CustomerEqualityComparer : IEqualityComparer<Customer>
    {
        public bool Equals([AllowNull] Customer x, [AllowNull] Customer y)
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

        public int GetHashCode([DisallowNull] Customer obj)
        {
            return obj.GetHashCode();
        }
    }



}
