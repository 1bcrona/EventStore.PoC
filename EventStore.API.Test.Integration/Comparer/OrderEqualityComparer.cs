using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.API.Model.Response.Dto;
using EventStore.Domain.Entity;

namespace EventStore.API.Test.Integration.Comparer
{
    public class OrderEqualityComparer : IEqualityComparer<OrderDto>
    {
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
    }


    public class ProductEqualityComparer : IEqualityComparer<ProductDto>
    {
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
    }


    public class CustomerEqualityComparer : IEqualityComparer<CustomerDto>
    {
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
    }



}
