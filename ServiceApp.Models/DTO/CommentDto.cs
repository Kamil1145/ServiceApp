namespace ServiceApp.Models.DTO;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserDto Author { get; set; }
}

