using EventStore.Domain.Entity;
using System;

namespace EventStore.API.Model.Response.Dto
{
    public class CustomerDto : BaseDto
    {
        #region Public Properties

        public string Address { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static implicit operator CustomerDto(Customer customer)
        {
            return customer == null ? null : new CustomerDto
            {
                Id = customer.Id == Guid.Empty ? null : customer.Id.ToString(),
                Address = customer.Address,
                Surname = customer.Surname,
                Name = customer.Name,
                Active = customer.Active
            };
        }

        #endregion Public Methods

        public override bool Active { get; set; }
    }
}