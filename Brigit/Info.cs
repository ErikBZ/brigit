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
        public Descision Descision;
        public DialogSinglet Dialog;

        public Info()
        {
            type = Type.None;
        }

        // i've decided to make the node data it self viewable
        public Info(Descision choices)
        {
            type = Type.Descision;
            Descision = choices;
        }

        public Info(Dialog dialog, int speechBlock)
        {
            DialogSinglet singlet = new DialogSinglet(dialog.Character, dialog.Text[speechBlock]);
            type = Type.Dialog;
            Dialog = singlet;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Character: ");
            return sb.ToString();
        }
    }
}
