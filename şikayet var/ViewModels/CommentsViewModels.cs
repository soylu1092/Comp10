using System;
using System.ComponentModel.DataAnnotations;

namespace şikayet_var.ViewModels;

public class CommentsViewModels
{

[Required(ErrorMessage ="İçerik boş bırakılamaz!")]
public string Content { get; set; }


}
