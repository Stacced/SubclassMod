using SubclassMod.API.Classes;
using UnityEngine;

namespace SubclassMod.Components
{
    public class SubclassedPlayer : MonoBehaviour
    {
        public ReferenceHub Hub { get; set; }
        public RoleInfo ActiveRole { get; set; }
        public SubclassInfo ActiveSubclass { get; set; }
    }
}