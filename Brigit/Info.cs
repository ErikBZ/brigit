using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brigit.Structure.Exchange;

namespace Brigit
{
    /// <summary>
    /// Holds all the info to be shown on screen
    /// </summary>
    public class Info
    {
        public enum Type { None, Dialog, Descision };
        public Type type;

        // all attributes are part of the singlet and descisions blocks already
		public ExchangeUnit Data;

        public Info()
        {
            type = Type.None;
        }

        // i've decided to make the node data it self viewable
        public Info(Decision choices)
        {
            type = Type.Descision;
            Data = choices;
        }

        public Info(Dialog dialog, int speechBlock)
        {
            DialogSinglet singlet = new DialogSinglet(dialog.Character, dialog.Text[speechBlock]);
            type = Type.Dialog;
            Data = singlet;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            switch (type)
            {
                case Type.Descision:
					var descision = Data as Decision;
                    for(int i=0;i<descision.Choices.Count;i++)
                    {
                        sb.Append(String.Format("{0}: {1}", i, descision.Choices[i]));
                        if(i < descision.Choices.Count - 1)
                        {
                            sb.Append("\n");
                        }
                    }
                    break;
                case Type.Dialog:
					var dialog = Data as DialogSinglet;
                    sb.Append(String.Format("{0}: {1}", dialog.Character, dialog.Text));
                    break;
            }

            return sb.ToString();
        }
    }
}
