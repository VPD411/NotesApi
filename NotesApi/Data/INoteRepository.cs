using NotesApi.Models;

namespace NotesApi.Data
{
    public interface INoteRepository
    {
        Task<Note> CreateAsync(Note note);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Note>> GetAllAsync();
        Task<Note?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Note newNote);
    }
}