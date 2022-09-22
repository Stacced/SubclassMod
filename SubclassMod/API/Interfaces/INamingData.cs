using SubclassMod.API.Enums;

namespace SubclassMod.API.Interfaces
{
    public interface INamedRole
    {
      string NamePrefix { get; set; }  
      string NamePostfix { get; set; }
      
      NamingMethod NamingMethod { get; set; }
    }
}