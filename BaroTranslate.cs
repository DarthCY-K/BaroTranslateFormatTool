
using System.Runtime.CompilerServices;
using BaroTranslateFormatTool.Class;
using BaroTranslateFormatTool.Tools;

namespace BaroTranslateFormatTool
{
    class BaroTranslate
    {
        private const string SourceFilePath = "D:\\DevelopSpace\\BaroTranslateFormatTool\\Mods\\";
        private const string ModifiedFilePath = "D:\\DevelopSpace\\BaroTranslateFormatTool\\ModsModified\\";
        private const string OldTranslateFilePath = "D:\\DevelopSpace\\BaroTranslateFormatTool\\OldTranslateTexts\\";
        private const string NewTranslateFilePath = "D:\\DevelopSpace\\BaroTranslateFormatTool\\NewTranslateTexts\\";

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
        }
    }
}

