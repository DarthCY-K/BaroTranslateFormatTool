<<<<<<< HEAD
﻿using BaroTranslateFormatTool.Tools;
=======
﻿using System;
using System.Collections.Generic;
>>>>>>> 6b2c672421d08a5dfe64228cf8d959f55e95fcc3
using System.Xml;

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

        #endregion

        public ModInf(string name, string loc)
        {
            Name = name;
            _fileLoc = loc;
            _itemXmlLoc = new List<string>();
            _characterXmlLoc = new List<string>();
            _afflictionXmlLoc = new List<string>();
            _missionXmlLoc = new List<string>();

            SearchForXmlLoc();

            _itemNameList = GetBaroID(_itemXmlLoc, BaroFileType.BaroFileTypeEnum.Item);
            _characterNameList = GetBaroID(_characterXmlLoc, BaroFileType.BaroFileTypeEnum.Character);
            _afflictionNameList = GetBaroID(_afflictionXmlLoc, BaroFileType.BaroFileTypeEnum.Affliction);
            _missionNameList = GetBaroID(_missionXmlLoc, BaroFileType.BaroFileTypeEnum.Mission);

            RemoveBaroDisplayName(_characterXmlLoc, BaroFileType.BaroFileTypeEnum.Character);
            RemoveBaroDisplayName(_itemXmlLoc, BaroFileType.BaroFileTypeEnum.Item);

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
            xmlDoc.Load(_fileLoc + "\\" + Name +"\\"+ FilelistName);
            XmlNode xn = xmlDoc.SelectSingleNode("contentpackage")!;
            XmlNodeList xn1 = xn.ChildNodes;

            foreach (XmlNode node in xn1)
            {
                XmlElement xe = (XmlElement)node;
                switch (xe.Name)
                {
                    case "Item":
                        _itemXmlLoc!.Add(xe.GetAttribute("file").Remove(0,5));
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

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 获得并返回指定类型的ID集合
        /// </summary>
        /// <param name="filePathList">文件路径集合</param>
        /// <param name="type">文件类型</param>
        /// <returns></returns>
        private List<string> GetBaroID(List<string> filePathList, BaroFileType.BaroFileTypeEnum type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            List<string> tempList = new List<string>();

            if (type.Equals(BaroFileType.BaroFileTypeEnum.Character))
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
<<<<<<< HEAD
=======
                        //tempList.Add(((XmlElement)node).GetAttribute(BaroFileType.BaroChildNodeDictionary[type]));
>>>>>>> 6b2c672421d08a5dfe64228cf8d959f55e95fcc3
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
<<<<<<< HEAD
=======
                            //tempList.Add(((XmlElement)node1).GetAttribute(BaroFileType.BaroChildNodeDictionary[type]));
>>>>>>> 6b2c672421d08a5dfe64228cf8d959f55e95fcc3
                        }
                    }
                }
            }

            return tempList;
        }

        //TODO Affliction和Mission写死的标签对尚不清楚是什么，等测试完了再写
        /// <summary>
        /// 检查并清理指定文件路径的指定类型文件下的xml定义名称，用于去除写死的翻译
        /// </summary>
        /// <param name="filePathList">文件路径集合</param>
        /// <param name="type">文件类型</param>
        /// <returns></returns>
        private void RemoveBaroDisplayName(List<string> filePathList, BaroFileType.BaroFileTypeEnum type)
        {
            XmlDocument xmlDoc = new XmlDocument();

            foreach (var path in filePathList)
            {
                xmlDoc.Load(_fileLoc + "\\" + path);
                XmlNode? node = xmlDoc.SelectSingleNode(BaroFileType.FileNameDictionary[type]);
                if (node != null)
                {
                    XmlNode? aNode = ((XmlElement)node).GetAttributeNode(BaroFileType.BaroDisplayNameDictionary[type]);
                    if (aNode != null)
                    {
                        if (!aNode.InnerText.Equals(""))
                        {
                            $"{_fileLoc + "\\" + path}的{BaroFileType.BaroDisplayNameDictionary[type]}={aNode.InnerText}被替换为空".WriteSuccessLine();
                            aNode.InnerText = "";
                        }
                    }
                }
                xmlDoc.Save(_fileLoc + "\\" + path);
            }
        }

        /// <summary>
        /// 修改mod文件名
        /// </summary>
        public void ModifyBaroFileName(string name)
        {
            FileTools.RenameMod(Name, name, _fileLoc);
        }

        /// <summary>
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
<<<<<<< HEAD
        /// 根据获得的列表生成xml对象
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="path">汉化路径</param>
        /// <returns>创建好的XmlDocument对象</returns>
        private XmlDocument? PrintToTranslationFile(string language, string path)
        {
=======
        /// 创建并写入xml翻译文件
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="path">汉化路径</param>
        public void WriteXmlFile(string language, string path)
        {
            //TODO 这里每次写之前要检查有无重复文件，目前遇到重复文件就不会再新建
            XmlDocument? doc = PrintToTranslationFile(language, path);
            doc?.Save(path + "\\" + _name + "-" + BaroFileType.BaroTranslateNameDictionary[language] + ".xml");
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
>>>>>>> 6b2c672421d08a5dfe64228cf8d959f55e95fcc3
            //是否有值，影响xml是否返回null
            bool hasValue = false;

            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            doc.AppendChild(dec);
            
<<<<<<< HEAD
            XmlElement rootElement = doc.CreateElement("infotext");

            XmlAttribute lanAttribute = doc.CreateAttribute("language");lanAttribute.Value = language;
            rootElement.Attributes.Append(lanAttribute);

            XmlAttribute whitespaceAttribute = doc.CreateAttribute("nowhitespace");whitespaceAttribute.Value = "true";
            rootElement.Attributes.Append(whitespaceAttribute);

            XmlAttribute translatedAttribute = doc.CreateAttribute("translatedname");translatedAttribute.Value = BaroFileType.BaroTranslateNameDictionary[language];
            rootElement.Attributes.Append(translatedAttribute);

            //TODO 下面部分需要针对特殊情况和内容进行整合，现在冗余度太高

=======
            //infotext
            XmlElement rootElement = doc.CreateElement("infotext");
            //language="" nowhitespace="true" translatedname=""
            XmlAttribute lanAttribute = doc.CreateAttribute("language");
            lanAttribute.Value = language;
            rootElement.Attributes.Append(lanAttribute);
            XmlAttribute whitespaceAttribute = doc.CreateAttribute("nowhitespace");
            whitespaceAttribute.Value = "true";
            rootElement.Attributes.Append(whitespaceAttribute);
            XmlAttribute translatedAttribute = doc.CreateAttribute("translatedname");
            translatedAttribute.Value = BaroFileType.BaroTranslateNameDictionary[language];
            rootElement.Attributes.Append(translatedAttribute);

>>>>>>> 6b2c672421d08a5dfe64228cf8d959f55e95fcc3
            //Item名称集合不为空
            if (_itemNameList != null && _itemNameList.Count > 0)
            {
                rootElement.AppendChild(doc.CreateComment("[Items]"));
                foreach (var name in _itemNameList)
                {
                    rootElement.AppendChild(doc.CreateComment(name));

                    XmlElement nameElement = doc.CreateElement("entityname." + name);
                    nameElement.InnerXml = " ";
                    rootElement.AppendChild(nameElement);

                    XmlElement descElement = doc.CreateElement("entitydescription." + name);
                    descElement.InnerXml = " ";
                    rootElement.AppendChild(descElement);
                }
                if(hasValue == false) hasValue = true;
            }

            //Character名称集合不为空
            if (_characterNameList != null && _characterNameList.Count > 0)
            {
                rootElement.AppendChild(doc.CreateComment("[Characters]"));
                foreach (var name in _characterNameList)
                {
                    rootElement.AppendChild(doc.CreateComment(name));

                    XmlElement nameElement = doc.CreateElement("character." + name);
                    nameElement.InnerXml = " ";
                    rootElement.AppendChild(nameElement);
                }
                if (hasValue == false) hasValue = true;
            }

            //Affliction名称集合不为空
            if (_afflictionNameList != null && _afflictionNameList.Count > 0)
            {
                rootElement.AppendChild(doc.CreateComment("[Afflictions]"));
                foreach (var name in _afflictionNameList)
                {
                    rootElement.AppendChild(doc.CreateComment(name));

                    XmlElement nameElement = doc.CreateElement("afflictionname." + name);
                    nameElement.InnerXml = " ";
                    rootElement.AppendChild(nameElement);

                    XmlElement descElement = doc.CreateElement("afflictiondescription." + name);
                    descElement.InnerXml = " ";
                    rootElement.AppendChild(descElement);

<<<<<<< HEAD
                    XmlElement causeOfDeathElement = doc.CreateElement("afflictioncauseofdeath." + name);
                    causeOfDeathElement.InnerXml = " ";
                    rootElement.AppendChild(causeOfDeathElement);

                    XmlElement causeOfDeathSelfElement = doc.CreateElement("afflictioncauseofdeathself." + name);
                    causeOfDeathSelfElement.InnerXml = " ";
                    rootElement.AppendChild(causeOfDeathSelfElement);
=======
                    //XmlElement causeOfDeathElement = doc.CreateElement("afflictioncauseofdeath." + name);
                    //causeOfDeathElement.InnerXml = " ";
                    //rootElement.AppendChild(causeOfDeathElement);

                    //XmlElement causeOfDeathSelfElement = doc.CreateElement("afflictioncauseofdeathself." + name);
                    //causeOfDeathSelfElement.InnerXml = " ";
                    //rootElement.AppendChild(causeOfDeathSelfElement);
>>>>>>> 6b2c672421d08a5dfe64228cf8d959f55e95fcc3

                }
                if (hasValue == false) hasValue = true;
            }

            //Mission名称集合不为空
            if (_missionNameList != null && _missionNameList.Count > 0)
            {
                rootElement.AppendChild(doc.CreateComment("[Missions]"));
                foreach (var name in _missionNameList)
                {
                    rootElement.AppendChild(doc.CreateComment(name));

                    XmlElement nameElement = doc.CreateElement("missionname." + name);
                    nameElement.InnerXml = " ";
                    rootElement.AppendChild(nameElement);

                    XmlElement descElement = doc.CreateElement("missiondescription." + name);
                    descElement.InnerXml = " ";
                    rootElement.AppendChild(descElement);

<<<<<<< HEAD
                    XmlElement successElement = doc.CreateElement("missionsuccess." + name);
                    successElement.InnerXml = " ";
                    rootElement.AppendChild(successElement);

                    XmlElement sonarElement = doc.CreateElement("missionsonarlabel." + name);
                    sonarElement.InnerXml = " ";
                    rootElement.AppendChild(sonarElement);

                    XmlElement headerElement = doc.CreateElement("missionheader0." + name);
                    headerElement.InnerXml = " ";
                    rootElement.AppendChild(headerElement);

                    XmlElement messageElement = doc.CreateElement("missionmessage0." + name);
                    messageElement.InnerXml = " ";
                    rootElement.AppendChild(messageElement);
=======
                    //XmlElement successElement = doc.CreateElement("missionsuccess." + name);
                    //successElement.InnerXml = " ";
                    //rootElement.AppendChild(successElement);

                    //XmlElement sonarElement = doc.CreateElement("missionsonarlabel." + name);
                    //sonarElement.InnerXml = " ";
                    //rootElement.AppendChild(sonarElement);

                    //XmlElement headerElement = doc.CreateElement("missionheader0." + name);
                    //headerElement.InnerXml = " ";
                    //rootElement.AppendChild(headerElement);

                    //XmlElement messageElement = doc.CreateElement("missionmessage0." + name);
                    //messageElement.InnerXml = " ";
                    //rootElement.AppendChild(messageElement);
>>>>>>> 6b2c672421d08a5dfe64228cf8d959f55e95fcc3
                }
                if (hasValue == false) hasValue = true;
            }

            doc.AppendChild(rootElement);

            return hasValue ? doc : null;
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
            else foreach (var item in list) Console.WriteLine(item);
        }

        #endregion

    }
}
