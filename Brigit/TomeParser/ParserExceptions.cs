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

        public class BranchTagExpectedException : Exception
        {
            public BranchTagExpectedException()
            {
            }

            public BranchTagExpectedException(string message) :
                base(message)
            {
            }

            public BranchTagExpectedException(string message, Exception inner) :
                base(message, inner)
            {
            }
        }

        public class TagDoesNotExistException : Exception
        {
            public TagDoesNotExistException()
            {
            }

            public TagDoesNotExistException(string message) :
                base(message)
            {
            }

            public TagDoesNotExistException(string message, Exception inner) :
                base(message, inner)
            {
            }
        }
        public class BranchIdDoesNotMatchException : Exception
        {
            public BranchIdDoesNotMatchException()
            {
            }

            public BranchIdDoesNotMatchException(string message) :
                base(message)
            {
            }

            public BranchIdDoesNotMatchException(string message, Exception inner) :
                base(message, inner)
            {
            }
        }
    }
}
