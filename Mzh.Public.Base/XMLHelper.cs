using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mzh.Public.Base
{
    public class XMLHelper
    {
        public static SortedDictionary<string, object> FromXml(string xml)
        {
            SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                m_values[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
            }
            return m_values;
        }

    }
}
