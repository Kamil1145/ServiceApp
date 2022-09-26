using ServiceApp.Models.DTO;

namespace ServiceApp.Models.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public User Author { get; set; }
    public Ticket Ticket { get; set; }
}
