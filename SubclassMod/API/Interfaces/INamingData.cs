using SubclassMod.API.Enums;

namespace SubclassMod.API.Interfaces
{
    public interface INamingData
    {
      string NamePrefix { get; set; }  
      string NamePostfix { get; set; }
      
      NamingMethod NamingMethod { get; set; }
    }
}