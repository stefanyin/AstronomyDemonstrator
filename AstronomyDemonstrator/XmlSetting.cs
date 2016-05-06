using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace AstronomyDemonstrator
{
    class XmlSetting
    {
        XmlDocument xmlFile;
        XmlNodeList xnl;
        XmlNode root;
        XmlElement xmlEle;
        public XmlSetting()
        {
            xmlFile = new XmlDocument();
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "PlayerSetting.xml"))
            {

            } 
            else
            {
                XmlDeclaration xmldec;
                xmldec = xmlFile.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlFile.AppendChild(xmldec);
                xmlEle = xmlFile.CreateElement("Paths");
                xmlFile.AppendChild(xmlEle);
                root = xmlFile.DocumentElement;
                XmlElement xe1 = xmlFile.CreateElement("MoviesPath");
                root.AppendChild(xe1);
                xmlFile.Save(System.AppDomain.CurrentDomain.BaseDirectory + "PlayerSetting.xml");
            }
            xmlFile.Load(System.AppDomain.CurrentDomain.BaseDirectory + "PlayerSetting.xml");
            xnl = xmlFile.DocumentElement.ChildNodes;
        }

        public string GetValueByName(string nodeName)
        {
            string tempvalue = string.Empty;
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.Name == nodeName)
                {
                    tempvalue = xe.InnerText;
                }
            }
            return tempvalue;
        }
        public void SetNodeValue(string nodeName, string nodeValue)
        {
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.Name == nodeName)
                {
                    xe.InnerText = nodeValue;
                    xmlFile.Save(System.AppDomain.CurrentDomain.BaseDirectory + "PlayerSetting.xml");
                    return;
                }
            }
        }
    }
}
