using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.Domain.ValueObject
{
    public class Price
    {
        public int Value { get; set; }
        public string Currency { get; set; }
    }



}
