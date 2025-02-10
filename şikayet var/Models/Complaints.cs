using System;

namespace şikayet_var.Models;

public class Complaints
{
public int Id { get; set; }
public string Title { get; set; }
public string Description { get; set; }
public DateTime CreatedAt { get; set; }
public string ImagePath { get; set; }
    public List<Comments> Comments { get; internal set; }
    public int CommentCount { get; internal set; }
}
