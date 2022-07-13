﻿using BusinessLayer.Interface;
using DatabaseLayer.NoteModel;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NoteBL : INoteBL
    {
        INoteRL noteRL;
        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }
        public async Task AddNote(int UserId, NoteModel noteModel)
        {
            try
            {
                await this.noteRL.AddNote(UserId, noteModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<NodeResponseModel>> GetNote(int UserId)
        {
            try
            {
               return await this.noteRL.GetNote(UserId);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
