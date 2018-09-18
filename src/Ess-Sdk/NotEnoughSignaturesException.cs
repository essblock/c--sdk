using System;

namespace Ess.Sdk
{
    public class NotEnoughSignaturesException : Exception
    {
        public NotEnoughSignaturesException()
            : base()
        {

        }

        public NotEnoughSignaturesException(string message)
            : base(message)
        {

        }
    }
}
