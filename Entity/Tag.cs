using System;

namespace BlogApp.Entity;

public class Tag
{
    public int TagId { get; set; }
    public string? Text { get; set; }
    public string PostId { get; set; }
    public Post Post { get; set; } = null!;


}
