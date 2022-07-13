using BusinessLayer.Interface;
using DatabaseLayer.NoteModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FundoNotes_ADO.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : Controller
    {
        INoteBL noteBL;
        public NoteController(INoteBL NoteBL)
        {
            this.noteBL = NoteBL;
        }

        [Authorize]
        [HttpPost("AddNote")]
        public async Task<IActionResult> Index(NoteModel noteModel)
        {
            if (noteModel == null)
            {
                return BadRequest("Note is null.");
            }
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                await this.noteBL.AddNote(UserId, noteModel);
                return Ok(new { success = true, Message = "Note Created Successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("GetNote")]
        public async Task<IActionResult> GetNote()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int UserId = Int32.Parse(userId.Value);
                var result = await this.noteBL.GetNote(UserId);
                return Ok(new { success = true, Message = "All Notes Fetch Successfully", data = result });
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
