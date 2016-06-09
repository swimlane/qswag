#region Using

using System.Collections.Generic;
using System.IO;
using System.Xml;
using QSwagGenerator.Models;

#endregion

namespace QSwagGenerator.Generators
{
    internal class XmlDocGenerator
    {
        #region Access: Internal

        internal static XmlDocs GetXmlDocs(string xmlDocPath)
        {
            var collect = new XmlDocs();
            if (string.IsNullOrEmpty(xmlDocPath) || !File.Exists(xmlDocPath))
                return collect;
            using (var fileStream = File.Open(xmlDocPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = XmlReader.Create(fileStream))
                {
                    while (!reader.EOF)
                    {
                        reader.Read();
                        if (reader.Name != "member" || !reader.IsStartElement()) continue;
                        var doc = GetDoc(reader);
                        collect.Add(doc.Name, doc);
                    }
                }
            }

            return collect;
        }

        #endregion

        #region Access: Private
        private static string CleanString(string value)
        {
            return value.Trim(' ', '\r', '\n', '\t');
        }
        private static XmlDoc GetDoc(XmlReader reader)
        {
            var doc = new XmlDoc {Name = reader.GetAttribute("name")};
            do
            {
                reader.Read();
                if (reader.Name == "summary")
                    doc.Summary = CleanString(reader.ReadInnerXml());
                else if (reader.Name == "returns")
                    doc.Returns = CleanString(reader.ReadInnerXml());
                else if (reader.Name == "param")
                    doc.Parameters.Add(reader.GetAttribute("name"), CleanString(reader.ReadInnerXml()));
            } while (reader.Name != "member");
            return doc;
        }

        #endregion
    }
}