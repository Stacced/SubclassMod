using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SubclassMod.API.Classes
{
    public class RoleInfo
    {
        [Description("Prefix that will be placed before nickname")]
        public string NamePrefix { get; set; } = "Dr.";
        
        [Description("Postfix that will be placed after nickname")]
        public string NamePostfix { get; set; } = String.Empty;
        
        [Description("Custom info of overridden role")]
        public string CustomInfo { get; set; } = "Just a sugar doctor";

        [Description("Replace default player name to roleplay name and secondname")]
        public bool RoleplayNameEnabled { get; set; } = true;
        
        [Description("Use second name in player display roleplay nickname")]
        public bool RoleplaySecondNameEnabled { get; set; } = false;

        [Description("Is inventory overridden")]
        public bool InventoryOverridden { get; set; } = false;

        [Description("Overwritten items list")]
        public List<ItemType> InventoryOverwrite { get; set; } = new List<ItemType>();
    }
}