using DatabaseLayer.NoteModel;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class NoteRL : INoteRL
    {
        private readonly string connectionString;
        public NoteRL(IConfiguration configuartion)
        {
            connectionString = configuartion.GetConnectionString("Fundoonotes");
        }
        public async Task AddNote(int UserId, NoteModel noteModel)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                using (connection)
                {
                    connection.Open();
                    //Creating a stored Procedure for adding Users into database
                    SqlCommand com = new SqlCommand("spAddNote", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Title", noteModel.Title);
                    com.Parameters.AddWithValue("@Description", noteModel.Description);
                    com.Parameters.AddWithValue("@BgColor", noteModel.Bgcolor);
                    com.Parameters.AddWithValue("@UserId", UserId);
                    await com.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<NodeResponseModel>> GetNote(int UserId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            List<NodeResponseModel> notes =new List<NodeResponseModel>();
            try
            {
                using (connection)
                {
                    connection.Open();   

                    SqlCommand com = new SqlCommand("spGetNote", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@UserId", UserId);
                    SqlDataReader rd = await com.ExecuteReaderAsync();

                    while (rd.Read())
                    {
                        NodeResponseModel response = new NodeResponseModel();

                        response.NoteId = rd["NoteId"] == DBNull.Value ? default : rd.GetInt32("NoteId");
                        response.Title = rd["Title"] == DBNull.Value ? default : rd.GetString("Title");
                        response.Description = rd["Description"] == DBNull.Value ? default : rd.GetString("Description");
                        response.Bgcolor = rd["Bgcolor"] == DBNull.Value ? default : rd.GetString("Bgcolor");
                        response.IsPin = rd["IsPin"] == DBNull.Value ? default : rd.GetBoolean("IsPin");
                        response.IsArchive = rd["IsArchive"] == DBNull.Value ? default : rd.GetBoolean("IsArchive");
                        response.IsRemainder = rd["IsRemainder"] == DBNull.Value ? default : rd.GetBoolean("IsRemainder");
                        response.IsTrash = rd["IsTrash"] == DBNull.Value ? default : rd.GetBoolean("IsTrash");
                        response.UserId = rd["UserId"] == DBNull.Value ? default : rd.GetInt32("UserId");
                        response.RegisteredDate = rd["RegisteredDate"] == DBNull.Value ? default : rd.GetDateTime("RegisteredDate");
                        response.Remainder = rd["Remainder"] == DBNull.Value ? default : rd.GetDateTime("Remainder");
                        response.ModifiedDate = rd["ModifiedDate"] == DBNull.Value ? default : rd.GetDateTime("ModifiedDate");
                        notes.Add(response);
                    }
                    return notes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task UpdateNote(int UserId, int NoteId, UpdateNoteModel noteModel)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            var result = 0;
            try
            {
                using (connection)
                {
                    connection.Open();
                    SqlCommand com = new SqlCommand("spUpdateNote", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@title", noteModel.Title);
                    com.Parameters.AddWithValue("@description", noteModel.Description);
                    com.Parameters.AddWithValue("@Bgcolor", noteModel.Bgcolor);
                    com.Parameters.AddWithValue("@UserId", UserId);
                    com.Parameters.AddWithValue("@NoteId", NoteId);
                    com.Parameters.AddWithValue("@IsPin", noteModel.IsPin);
                    com.Parameters.AddWithValue("@IsArchive", noteModel.IsArchive);
                    com.Parameters.AddWithValue("@IsTrash", noteModel.IsTrash);
                    result = await com.ExecuteNonQueryAsync();
                    if (result <= 0)
                    {
                        throw new Exception("Note Does not Exist");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

