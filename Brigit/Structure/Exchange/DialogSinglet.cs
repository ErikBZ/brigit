using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brigit.Structure.Exchange
{
    // a signle instance of a dialog quote, a Dialog can be split into multiple singlets
    // this is mostly used by the client side renderer
    public class DialogSinglet
    {
        public string Character { get; set; }
        public SpeechText Text { get; set; }

        public DialogSinglet(string character, SpeechText text)
        {
            Character = character;
            Text = text;
        }
    }
}