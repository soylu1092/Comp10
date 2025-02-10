using System;
using System.Data;
using Dapper;
using şikayet_var.ViewModels;

namespace şikayet_var.Models.Repository;

public class ComplaintsRepository
{
 private readonly IDbConnection _dbConnection;

    public ComplaintsRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<ComplaintsViewModels>> GetAllAsync()
    {
        string query ="SELECT * FROM Complaints";
        return await _dbConnection.QueryAsync<ComplaintsViewModels>(query);
    }

    public async Task CreateAsync(ComplaintsViewModels complaints)
    {
        string query ="INSERT INTO Complaints(Title,Description,CreatedAt,ImagePath) VALUES(@Title,@Description,@CreatedAt,ImagePath)";
        await _dbConnection.ExecuteAsync(query, complaints);
    }


    public async Task UpdateAsync(ComplaintsViewModels complaints)
    {
        string query="UPDATE Complaints SET Title=@Title,Description=@Description,CreatedAt=@CreatedAt,ImagePath=@ImagePath";
        await _dbConnection.ExecuteAsync(query, complaints);
    }

    public async Task<dynamic?> HardDeleteAsync(int id)
    {
        string queryGet="SELECT * FROM Complaints WHERE Id=@Id";
        var complaints = await _dbConnection.QueryFirstOrDefaultAsync(queryGet, new {Id = id});
        string query="DELETE FROM Complaints WHERE Id=@Id";
        await _dbConnection.ExecuteAsync(query, new {Id = id});
        return complaints;
    }
}
