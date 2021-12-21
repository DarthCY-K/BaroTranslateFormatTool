using System.Xml;
using BaroTranslateFormatTool.Tools;
// ReSharper disable InconsistentNaming

namespace BaroTranslateFormatTool.Class
{
    internal class ModInf
    {
        #region 变量声明

        private const string FilelistName = "filelist.xml";

        public string Name;
        private readonly string _fileLoc;
        private readonly List<string>? _itemXmlLoc;
        private readonly List<string>? _characterXmlLoc;
        private readonly List<string>? _afflictionXmlLoc;
        private readonly List<string>? _missionXmlLoc;

        private readonly List<string>? _itemNameList;
        private readonly List<string>? _characterNameList;
        private readonly List<string>? _afflictionNameList;
        private readonly List<string>? _missionNameList;

        private readonly Dictionary<BaroFileType.BaroType, List<string>> TypeToNameList;

        #endregion

        public ModInf(string name, string loc)
        {
            Name = name;
            _fileLoc = loc;
            _itemXmlLoc = new List<string>();
            _characterXmlLoc = new List<string>();
            _afflictionXmlLoc = new List<string>();
            _missionXmlLoc = new List<string>();
            TypeToNameList = new Dictionary<BaroFileType.BaroType, List<string>>
            {
                { BaroFileType.BaroType.Item, _itemNameList },
                { BaroFileType.BaroType.Character, _characterNameList },
                { BaroFileType.BaroType.Affliction, _afflictionNameList },
                { BaroFileType.BaroType.Mission, _missionNameList }
            };

            SearchForXmlLoc();

            _itemNameList = GetBaroID(_itemXmlLoc, BaroFileType.BaroType.Item);
            _characterNameList = GetBaroID(_characterXmlLoc, BaroFileType.BaroType.Character);
            _afflictionNameList = GetBaroID(_afflictionXmlLoc, BaroFileType.BaroType.Affliction);
            _missionNameList = GetBaroID(_missionXmlLoc, BaroFileType.BaroType.Mission);


            RemoveBaroDisplayName(_characterXmlLoc, BaroFileType.BaroType.Character);
            RemoveBaroDisplayName(_itemXmlLoc, BaroFileType.BaroType.Item);

            //Debug
            PrintXmlLoc();
        }

        #region 工具函数

        /// <summary>
        /// 通过filelist.xml获取主要文件路径
        /// </summary>
        private void SearchForXmlLoc()
        {
            List<string> tempList = new List<string>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_fileLoc + "\\" + Name + "\\" + FilelistName);
            XmlNode xn = xmlDoc.SelectSingleNode("contentpackage")!;
            XmlNodeList xn1 = xn.ChildNodes;

