using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Brigit.Attributes;

namespace Brigit.Structure.Exchange
{
	[DataContract]
	public class Dialog
	{
		[DataMember]
		public string Character { get; set; }
		[DataMember]
		public AttributeManager Attributes { get; set; }
		[DataMember]
		public List<SpeechText> Text { get; set; }

		public Dialog()
		{
			Character = string.Empty;
			Attributes = new AttributeManager();
			Text = new List<SpeechText>();
		}

		// for easier testing and stuff but i could use it at the end of parsing
		public Dialog(string character, params string[] speech)
		{
			Attributes = new AttributeManager();
			Character = character;
			Text = new List<SpeechText>();

			foreach (string str in speech)
			{
				Text.Add(new SpeechText(str));
			}
		}

		public override bool Equals(object obj)
		{
			Dialog other;
			if(obj is Dialog)
			{
				other = obj as Dialog;	
			}
			else
			{
				return false;
			}

			bool equal = Character.Equals(other.Character);
			for (int i = 0; (i < Text.Count) && equal; i++)
			{
				if(i >= other.Text.Count)
				{
					equal = false;
				}
				else
				{
					equal &= Text[i].Equals(other.Text[i]);
				}
			}

			return equal;
		}

		public override int GetHashCode()
		{
            int hash = 0;
            foreach(SpeechText st in Text)
            {
                hash ^= st.GetHashCode();
            }
            hash = (~0xf & hash) | Text.Count;
            return hash;
		}
	}
}
