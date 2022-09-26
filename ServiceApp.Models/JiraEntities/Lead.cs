namespace ServiceApp.Models.JiraEntities;

public class Lead
{
    public string self { get; set; }
    public string accountId { get; set; }
    public AvatarUrls avatarUrls { get; set; }
    public string displayName { get; set; }
    public bool active { get; set; }
}