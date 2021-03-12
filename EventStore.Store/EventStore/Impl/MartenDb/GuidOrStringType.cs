using System;

namespace EventStore.Store.EventStore.Impl.MartenDb
{
    public class GuidOrStringType
    {
        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public GuidOrStringType(string value)
        {
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Value { get; }

        #endregion Public Properties

        #region Public Methods

        public static implicit operator Guid(GuidOrStringType m)
        {
            return Guid.Parse(m.Value);
        }

        public static implicit operator GuidOrStringType(string val)
        {
            return new(val);
        }

        public static implicit operator GuidOrStringType(Guid val)
        {
            return new(val.ToString());
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