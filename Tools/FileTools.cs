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

        /// <summary>
        /// 修改mod文件名，同时删除部分内容
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <param name="path"></param>
        public static void RenameMod(string oldName, string newName, string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path + "\\" + oldName);
            foreach (var file in dirInfo.GetFiles("*.xml", SearchOption.AllDirectories))
            {
                var fileContent = File.ReadAllText(file.FullName);
                if (!fileContent.Contains(oldName))
                {
                    $"路径：{file.FullName}中未发现{oldName}，跳过".WriteWarningLine();
                    continue;
                }
                $"路径：{file.FullName}中的{oldName}被替换成了{newName}".WriteSuccessLine();
                fileContent = fileContent.Replace(oldName, newName);
                File.WriteAllText(file.FullName, fileContent);
            }

            Directory.Move(path + "\\" + oldName, path + "\\" + newName);
        }
    }
}
