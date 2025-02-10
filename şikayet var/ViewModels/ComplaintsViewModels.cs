using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using static System.Net.WebRequestMethods;

namespace şikayet_var.ViewModels;

public class ComplaintsViewModels
{
    
    [Required(ErrorMessage ="Başlık kısmı boş bırakılamaz!")]
public string? Title { get; set; }
[Required(ErrorMessage ="Açıklama Boş bırakılamaz!")]
public string? Description { get; set; }
public DateTime CreatedAt { get; set; }
public string? ImagePath { get; set; }
}
