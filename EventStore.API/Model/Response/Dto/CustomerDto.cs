using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Domain.Entity;

namespace EventStore.API.Model.Response.Dto
{
    public class CustomerDto
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public static implicit operator CustomerDto(Customer customer)
        {
            return new() { Id = customer.Id == Guid.Empty ? null : customer.Id.ToString(), Address = customer.Address, Surname = customer.Surname, Name = customer.Name };
        }
    }
}
