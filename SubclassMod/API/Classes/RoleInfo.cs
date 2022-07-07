using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SubclassMod.API.Classes
{
    public class RoleInfo
    {
        [Description("Prefix that will be placed before nickname")]
        public string Prefix { get; set; } = "Dr.";
        
        [Description("Postfix that will be placed after nickname")]
        public string Postfix { get; set; } = String.Empty;
        
        [Description("Custom info of overridden role")]
        public string CustomInfo { get; set; } = "Just a sugar doctor";
        
        [Description("Is inventory overridden")]
        public bool InventoryOverridden { get; set; } = false;

        [Description("Overwritten items list")]
        public List<ItemType> InventoryOverwrite { get; set; } = new List<ItemType>();
    }
}