using System;

namespace BlogApp.Entity;

public class Comment
{
    public int CommentId { get; set; }
    public string? Commenttext { get; set; }
    public DateTime PublishedOn { get; set; }
    public string UserId { get; set; }
    public User user { get; set; } = null!;
    public string PostId { get; set; }
    public Post Post { get; set; } = null!;


}
