using System;
using System.IO;
using System.Text;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit
{
    public class Parser
    {
	    public Parser()
	    {
	    }
    }

    /// <summary>
    /// A class that steps through 
    /// </summary>
    public class Eater
    {
        string data;
        int pos;

        public Eater(string data)
        {
            this.data = data;
            pos = 0;
        }

        public bool Complete()
        {
            return pos >= data.Length;
        }

        /// <summary>
        /// Peeks at the current char
        /// </summary>
        /// <returns>Returns the current char</returns>
        public char SniffChar()
        {
            // if pos is greater than the string length return the null terminator
            if (pos >= data.Length)
                return '\0';
            return data[pos];
        }

        /// <summary>
        /// Steps forward by 1 in the string
        /// </summary>
        public void ConsumeChar()
        {
            pos++;
        }

        /// <summary>
        /// Returns the current char and steps forward by one
        /// </summary>
        /// <returns>Returns the current char</returns>
        public char SpitChar()
        {
            char c = SniffChar();
            ConsumeChar();
            return c;
        }

        /// <summary>
        /// Func is pretty cool
        /// </summary>
        /// <param name="predicate"></param>
        public void EatWhile(Func<Char, bool> predicate)
        {
            while(predicate(SniffChar()))
            {
                ConsumeChar();
            }
        }

        /// <summary>
        /// I hope I'm using this right
        /// </summary>
        public void EatWhiteSpace()
        {
            Func<char, bool> p = delegate (char c) { return Char.IsWhiteSpace(c); };
            EatWhile(p);
        }

        /// <summary>
        /// Consumes a string of characters then spits them back up
        /// </summary>
        /// <param name="pred">A predicate that will test a char</param>
        /// <returns>Returns a string fufilling the predicate</returns>
        public string SpitUpWhile(Func<char, bool> pred)
        {
            StringBuilder sb = new StringBuilder();
            while(pred(SniffChar()))
            {
                sb.Append(SpitChar());
            }

            return sb.ToString();   
        }

        /// <summary>
        /// Eats only letters and then spits them back up
        /// </summary>
        /// <returns>A string of letters</returns>
        public string SpitUpAlpha()
        {
            Func<char, bool> p = delegate (char c) { return Char.IsLetter(c); };
            return SpitUpWhile(p);
        }
    }
}
