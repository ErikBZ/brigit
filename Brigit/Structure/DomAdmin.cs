using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Brigit.Structure
{
    /// <summary>
    /// Static class for loading and writing
    /// DomTree's and lists of DomTrees maybe even Characters
    /// </summary>
    // this can probably be deleted soon
    [Obsolete]
    public static class DomAdmin
    {
        /// <summary>
        /// Writes a single tree into a binary file
        /// </summary>
        /// <param name="tree"></param>
        public static void WriteDomTree(DomTree tree)
        {
            IFormatter iformatter = new BinaryFormatter();
            string folderPath = @"..\..\doms\" + tree.Name;
            Console.WriteLine(folderPath);
            Stream stream = new FileStream(folderPath, FileMode.Create,
                FileAccess.Write, FileShare.None);
            iformatter.Serialize(stream, tree);
            stream.Close();
        }

        /// <summary>
        /// Loads a DOM Tree from the disk
        /// </summary>
        /// <param name="path"></param>
        public static DomTree ReadDomTree(string path)
        {
            IFormatter iformatter = new BinaryFormatter();
            if(File.Exists(path))
            {
                Stream stream = new FileStream(path, FileMode.Open,
                    FileAccess.Read, FileShare.Read);
                DomTree tree = (DomTree)iformatter.Deserialize(stream);
                return tree;
            }
            return null;
        }

        public static DomTree ReadDomFromDialog()
        {
            // Create an instance of the file open dialog

            return null;
        }
    }
}
