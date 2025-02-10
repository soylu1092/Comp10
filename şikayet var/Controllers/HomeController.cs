using System.Data;
using System.Diagnostics;
using System.Reflection;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using şikayet_var.Models;

namespace şikayet_var.Controllers;

public class HomeController : Controller
{
    private readonly IDbConnection _connection;

    public HomeController(IDbConnection connection)
    {
        _connection = connection;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var complaints = _connection.Query<Complaints>("SELECT * FROM Complaints ORDER BY CreatedAt DESC").ToList();

        foreach (var complaint in complaints)
        {
            complaint.Comments = _connection.Query<Comments>(
                "SELECT * FROM Comments WHERE ComplaintId = @Id ORDER BY CreatedAt DESC",
                new { Id = complaint.Id }).ToList();

            complaint.CommentCount = complaint.Comments.Count;
        }

        ViewBag.TotalComplaints = GetTotalComplaints();

        return View(complaints);
    }


    [HttpPost]
    public IActionResult AddComments(Complaints model, string content)
    {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                var sql = "INSERT INTO Complaints (Title, Description, ImagePath, CreatedAt) VALUES(@Title, @Description, @ImagePath, @CreatedAt)";
                _connection.Execute(sql, model);
            }
            else
            {
                ViewBag.ErrorMsg = "Şikayet eklenemedi. Daha sonra tekrar deneyin.";
            }
            ViewBag.Model=model;
        ViewData["SuccessMsg"] = "Yeni şikayet eklendi.";
        return View();
    }
    public IActionResult DeleteComplaints(int id)
    {
        _connection.Execute("DELETE FROM Comments WHERE ComplaintId = @id", new { id });
        _connection.Execute("DELETE FROM Complaints WHERE Id = @id", new { id });
        return RedirectToAction("Index", "Home");
        // ilk önce yorumları sonra gönderiyi siliyoruz sonra ana sayfaya geri dönüyor
    }

    public IActionResult DeleteComments(int id)
    {
        _connection.Execute("DELETE FROM Comments WHERE Id = @id", new { id });
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult AddComplaint(int complaintId, string content)
    {
        if (ModelState.IsValid)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                ViewBag.ErrorMsg = "Yorum içeriği boş olamaz.";
                return View();
            }
            var comment = new Comments
            {
                ComplaintId = complaintId,
                Content = content,
                CreatedAt = DateTime.Now
            };

            var sql = "INSERT INTO Comments (ComplaintId, Content, CreatedAt) VALUES (@ComplaintId, @Content, @CreatedAt)";
            _connection.Execute(sql, comment);
            var comments = _connection.Query<Comments>("SELECT * FROM Comments WHERE ComplaintId = @ComplaintId ORDER BY CreatedAt DESC", new { ComplaintId = complaintId }).ToList();

            TempData["SuccessMsg"] = "Yeni şikayet eklendi.";

        }
        return RedirectToAction("Index"); 
    }
    [HttpGet]
    public IActionResult EditComplaint(int id)
    {
        var complaint = _connection.QuerySingleOrDefault<Complaints>("SELECT * FROM Complaints WHERE Id = @id", new { id });

        if (complaint == null)
        {
            return NotFound();
        }

        return View("EditComplaint", complaint);
    }

    [HttpPost]
    public IActionResult EditComplaint(Complaints model)
    {

        var sql = @"UPDATE Complaints 
                    SET Title=@Title, Description=@Description, ImagePath=@ImagePath
                    WHERE Id = @Id";

        _connection.Execute(sql, model);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditComment(int id)
    {
        var comment = _connection.QuerySingleOrDefault<Comments>("SELECT * FROM Comments WHERE Id = @id", new { id });

        if (comment == null)
        {
            return NotFound();
        }

        return View("EditComment", comment);
    }

    [HttpPost]
    public IActionResult EditComment(Comments model)
    {

        var sql = @"UPDATE Comments 
                    SET Content=@Content, 
                    WHERE Id = @Id";

        _connection.Execute(sql, model);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult ComplaintDetails(int id)
    {
        var complaint = _connection.QuerySingleOrDefault<Complaints>(
            "SELECT * FROM Complaints WHERE Id = @id", new { id });

        if (complaint == null)
        {
            return NotFound();
        }

        var comments = _connection.Query<CommentViewModel>(
            "SELECT Content, CreatedAt FROM Comments WHERE ComplaintId = @Id ORDER BY CreatedAt DESC",
            new { Id = complaint.Id }).ToList();

        var images = _connection.Query<string>(
            "SELECT ImagePath FROM ComplaintImages WHERE ComplaintId = @Id",
            new { Id = complaint.Id }).ToList();

        var viewModel = new ComplaintDetailsViewModel
        {
            Id = complaint.Id,
            Title = complaint.Title,
            Description = complaint.Description,
            CreatedAt = complaint.CreatedAt,
            Comments = comments,
            ImagePath = images
        };

        return View(viewModel);
    }

    [HttpPost] // Formdan gelen veriyi işler
    public IActionResult Search(string query)
    {
        // Kullanıcı boş bir arama yaparsa ana sayfaya yönlendir
        if (string.IsNullOrWhiteSpace(query))
        {
            return RedirectToAction("Index"); //bu method chat gpt den copy paste dir !
        }

        // Veritabanında başlığa göre arama yapar ve ilgili şikayetleri getirir
        var results = _connection.Query<Complaints>(
            "SELECT * FROM Complaints WHERE Title LIKE @search ORDER BY CreatedAt DESC",
            new { search = "%" + query + "%" }).ToList();

        // Eğer eşleşen bir şikayet yoksa boş liste döner
        return View("SearchResults", results);
    }

    public int GetTotalComplaints()
    {
        return _connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Complaints");
    }

}