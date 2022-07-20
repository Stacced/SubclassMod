using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;
using SubclassMod.API.Classes;

namespace SubclassMod
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Replace roleplay names for class d to number identificators")]
        public bool ClassDNumbers { get; set; } = false;

        [Description("Chance to get subclass instead of overwritten role")]
        public float SubclassChance { get; set; } = 70f;

        [Description("First names for human classes")]
        public List<string> HumanFirstNames { get; set; } = new List<string>();
        
        [Description("Special signs for human classes")]
        public List<string> HumanSpecialSigns { get; set; } = new List<string>();

        [Description("Additional info for roles")]
        public Dictionary<RoleType, RoleInfo> CustomRolesInfo { get; set; } = new Dictionary<RoleType, RoleInfo>
        {
            [RoleType.FacilityGuard] = new RoleInfo(),
            [RoleType.Scientist] = new RoleInfo()
        };
        
        [Description("List of subclasses for every base role. EVERY new custom subclass start from - (ID'S SHOULD BE UNIQUE)")]
        public List<SubclassInfo> CustomSubclasses { get; set; } = new List<SubclassInfo>();

    }
}