            foreach (XmlNode node in xn1)
            {
                XmlElement xe = (XmlElement)node;
                switch (xe.Name)
                {
                    case "Item":
                        _itemXmlLoc!.Add(xe.GetAttribute("file").Remove(0, 5));
                        break;
                    case "Character":
                        _characterXmlLoc!.Add(xe.GetAttribute("file").Remove(0, 5));
                        break;
                    case "Afflictions":
                        _afflictionXmlLoc!.Add(xe.GetAttribute("file").Remove(0, 5));
                        break;
                    case "Missions":
                        _missionXmlLoc!.Add(xe.GetAttribute("file").Remove(0, 5));
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 获得并返回指定类型的ID集合
        /// </summary>
        /// <param name="filePathList">文件路径集合</param>
        /// <param name="type">文件类型</param>
        /// <returns></returns>
        private List<string> GetBaroID(List<string> filePathList, BaroFileType.BaroType type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            List<string> tempList = new List<string>();

            if (type.Equals(BaroFileType.BaroType.Character))
            {
                foreach (var path in filePathList)
                {
                    xmlDoc.Load(_fileLoc + "\\" + path);
                    XmlNode? node = xmlDoc.SelectSingleNode(BaroFileType.FileNameDictionary[type]);
                    if (node != null)
                    {
                        if (node.NodeType == XmlNodeType.Comment) continue;
                        if (node.Name.Equals("Override")) continue;
                        string name = ((XmlElement)node).GetAttribute(BaroFileType.BaroChildNodeDictionary[type]);
                        //防止出现多重identifier
                        if (name.Contains(','))
                            name = name.Substring(0, name.IndexOf(','));
                        tempList.Add(name);
                    }
                }
            }
            else
            {
                foreach (var path in filePathList)
                {
                    xmlDoc.Load(_fileLoc + "\\" + path);
                    XmlNode? node = xmlDoc.SelectSingleNode(BaroFileType.ParentNodeDictionary[type]);
                    if (node != null && node.HasChildNodes)
                    {
                        foreach (XmlNode node1 in node.ChildNodes)
                        {
                            if (node1.NodeType == XmlNodeType.Comment) continue;
                            if (node1.Name.Equals("Override")) continue;
                            string name = ((XmlElement)node1).GetAttribute(BaroFileType.BaroChildNodeDictionary[type]);
                            //防止出现多重identifier
                            if (name.Contains(','))
                                name = name.Substring(0, name.IndexOf(','));
                            tempList.Add(name);
                        }
                    }
                }
            }
            return tempList;
        }

        /// <summary>
        /// 检查并清理指定文件路径的指定类型文件下的xml定义名称，用于去除写死的翻译
        /// </summary>
        /// <param name="filePathList">文件路径集合</param>
        /// <param name="type">文件类型</param>
        /// <returns></returns>
        private void RemoveBaroDisplayName(List<string> filePathList, BaroFileType.BaroType type)
        {
            XmlDocument xmlDoc = new XmlDocument();

            //TODO 这里有不应该存在的冗余，待整理
            if (type.Equals(BaroFileType.BaroType.Item))
            {
                foreach (var path in filePathList)
                {
                    xmlDoc.Load(_fileLoc + "\\" + path);
                    XmlNodeList? nodeList = xmlDoc.SelectNodes(BaroFileType.ParentNodeDictionary[type] + "/" + BaroFileType.FileNameDictionary[type]);
                    
                    if (nodeList != null)
                    {
                        foreach (var node in nodeList)
                        {
                            XmlNode? aNode = ((XmlElement)node).GetAttributeNode(BaroFileType.BaroDisplayNameDictionary[type]);
                            if (aNode == null) continue;
                            if (aNode.InnerText.Equals("")) continue;
                            aNode.InnerText = "";
                            $"{_fileLoc + "\\" + path}的{BaroFileType.BaroDisplayNameDictionary[type]}={aNode.InnerText}被替换为空"
                                .WriteSuccessLine();
                        }
                    }
                    xmlDoc.Save(_fileLoc + "\\" + path);
                }
            }
            else
            {
                foreach (var path in filePathList)
                {
                    xmlDoc.Load(_fileLoc + "\\" + path);
                    XmlNodeList? nodeList = xmlDoc.SelectNodes(BaroFileType.FileNameDictionary[type]);
                    if (nodeList != null)
                    {
                        foreach (var node in nodeList)
                        {
                            XmlNode? aNode = ((XmlElement)node).GetAttributeNode(BaroFileType.BaroDisplayNameDictionary[type]);
                            if (aNode == null) continue;
                            if (aNode.InnerText.Equals("")) continue;
                            $"{_fileLoc + "\\" + path}的{BaroFileType.BaroDisplayNameDictionary[type]}={aNode.InnerText}被替换为空"
                                .WriteSuccessLine();
                            aNode.InnerText = "";
                        }

                    }

                    xmlDoc.Save(_fileLoc + "\\" + path);
                }
            }
        }

        /// <summary>
        /// 修改mod文件名
        /// </summary>
        public void ModifyBaroFileName(string name)
        {
            FileTools.RenameMod(Name, name, _fileLoc);
        }

        ///<summary>
        /// 创建并写入xml翻译文件
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="path">汉化路径</param>
        public void WriteXmlFile(string language, string path)
        {
            //TODO 这里每次写之前要检查有无重复文件，目前遇到重复文件就不会再新建
            XmlDocument? doc = PrintToTranslationFile(language, path);
            doc?.Save(path + "\\" + Name + "-" + BaroFileType.BaroTranslateNameDictionary[language] + ".xml");
        }
        /// <summary>
        /// 创建并写入xml翻译文件
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="name">文件名</param>
        /// <param name="path">汉化路径</param>
        public void WriteXmlFile(string language, string name, string path)
        {
            //TODO 这里每次写之前要检查有无重复文件，目前遇到重复文件就不会再新建
            XmlDocument? doc = PrintToTranslationFile(language, path);
            doc?.Save(path + "\\" + name + ".xml");
        }

        /// <summary>
        /// 根据获得的列表生成xml对象
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="path">汉化路径</param>
        /// <returns>创建好的XmlDocument对象</returns>
        private XmlDocument? PrintToTranslationFile(string language, string path)
        {
            //是否有值，影响xml是否返回null
            bool hasValue = false;

            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            doc.AppendChild(dec);
            XmlElement rootElement = doc.CreateElement("infotext");
            XmlAttribute lanAttribute = doc.CreateAttribute("language");
            lanAttribute.Value = language;
            rootElement.Attributes.Append(lanAttribute);
            XmlAttribute whitespaceAttribute = doc.CreateAttribute("nowhitespace");
            whitespaceAttribute.Value = "true";
            rootElement.Attributes.Append(whitespaceAttribute);
            XmlAttribute translatedAttribute = doc.CreateAttribute("translatedname");
            translatedAttribute.Value = BaroFileType.BaroTranslateNameDictionary[language];
            rootElement.Attributes.Append(translatedAttribute);
            rootElement.Attributes.Append(translatedAttribute);

            hasValue = ProduceAllXmlElement(doc, rootElement, BaroFileType.BaroType.Item);
            doc.AppendChild(rootElement);

            return hasValue ? doc : null;
        }

        private bool ProduceAllXmlElement(XmlDocument doc, XmlElement root, BaroFileType.BaroType type)
        {
            bool hasValue;

            //Item名称集合不为空
            hasValue = ProduceClassifiedXmlElement(doc, root, BaroFileType.BaroType.Item, _itemNameList);
            //Character名称集合不为空
            hasValue = ProduceClassifiedXmlElement(doc, root, BaroFileType.BaroType.Item, _characterNameList);
            //Affliction名称集合不为空
            hasValue = ProduceClassifiedXmlElement(doc, root, BaroFileType.BaroType.Item, _afflictionNameList);
            //Mission名称集合不为空
            hasValue = ProduceClassifiedXmlElement(doc, root, BaroFileType.BaroType.Item, _missionNameList);

            return hasValue;
        }

        /// <summary>
        /// 新建标签名为name的XmlElement
        /// </summary>
        /// <param name="doc">所属的XmlDocument</param>
        /// <param name="name">XmlElement</param>
        /// <returns></returns>
        private static XmlElement ProduceXmlElement(XmlDocument doc, string name)
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
        /// <returns></returns>
        private static bool ProduceClassifiedXmlElement(XmlDocument doc, XmlElement root, BaroFileType.BaroType type, List<string> list)
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

        #endregion

        #region Debug用

        /// <summary>
        /// 打印xml，测试用
        /// </summary>
        public void PrintXmlLoc()
        {
            Console.WriteLine($"++++++++{Name}++++++++");

            if (_itemXmlLoc != null && _itemXmlLoc.Count != 0)
            {
                Console.WriteLine("--Item--");
                PrintForeach(_itemXmlLoc);
                Console.WriteLine("-ID-");
                PrintForeach(_itemNameList);
            }

            if (_characterXmlLoc != null && _characterXmlLoc.Count != 0)
            {
                Console.WriteLine("\n--Character--");
                PrintForeach(_characterXmlLoc);
                Console.WriteLine("-ID-");

                PrintForeach(_characterNameList);
            }

            if (_afflictionXmlLoc != null && _afflictionXmlLoc.Count != 0)
            {
                Console.WriteLine("\n--Affliction--");
                PrintForeach(_afflictionXmlLoc);
                Console.WriteLine("-ID-");
                PrintForeach(_afflictionNameList);
            }


            if (_missionXmlLoc != null && _missionXmlLoc.Count != 0)
            {
                Console.WriteLine("\n--Mission--");
                PrintForeach(_missionXmlLoc);
                Console.WriteLine("-ID-");
                PrintForeach(_missionNameList);
            }

            Console.Write("\n\n");
        }

        private void PrintForeach(List<string> list)
        {
            if (list == null || list.Count == 0) Console.WriteLine("null");
            else
                foreach (var item in list)
                    Console.WriteLine(item);
        }

        #endregion
    }
}

