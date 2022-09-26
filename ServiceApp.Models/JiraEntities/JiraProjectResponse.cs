namespace ServiceApp.Models.JiraEntities;

public class JiraProjectResponse
{
    public string expand { get; set; }
    public string self { get; set; }
    public string id { get; set; }
    public string key { get; set; }
    public string description { get; set; }
    public Lead lead { get; set; }
    public List<object> components { get; set; }
    public List<IssueType> issueTypes { get; set; }
    public string assigneeType { get; set; }
    public List<object> versions { get; set; }
    public string name { get; set; }
    public Roles roles { get; set; }
    public AvatarUrls avatarUrls { get; set; }
    public string projectTypeKey { get; set; }
    public bool simplified { get; set; }
    public string style { get; set; }
    public bool isPrivate { get; set; }
    public Properties properties { get; set; }
    public string entityId { get; set; }
    public string uuid { get; set; }
}