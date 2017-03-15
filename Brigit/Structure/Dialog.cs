using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit.Structure
{
    [Serializable]
    public class Dialog : DomNode
    {
        public string speechText;

        public string Text
        {
            get { return speechText; }
            set { speechText = value; }
        }


        // i'll add the other ones later
        public Dialog():
            base()
        {
            this.speechText = string.Empty;
        }

        public override string ToString()
        {
            return this.speechText;
        }

        public override bool Equals(object obj)
        {
            if(obj == null || !(obj is Dialog))
            {
                return false;
            }
            Dialog node = (Dialog)obj;

            bool dialogsAreEqual = base.Equals(obj) && speechText.Equals(node.speechText);
            return dialogsAreEqual;
        }
    }
}
