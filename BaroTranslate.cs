using System;
using System.Collections.Generic;
using System.IO;
using BaroTranslateFormatTool.Class;
using BaroTranslateFormatTool.Tools;

namespace BaroTranslateFormatTool
{
    class BaroTranslate
    {
        
        private const string SourceFilePath = "C:\\Users\\DarthCY\\Desktop\\BaroTranslateFormatTool\\Mods\\";
        private const string ModifiedFilePath = "C:\\Users\\DarthCY\\Desktop\\BaroTranslateFormatTool\\ModsModified\\";
        private const string OldTranslateFilePath = "C:\\Users\\DarthCY\\Desktop\\BaroTranslateFormatTool\\OldTranslateTexts\\";
        private const string NewTranslateFilePath = "C:\\Users\\DarthCY\\Desktop\\BaroTranslateFormatTool\\NewTranslateTexts\\";

        public static void Main(string[] args)
        {
            List<ModInf> modObjectList = new List<ModInf>();

            try
            {
                FileTools.DeleteFolder(ModifiedFilePath);
                FileTools.CopyFolder(SourceFilePath, ModifiedFilePath);
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }

            foreach (DirectoryInfo info in new DirectoryInfo(ModifiedFilePath).GetDirectories())
            {
                modObjectList.Add(new ModInf(info.Name, ModifiedFilePath));
            }

            foreach (var mod in modObjectList)
            {
                mod.WriteXmlFile("Simplified Chinese", NewTranslateFilePath);
            }
        }
    }
}

