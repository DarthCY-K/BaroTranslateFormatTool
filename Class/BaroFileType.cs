namespace BaroTranslateFormatTool.Class
{
    public static class BaroFileType
    {
        internal enum BaroType
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

        private static readonly string[] CharacterNameArray =
        {
            "character."
        };

        private static readonly string[] ItemNameArray =
        {
            "entityname.", "entitydescription."
        };

        private static readonly string[] MissionNameArray =
        {
            "missionname.", "missiondescription.", "missionsuccess.", "missionfailure.", "missionsonarlabel.",
            "missionheader0.", "missionmessage0."
        };

        private static readonly string[] AfflictionNameArray =
        {
            "afflictionname.", "afflictiondescription.", "afflictioncauseofdeath.", "afflictioncauseofdeathself."
        };

        internal static Dictionary<BaroType, string> FileNameDictionary = new()
        {
            {BaroType.Item,"Item"},
            {BaroType.Character,"Character"},
            {BaroType.Affliction,"Afflictions"},
            {BaroType.Mission,"Missions"}
        };

        internal static Dictionary<BaroType, string> ParentNodeDictionary = new()
        {
            {BaroType.Item,"Items"},
            {BaroType.Character,"Character"},
            {BaroType.Affliction,"Afflictions"},
            {BaroType.Mission,"Missions"}
        };

        internal static Dictionary<BaroType, string> BaroChildNodeDictionary = new()
        {
            {BaroType.Item,"identifier"},
            {BaroType.Character,"speciesname"},
            {BaroType.Affliction,"identifier"},
            {BaroType.Mission,"identifier"}
        };

        internal static Dictionary<BaroType, string> BaroDisplayNameDictionary = new()
        {
            {BaroType.Item,"name"},
            {BaroType.Character,"displayname"},
            {BaroType.Affliction,""},
            {BaroType.Mission,""}
        };

        internal static Dictionary<BaroType, string[]> BaroXmlDescDictionary = new()
        {
            {BaroType.Item, ItemNameArray},
            {BaroType.Character, CharacterNameArray},
            {BaroType.Affliction, AfflictionNameArray},
            {BaroType.Mission, MissionNameArray}
        };

        internal static Dictionary<string, string> BaroTranslateNameDictionary = new()
        {
            {"Simplified Chinese","中文(简体)"}
        };

        internal static Dictionary<string, string> BaroAbbreviation = new()
        {
            { "Simplified Chinese", "中文(简体)" }
        };
    }
}
