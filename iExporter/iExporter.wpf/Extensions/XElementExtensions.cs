using System;
using System.Xml;
using System.Xml.Linq;

namespace iExporter.wpf.Extensions
{
    public static class XElementExtensions
    {
        public static int ToInt(this XElement xe, int emptyValue)
        {
            return xe == null ? emptyValue : int.Parse(xe.Value);
        }

        public static Int64 ToInt64(this XElement xe, Int64 emptyValue)
        {
            return xe == null ? emptyValue : Int64.Parse(xe.Value);
        }

        public static string ToString(this XElement xe, string emptyValue)
        {
            return xe?.Value ?? emptyValue;
        }

        public static XElement GetXElement(this XmlNode node)
        {
            XDocument xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);

            return xDoc.Root;
        }

        public static XmlNode GetXmlNode(this XElement element)
        {
            var xnNode = (XmlNode)GetXmlDocument(element);
            return xnNode;
        }

        public static XmlDocument GetXmlDocument(this XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        public static XmlElement GetXmlElement(this XElement element)
        {
            var xeNode = GetXmlDocument(element).DocumentElement;
            return xeNode;
        }
    }
}
