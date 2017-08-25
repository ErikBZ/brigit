using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brigit
{
    /// <summary>
    /// Holds all the info to be shown on screen
    /// </summary>
    public class Info
    {
        public enum Type { Choice, Dialog };

        public Type type;
        public string Character;
        /// <summary>
        /// Lists of choices when this class responds with a choice
        /// </summary>
        public List<string> Text;

        // extra stuff like "emotion", "time" and shit like that that are part of attribute manager

        public Info()
        {
            type = Type.Dialog;
            Character = string.Empty;
            Text= new List<string>();
        }
    }
}
