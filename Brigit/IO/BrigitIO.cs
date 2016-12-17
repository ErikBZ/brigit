using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Brigit.IO
{
    class BrigitIO
    {
        public static DomTree ReadTree(string name)
        {
            DomTree tree;
            XmlSerializer xml = new XmlSerializer(typeof(DomTree));

            using (var reader = new StreamReader(name))
            {
                tree = (DomTree)xml.Deserialize(reader);
            }
            return tree;
        }
        public static void WriteTree(string name, DomTree tree)
        {
            XmlSerializer xm = new XmlSerializer(typeof(DomNode));
            XmlSerializer xml = new XmlSerializer(typeof(DomTree));
            TextWriter writer = new StreamWriter(name);
            xml.Serialize(writer, tree);
        }
    }
}
