<<<<<<< HEAD
﻿using BaroTranslateFormatTool.Class;
=======
﻿using System;
using System.Collections.Generic;
using System.IO;
using BaroTranslateFormatTool.Class;
>>>>>>> 6b2c672421d08a5dfe64228cf8d959f55e95fcc3
using BaroTranslateFormatTool.Tools;

namespace BaroTranslateFormatTool
{
    class BaroTranslate
    {
<<<<<<< HEAD
        private const string RootPath = "D:\\DevelopSpace\\BaroTranslateFormatTool\\";
        private const string SourceFilePath = RootPath + "Mods\\";
        private const string ModifiedFilePath = RootPath + "ModsModified\\";
        private const string OldTranslateFilePath = RootPath + "OldTranslateTexts\\";
        private const string NewTranslateFilePath = RootPath + "NewTranslateTexts\\";
=======
        
        private const string SourceFilePath = "C:\\Users\\DarthCY\\Desktop\\BaroTranslateFormatTool\\Mods\\";
        private const string ModifiedFilePath = "C:\\Users\\DarthCY\\Desktop\\BaroTranslateFormatTool\\ModsModified\\";
        private const string OldTranslateFilePath = "C:\\Users\\DarthCY\\Desktop\\BaroTranslateFormatTool\\OldTranslateTexts\\";
        private const string NewTranslateFilePath = "C:\\Users\\DarthCY\\Desktop\\BaroTranslateFormatTool\\NewTranslateTexts\\";
>>>>>>> 6b2c672421d08a5dfe64228cf8d959f55e95fcc3

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
<<<<<<< HEAD
                mod.ModifyBaroFileName(mod.Name + " CN");
=======
>>>>>>> 6b2c672421d08a5dfe64228cf8d959f55e95fcc3
            }
        }
    }
}

