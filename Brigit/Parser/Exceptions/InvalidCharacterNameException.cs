using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brigit.Parser.Exceptions
{
	class InvalidCharacterNameException: Exception
	{
		public InvalidCharacterNameException()
		{
		}

		public InvalidCharacterNameException(string message)
			: base(message)
		{
		}

		public InvalidCharacterNameException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
