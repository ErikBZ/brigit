using System;
using System.Collections.Generic;
using System.Text;
using Brigit.Attributes;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit.Structure
{
    [Serializable]
    public class Choice : DomNode
    {
        public string[] Choices { get; set; }

        // once again i'll add the other ones later
        public Choice() :
            base()
        {
            this.Choices = new string[0];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for(int i=0;i<Choices.Length;i++)
            {
                string c = Choices[i];
                sb.Append(i);
                sb.Append(c);
                sb.Append('\n');
            }

            return sb.ToString();
        }

        // with this function we choose based on the index shown to the player. Prior to this
        // we can calculate what choices will be availabe to the player
        public override DomNode GetNext(int choice, DomTree scene)
        {
            DomNode next = null;

            // for now assume that all choices show up to the player
            if(choice != -1 || choice >= this.Children.Length)
            {
                next = this.Children[choice];
            }
            else
            {
                throw new Exception("Choice does not exist in this context");
            }

            return next;
        }

        public override bool Equals(object obj)
        {
            if(obj == null || !(obj is Choice))
            {
                return false;
            }

            Choice node = (Choice)obj;
            bool choicesAreEqual = base.Equals(node) &&
                this.Choices.Length == node.Choices.Length;

            return choicesAreEqual;
        }

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
 
    }
}
