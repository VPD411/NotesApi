using NotesApi.Models;

namespace NotesApi.Services
{
    public interface INoteService
    {
        Note Create(Note note);
        bool Delete(int id);
        List<Note> GetAll();
        Note? GetById(int id);
        bool Update(int id, Note newNote);
    }
}