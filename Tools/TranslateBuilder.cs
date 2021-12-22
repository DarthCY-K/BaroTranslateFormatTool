using System.Xml;
using BaroTranslateFormatTool.Class;

namespace BaroTranslateFormatTool.Tools
{
    internal static class TranslateBuilder
    {
        /// <summary>
        /// 新建标签名为name的XmlElement
        /// </summary>
        /// <param name="doc">所属的XmlDocument</param>
        /// <param name="name">XmlElement</param>
        /// <returns></returns>
        public static XmlElement ProduceXmlElement(XmlDocument doc, string name)
        {
            XmlElement element = doc.CreateElement(name);
            element.InnerXml = " ";

            return element;
        }

        /// <summary>
        /// 根据不同的分类提供创建汉化xml方法
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="root"></param>
        /// <param name="type">翻译类型</param>
        /// <param name="list">列表</param>
        /// <returns></returns>
        public static bool ProduceClassifiedXmlElement(XmlDocument doc, XmlElement root, BaroFileType.BaroType type, List<string> list)
        {
            if (list.Count <= 0) return false;

            root.AppendChild(doc.CreateComment(BaroFileType.FileNameDictionary[type]));

            foreach (var name in list)
            {
                root.AppendChild(doc.CreateComment(name));

                foreach (var desc in BaroFileType.BaroXmlDescDictionary[type])
                {
                    root.AppendChild(ProduceXmlElement(doc, desc + name));
                }
            }
            doc.AppendChild(root);
            return true;
        }
    }
}
