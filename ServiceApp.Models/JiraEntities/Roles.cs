using Newtonsoft.Json;

namespace ServiceApp.Models.JiraEntities
{
    public class Roles
    {
        [JsonProperty("atlassian-addons-project-access")]
        public string AtlassianAddonsProjectAccess { get; set; }
        public string Administrator { get; set; }
        public string Viewer { get; set; }
        public string Member { get; set; }
    }
}