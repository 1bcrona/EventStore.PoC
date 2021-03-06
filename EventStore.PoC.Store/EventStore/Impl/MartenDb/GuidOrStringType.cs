using System;

namespace EventStore.PoC.Store.EventStore.Impl.MartenDb
{
    public class GuidOrStringType
    {
        #region Private Fields

        private readonly string _Value;

        #endregion Private Fields

        #region Public Constructors

        public GuidOrStringType(string value)
        {
            _Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Value => _Value;

        #endregion Public Properties

        #region Public Methods

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

        public static implicit operator string(GuidOrStringType m)
        {
            return m.Value;
        }

        public static GuidOrStringType Parse(Guid val)
        {
            return val;
        }

        public static GuidOrStringType Parse(string val)
        {
            return val;
        }

        #endregion Public Methods
    }
}