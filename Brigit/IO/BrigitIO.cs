using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Brigit.Structure;

namespace Brigit.IO
{
    class BrigitIO
    {
        public static string[] ReadTomeFile(string path)
        {
            string[] tome = new string[0];
            if (File.Exists(path))
            {
                tome = File.ReadAllLines(path);
            }
            else
            {
                Console.WriteLine($"Tome file at {path} does not exist");
            }
            return tome;
        }

        /// <summary>
        /// Reads a domtree from the given file location
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DomTree ReadTree(string name)
        {
            DomTree tree;
            DataContractSerializer dcs = new DataContractSerializer(typeof(DomTree), null, 0x7FF, false, true, null);
            
            using (FileStream reader = File.Open(name, FileMode.Open))
            {
                tree = (DomTree)dcs.ReadObject(reader);
            }
            return tree;
        }
        
        /// <summary>
        /// Writes a DomTree to a file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tree"></param>
        public static void WriteTree(string name, DomTree tree)
        {
            // 5th parameters is the mantain references parameter
            DataContractSerializer dcs = new DataContractSerializer(tree.GetType(), null, 0x7FF, false, true, null);
            using (FileStream writer = File.Open(name, FileMode.Create))
            {
                dcs.WriteObject(writer, tree);
            }
        }

        public static void WriteDomNode(string name, DomNode node)
        {
            DataContractSerializer dcs = new DataContractSerializer(node.GetType(), null, 0x7FFF, false, true, null);
            using (FileStream writer = File.Open(name, FileMode.Create))
            {
                dcs.WriteObject(writer, node);
            }
        }
    }
}
