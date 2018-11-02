using System;
using System.Runtime.Serialization;

namespace BrnMall.Core
{
    /// <summary>
    /// BrnMall“Ï≥£¿‡
    /// </summary>
    [Serializable]
    public class BMAException : ApplicationException
    {
        public BMAException() { }

        public BMAException(string message) : base(message) { }

        public BMAException(string message, Exception inner) : base(message, inner) { }

        protected BMAException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
