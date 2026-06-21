using Microsoft.AspNetCore.Mvc;
using NotesApi.Models;
using NotesApi.Services;

namespace NotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]

// http://localhost:5118/api/notes
public class NotesController : ControllerBase
{
    private readonly INoteService _service;

    public NotesController(INoteService service)
    {
        _service = service;
    }

    [HttpGet]
    // GET: http://localhost:5118/api/notes
    public ActionResult<List<Note>> GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("{id::int}")]
    // GET: http://localhost:5118/api/notes/{id}
    public ActionResult<Note> Get(int id)
    {
        var note = _service.GetById(id);
        return note == null ? NotFound() : Ok(note);
    }

    [HttpPost]
    // POST: http://localhost:5118/api/notes/
    public ActionResult Create(Note note)
    {
        var created = _service.Create(note);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id::int}")]
    // PUT: http://localhost:5118/api/notes/{id}
    public ActionResult Uodate(int id, Note newNote)
    {
        return _service.Update(id, newNote) ? NoContent() : NotFound();
    }

    [HttpDelete("{id::int}")]
    // DELETE: http://localhost:5118/api/notes/{id}
    public ActionResult Delete(int id)
    {
        return _service.Delete(id) ? NoContent() : NotFound();
    }
}