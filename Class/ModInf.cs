using System.Runtime.CompilerServices;
using System.Xml;

namespace BaroTranslateFormatTool.Class
{
    internal class ModInf
    {
        #region 变量声明

        private const string FilelistName = "filelist.xml";

        private string _name;
        private string _fileLoc; 
        private List<string>? _itemXmlLoc;
        private List<string>? _characterXmlLoc;
        private List<string>? _afflictionXmlLoc;
        private List<string>? _missionXmlLoc;

        private List<string>? _itemNameList;
        private List<string>? _characterNameList;
        private List<string>? _afflictionNameList;
        private List<string>? _missionNameList;

        #endregion

        #region 构造函数

        public ModInf(string name, string loc)
        {
            _name = name;
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

            //Debug测试方法
            PrintXmlLoc();
        }

        #endregion

        #region 工具函数

        /// <summary>
        /// 通过filelist.xml获取主要文件路径
        /// </summary>
        private void SearchForXmlLoc()
        {
            List<string> tempList = new List<string>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_fileLoc + "\\" + _name +"\\"+ FilelistName);
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
                        tempList.Add(((XmlElement)node).GetAttribute(BaroFileType.BaroChildNodeDictionary[type]));
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
                            tempList.Add(((XmlElement)node1).GetAttribute(BaroFileType.BaroChildNodeDictionary[type]));
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
                            Console.WriteLine($"{_fileLoc + "\\" + path}的{BaroFileType.BaroDisplayNameDictionary[type]}={aNode.InnerText}被替换为空");
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
            //TODO 更改文件名的时候要注意修改xml里的绝对路径，将旧名字换为新名字
            //TODO 嫌疑文件除了四大基本类型的xml文件外，还有filelist.xml和character类型里的ragdoll系列xml，建议是遍历所有存在的xml文件进行匹配删除
        }

        /// <summary>
        /// 新建汉化文件并写入空白xml翻译格式
        /// </summary>
        /// <param name="path">汉化路径</param>
        public void PrintToTranslationFile(string path)
        {
            //TODO 针对四类文件进行汉化xml格式输出
        }

        #endregion

        #region Debug用

        /// <summary>
        /// 打印xml，测试用
        /// </summary>
        public void PrintXmlLoc()
        {
            Console.WriteLine($"++++++++{_name}++++++++");

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
