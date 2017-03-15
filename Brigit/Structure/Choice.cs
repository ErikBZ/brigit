using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit.Structure
{
    [Serializable]
    public class Choice : DomNode
    {
        public string[] Choices { get; set; }
        public Dictionary<int, Dictionary<string, bool>> FlagsRasiedByChoices { get; set; }


        // once again i'll add the other ones later
        public Choice() :
            base()
        {
            this.Choices = new string[0];
            FlagsRasiedByChoices = new Dictionary<int, Dictionary<string, bool>>();
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

        /// <summary>
        /// Sets the global and local variables and 
        /// </summary>
        /// <param name="ch"></param>
        // TODO refactor this out since it will probably never be used
        public DomNode MakeChoice(string ch, DomTree scene)
        {
            int choice = -1;
            DomNode next = null;
            if(int.TryParse(ch, out choice))
            {

                Dictionary<string, bool> flags = FlagsRasiedByChoices[choice];
                foreach(KeyValuePair<string, bool> entry in flags)
                {
                    if(scene.GlobalFlags.ContainsKey(entry.Key))
                    {
                        scene.GlobalFlags[entry.Key] = entry.Value;
                    }
                    else if(scene.LocalFlags.ContainsKey(entry.Key))
                    {
                        scene.LocalFlags[entry.Key] = entry.Value;
                    }
                    else
                    {
                        throw new Exception("Flags set by this choice are not present in either Local or Global flags within the scene");
                    }
                }
            }
            else
            {
                // do something here?
            }
            return next;
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
    }
}
