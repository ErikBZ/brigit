using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brigit.Parser.Stream
{
	// holds the string as an array of strings where the index
	// is the line number
	public class TomeStream
	{
		string[] stream;
		int lineNumber;
		int positionNumber;

		public string Position
		{
			get { return $"Line: {lineNumber} Posiiton: {positionNumber}"; }
		}

		public TomeStream()
		{
			stream = new string[0];
			lineNumber = 0;
			positionNumber = 0;
		}

		public TomeStream(string[] lines)
		{
			stream = lines;
			lineNumber = 0;
			positionNumber = 0;
		}

		public bool Complete()
		{
			return lineNumber == stream.Length ||
				(lineNumber == stream.Length - 1 && positionNumber == stream[lineNumber].Length);
		}

		public char PeekChar()
		{
			if (!Complete())
			{
				return stream[lineNumber][positionNumber];
			}
			else
			{
				// reutrn null terminator
				return '\0';
			}
		}

		public void SkipChar()
		{
			if (!Complete())
			{
				// if position number is equal to the lenght of the string at the stream line
				// then it will roll back to 0 because of the mod
				positionNumber = (positionNumber + 1) % stream[lineNumber].Length;
				if (positionNumber == 0)
				{
					lineNumber++;
				}
			}
			else
			{
				throw new Exception("End of stream reached");
			}
		}

		public char NextChar()
		{
			char curr = PeekChar();
			SkipChar();
			return curr;
		}

		public override string ToString()
		{
			return Position;
		}
	}
}
