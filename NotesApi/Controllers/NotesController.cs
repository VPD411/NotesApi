using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using NotesApi.Data;
using NotesApi.Models;

namespace NotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]

// http://localhost:5118/api/notes
public class NotesController : ControllerBase
{
    private readonly INoteRepository _repository;

    public NotesController(INoteRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    // GET: http://localhost:5118/api/notes
    public async Task<ActionResult<List<Note>>> GetAll()
    {
        var notes = await _repository.GetAllAsync();
        return Ok(notes);
    }

    [HttpGet("{id::int}")]
    // GET: http://localhost:5118/api/notes/{id}
    public async Task<ActionResult<Note>> Get(int id)
    {
        var note = await _repository.GetByIdAsync(id);
        return note == null ? NotFound() : Ok(note);
    }

    [HttpPost]
    // POST: http://localhost:5118/api/notes/
    public async Task<ActionResult> Create(Note note)
    {
        var createdNote = await _repository.CreateAsync(note);
        return CreatedAtAction(nameof(Get), new { id = createdNote.Id }, createdNote);
    }

    [HttpPut("{id::int}")]
    // PUT: http://localhost:5118/api/notes/{id}
    public async Task<ActionResult> Update(int id, [FromBody] Note newNote)
    {
        var isUpdated = await _repository.UpdateAsync(id, newNote);
        return isUpdated == false ? NotFound() : NoContent();
    }

    [HttpDelete("{id::int}")]
    // DELETE: http://localhost:5118/api/notes/{id}
    public async Task<ActionResult> Delete(int id)
    {
        var isDeleted = await _repository.DeleteAsync(id);
        return isDeleted == false ? NotFound() : NoContent();
    }
}