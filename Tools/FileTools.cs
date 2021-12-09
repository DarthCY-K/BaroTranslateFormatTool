using System.IO;
using System.Reflection.PortableExecutable;
using System.Xml;

namespace BaroTranslateFormatTool.Tools
{
    internal static class FileTools
    {
        /// <summary>
        /// 递归复制文件夹
        /// </summary>
        /// <param name="sourcePath">旧路径</param>
        /// <param name="targetPath">新路径</param>
        public static void CopyFolder(string sourcePath, string targetPath)
        {
            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

            string[] files = Directory.GetFiles(sourcePath);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(targetPath, name);
                File.Copy(file, dest);
            }

            string[] folders = Directory.GetDirectories(sourcePath);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(targetPath, name);
                CopyFolder(folder, dest);
            }
        }

        /// <summary>
        /// 删除文件夹以及其所有子文件
        /// </summary>
        /// <param name="path">路径</param>
        public static void DeleteFolder(string path)
        {
            Directory.Delete(path, true);
        }

    }
}
