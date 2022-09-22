using System;
using System.Collections.Generic;
using System.ComponentModel;
using SubclassMod.API.Enums;
using SubclassMod.API.Interfaces;

namespace SubclassMod.API.Classes
{
    public class RoleInfo : INamedRole
    {
        [Description("Prefix that will be placed before nickname")]
        public string NamePrefix { get; set; } = "Dr.";
        
        [Description("Postfix that will be placed after nickname")]
        public string NamePostfix { get; set; } = String.Empty;
        
        [Description("Custom info of overridden role")]
        public string CustomInfo { get; set; } = "Just a sugar doctor";

        [Description("Is inventory overridden")]
        public bool InventoryOverridden { get; set; } = false;

        [Description("Naming method for this role")]
        public NamingMethod NamingMethod { get; set; } = NamingMethod.Nickname;

        [Description("Overwritten items list")]
        public List<ItemType> InventoryOverwrite { get; set; } = new List<ItemType>();
    }
}