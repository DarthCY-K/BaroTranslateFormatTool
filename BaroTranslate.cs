using BaroTranslateFormatTool.Class;
using BaroTranslateFormatTool.Tools;

namespace BaroTranslateFormatTool
{
    internal class BaroTranslate
    {

        private const string RootPath = "D:\\DevelopSpace\\BaroTranslateFormatTool\\";
        private const string SourceFilePath = RootPath + "Mods\\";
        private const string ModifiedFilePath = RootPath + "ModsModified\\";
        private const string OldTranslateFilePath = RootPath + "OldTranslateTexts\\";
        private const string NewTranslateFilePath = RootPath + "NewTranslateTexts\\";

        public static void Main(string[] args)
        {
            List<ModInf> modObjectList = new List<ModInf>();

            try
            {
                FileTools.DeleteFolder(ModifiedFilePath);
                FileTools.CopyFolder(SourceFilePath, ModifiedFilePath);
            }catch (IOException e) { e.ToString().WriteErrorLine(); }

            foreach (DirectoryInfo info in new DirectoryInfo(ModifiedFilePath).GetDirectories()) 
                modObjectList.Add(new ModInf(info.Name, ModifiedFilePath));

            //foreach (var mod in modObjectList)
            //{
            //    mod.WriteXmlFile("Simplified Chinese", NewTranslateFilePath);
            //    mod.ModifyBaroFileName(mod.Name + " CN");
            //}
        }
    }
}