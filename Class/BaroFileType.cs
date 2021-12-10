namespace BaroTranslateFormatTool.Class
{
    public static class BaroFileType
    {
        internal enum BaroFileTypeEnum
        {
            /// <summary>
            /// 物品类型
            /// </summary>
            Item,
            /// <summary>
            /// 生物类型
            /// </summary>
            Character,
            /// <summary>
            /// 效果类型
            /// </summary>
            Affliction,
            /// <summary>
            /// 任务类型
            /// </summary>
            Mission
        }

        internal static Dictionary<BaroFileTypeEnum, string> FileNameDictionary = new Dictionary<BaroFileTypeEnum, string>()
        {
            {BaroFileTypeEnum.Item,"Item"},
            {BaroFileTypeEnum.Character,"Character"},
            {BaroFileTypeEnum.Affliction,"Afflictions"},
            {BaroFileTypeEnum.Mission,"Missions"}
        };

        internal static Dictionary<BaroFileTypeEnum, string> ParentNodeDictionary = new Dictionary<BaroFileTypeEnum, string>()
        {
            {BaroFileTypeEnum.Item,"Items"},
            {BaroFileTypeEnum.Character,"Character"},
            {BaroFileTypeEnum.Affliction,"Afflictions"},
            {BaroFileTypeEnum.Mission,"Missions"}
        };

        internal static Dictionary<BaroFileTypeEnum, string> BaroChildNodeDictionary = new Dictionary<BaroFileTypeEnum, string>()
        {
            {BaroFileTypeEnum.Item,"identifier"},
            {BaroFileTypeEnum.Character,"speciesname"},
            {BaroFileTypeEnum.Affliction,"identifier"},
            {BaroFileTypeEnum.Mission,"identifier"}
        };

        internal static Dictionary<BaroFileTypeEnum, string> BaroDisplayNameDictionary = new Dictionary<BaroFileTypeEnum, string>()
        {
            {BaroFileTypeEnum.Item,"name"},
            {BaroFileTypeEnum.Character,"displayname"},
            {BaroFileTypeEnum.Affliction,""},
            {BaroFileTypeEnum.Mission,""}
        };

        internal static Dictionary<string, string> BaroTranslateNameDictionary = new Dictionary<string, string>()
        {
            {"Simplified Chinese","中文(简体)"}
        };
    }
}
