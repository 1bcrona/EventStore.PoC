using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Domain.Entity;
using EventStore.Domain.Event.Infrastructure;

namespace EventStore.Domain.Event.Impl
{
    public class UserCreated : Event<User>
    {
    }
}
