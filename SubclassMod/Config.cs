using System.Collections.Generic;
using System.ComponentModel;

using Exiled.API.Interfaces;

using PlayerRoles;

using SubclassMod.API.Classes;

namespace SubclassMod
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        [Description("Replace roleplay names for class d to number identificators")]
        public bool ClassDNumbers { get; set; } = false;

        [Description("Chance to get subclass instead of overwritten role")]
        public float SubclassChance { get; set; } = 70f;

        [Description("First names for human classes")]
        public List<string> HumanFirstNames { get; set; } = new List<string>();
        
        [Description("Special signs for human classes")]
        public List<string> HumanSpecialSigns { get; set; } = new List<string>();

        [Description("Additional info for roles")]
        public Dictionary<RoleTypeId, RoleInfo> CustomRolesInfo { get; set; } = new Dictionary<RoleTypeId, RoleInfo>
        {
            [RoleTypeId.FacilityGuard] = new RoleInfo(),
            [RoleTypeId.Scientist] = new RoleInfo()
        };
        
        [Description("List of subclasses for every base role. EVERY new custom subclass start from - (ID'S SHOULD BE UNIQUE)")]
        public List<SubclassInfo> CustomSubclasses { get; set; } = new List<SubclassInfo>();

        [Description("List of player's personal custom characters that can be selected by console command")]
        public List<CharacterInfo> CustomCharacters { get; set; } = new List<CharacterInfo>();
    }
}