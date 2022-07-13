using DatabaseLayer.NoteModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INoteRL
    {
        Task AddNote(int UserId, NoteModel noteModel);
        Task<List<NodeResponseModel>> GetNote(int UserId);
    }
}
