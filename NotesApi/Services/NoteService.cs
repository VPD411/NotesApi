using NotesApi.Models;

namespace NotesApi.Services;

public class NoteService : INoteService
{
    private static readonly List<Note> _notes = [];
    private static int _nextId = 1;

    public List<Note> GetAll()
    {
        return [.. _notes];
    }

    public Note? GetById(int id)
    {
        return _notes.FirstOrDefault(n => n.Id == id);
    }

    public Note Create(Note note)
    {
        note.Id = _nextId++;
        _notes.Add(note);
        return note;
    }

    public bool Update(int id, Note newNote)
    {
        var existingNote = _notes.FirstOrDefault(n => n.Id == id);
        if (existingNote == null)
        {
            return false;
        }
        existingNote.Title = newNote.Title;
        existingNote.Content = newNote.Content;
        return true;
    }

    public bool Delete(int id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note == null)
        {
            return false;
        }
        _notes.Remove(note);
        return true;
    }
}
