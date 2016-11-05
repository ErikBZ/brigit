using System;

namespace Brigit
{
    namespace ParserExceptions
    {
        public class NoCommentEndException : Exception
        {
            public NoCommentEndException()
            {
            }

            public NoCommentEndException(string message) :
                base(message)
            {
            }

            public NoCommentEndException(string message, Exception inner) :
                base(message, inner)
            {
            }
        }

        public class NoCommnetExsistsException : Exception
        {
            public NoCommnetExsistsException()
            {
            }

            public NoCommnetExsistsException(string message) :
                base(message)
            {
            }

            public NoCommnetExsistsException(string message, Exception inner) :
                base(message, inner)
            {
            }
        }
    }
}
