using DatabaseLayer.NoteModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface INoteBL
    {
        Task AddNote(int UserId, NoteModel noteModel);

        Task<List<NodeResponseModel>> GetNote(int UserId);
    }
}
