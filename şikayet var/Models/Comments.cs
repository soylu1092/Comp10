using System;

namespace şikayet_var.Models;

public class Comments
{
public int Id { get; set; }
public int ComplaintId { get; set; }
public string Content { get; set; }
public DateTime CreatedAt { get; set; }

}
