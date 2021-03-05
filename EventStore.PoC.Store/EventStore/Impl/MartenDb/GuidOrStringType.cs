using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace EventStore.PoC.Store.EventStore.Impl.MartenDb
{
    public class GuidOrStringType
    {

        public string Value => _Value;

        private readonly string _Value;


        public GuidOrStringType(string value)
        {
            _Value = value;
        }
        public static implicit operator string(GuidOrStringType m)
        {
            return m.Value;
        }

        public static implicit operator Guid(GuidOrStringType m)
        {
            return Guid.Parse(m.Value);
        }

        public static implicit operator GuidOrStringType(string val)
        {
            return new GuidOrStringType(val);
        }

        public static implicit operator GuidOrStringType(Guid val)
        {
            return new GuidOrStringType(val.ToString());
        }


        public static GuidOrStringType Parse(Guid val)
        {
            return val;
        }


        public static GuidOrStringType Parse(string val)
        {
            return val;
        }

    }




}
