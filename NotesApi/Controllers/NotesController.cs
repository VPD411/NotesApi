using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using NotesApi.Models;

namespace NotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]

// http://localhost:5118/api/notes
public class NotesController : ControllerBase
{
    private readonly string _connectionString;

    public NotesController(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    private SqliteConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    [HttpGet]
    // GET: http://localhost:5118/api/notes
    public async Task<ActionResult<List<Note>>> GetAll()
    {
        using var db = GetConnection();
        var notes = await db.QueryAsync<Note>("SELECT * FROM Notes ORDER BY CreatedAt DESC");
        return Ok(notes);
    }

    [HttpGet("{id::int}")]
    // GET: http://localhost:5118/api/notes/{id}
    public async Task<ActionResult<Note>> Get(int id)
    {
        using var db = GetConnection();
        var note = await db.QuerySingleOrDefaultAsync<Note>("SELECT * FROM Notes WHERE Id = @Id", new { Id = id });
        return note == null ? NotFound() : Ok(note);
    }

    [HttpPost]
    // POST: http://localhost:5118/api/notes/
    public async Task<ActionResult> Create(Note note)
    {
        using var db = GetConnection();
        var sql = "INSERT INTO Notes(Title, Content, CreatedAt) VALUES (@Title, @Content, @CreatedAt); SELECT last_insert_rowid();";
        note.CreatedAt = DateTime.Now;
        note.Id = await db.QuerySingleAsync<int>(sql, note);
        return CreatedAtAction(nameof(Get), new { id = note.Id }, note);
    }

    [HttpPut("{id::int}")]
    // PUT: http://localhost:5118/api/notes/{id}
    public async Task<ActionResult> Uodate(int id, [FromBody] Note newNote)
    {
        using var db = GetConnection();
        var sql = "UPDATE Notes SET Title = @Title, Content = @Content WHERE Id = @Id";
        var rows = await db.ExecuteAsync(sql, new { newNote.Title, newNote.Content, Id = id });
        return rows == 0 ? NotFound() : NoContent();
    }

    [HttpDelete("{id::int}")]
    // DELETE: http://localhost:5118/api/notes/{id}
    public async Task<ActionResult> Delete(int id)
    {
        using var db = GetConnection();
        var rows = await db.ExecuteAsync("DELETE FROM Notes WHERE Id = @Id", new { Id = id });
        return rows == 0 ? NotFound() : NoContent();
    }
}