using System.Collections.Generic;

namespace dataentry.Utility
{
    public class Configs
    {
        public PreviewSettings PreviewSettings { get; set; }
        public IdentitySettings IdentitySettings { get; set; }
    }

    public class IdentitySettings
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Key { get; set; }
    }

    public class PreviewSettings
    {
        public string Path { get; set; }
        public List<Proxy> Proxy { get; set; }
        public List<Site> Sites { get; set; } 
    }

    public class Proxy
    {
        public string PathMatch { get; set; }
        public string UpStreamHost { get; set; }
    }

    public class Site
    {
        public string HomeSiteId { get; set; }
        public string ControllerPath { get; set; }
        public List<SPAConfigPaths> SPAConfigPaths { get; set; }
    }

    public class SPAConfigPaths
    {
        public string UsageType { get; set; }
        public string Path { get; set; }
    }
}
