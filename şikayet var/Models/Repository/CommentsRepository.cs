using System;
using System.Data;
using Dapper;
using şikayet_var.ViewModels;

namespace şikayet_var.Models.Repository;

public class CommentsRepository
{
    private readonly IDbConnection _dbConnection;

    public CommentsRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<CommentsViewModels>> GetAllAsync()
    {
        string query = "SELECT * From Comments";
        return await _dbConnection.QueryAsync<CommentsViewModels>(query);
        
    }

    public async Task CreateAsync(CommentsViewModels comments)
    {
        string query="INSERT INTO Comments(Content) VALUES(@Content)";
        await _dbConnection.ExecuteAsync(query, comments);
    }
    public async Task UpdateAsync(CommentsViewModels comments)
    {
        string query = "UPDATE Comments SET Content=@Content";
        await _dbConnection.ExecuteAsync(query, comments);
    }

    public async Task<dynamic?> HardDeleteAsync(int id)
    {
        string queryGet="SELECT * FROM Comments WHERE Id=@Id";
        var comments = await _dbConnection.QueryFirstOrDefaultAsync(queryGet, new {Id = id});
        string query ="DELETE FROM Comments WHERE Id=@Id";
        await _dbConnection.ExecuteAsync(query,new {Id=id});
        return comments;
    }

    
}
