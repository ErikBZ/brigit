using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brigit.Parser.Stream
{
	/// <summary>
	/// Removes comments from a given array of strings
	/// </summary>
	public static class CommentRemover
	{
		// as of right now only hashs will work correctly
		public static string[] RemoveComments(string[] text)
		{
			List<string> newTome = new List<string>();

			for (int i = 0; i < text.Length; i++)
			{
				string remaining = text[i];
				if (text[i] != null && text[i].Contains('#'))
				{
					int index = text[i].IndexOf('#');
					// only adding in the left side of the hash
					remaining = text[i].Split('#')[0];
				}

				OnlyAddIfNotNullOrWhiteSpace(newTome, remaining);
			}

			return newTome.ToArray();
		}

		public static void OnlyAddIfNotNullOrWhiteSpace(List<string> list, string str)
		{
			if (!String.IsNullOrEmpty(str) || !(str != null && str.All(char.IsWhiteSpace)))
			{
				list.Add(str);
			}
		}
	}
}
