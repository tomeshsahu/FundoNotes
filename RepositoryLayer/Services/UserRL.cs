using DatabaseLayer;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Experimental.System.Messaging;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        private readonly string connetionString;
        public UserRL(IConfiguration configuration)
        {
            connetionString = configuration.GetConnectionString("Fundoonotes");
        }
        public void AddUser(UserModel user)
        {
            SqlConnection sqlconnection = new SqlConnection(connetionString);
            try
            {
                using (sqlconnection)
                {
                    sqlconnection.Open();
                    SqlCommand cmd = new SqlCommand("spAddUser", sqlconnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Firstname", user.Firstname);
                    cmd.Parameters.AddWithValue("@Lastname", user.Lastname);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlconnection.Close();
            }
        }

       
      

    public List<GetAllUserModel> GetAllUser()
    {

            List<GetAllUserModel> users = new List<GetAllUserModel>();
            SqlConnection connection = new SqlConnection(connetionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    SqlCommand com = new SqlCommand("spGetAllUser", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        GetAllUserModel user = new GetAllUserModel();
                        user.UserId = reader["UserId"] == DBNull.Value ? default : reader.GetInt32("UserId");
                        user.Firstname = reader["Firstname"] == DBNull.Value ? default : reader.GetString("Firstname");
                        user.Lastname = reader["Lastname"] == DBNull.Value ? default : reader.GetString("Lastname");
                        user.Email = reader["Email"] == DBNull.Value ? default : reader.GetString("Email");
                        user.Password = reader["Password"] == DBNull.Value ? default : reader.GetString("Password");
                        user.CreateDate = reader["CreateDate"] == DBNull.Value ? default : reader.GetDateTime("CreateDate");
                        user.MoidifyDate = reader["MoidifyDate"] == DBNull.Value ? default : reader.GetDateTime("MoidifyDate");
                        users.Add(user);
                    }
                    return users;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public string LoginUser(LoginUserModel loginUser)
        {
            SqlConnection connection = new SqlConnection(connetionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    SqlCommand com = new SqlCommand("spLoginUser", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Email", loginUser.Email);
                    com.Parameters.AddWithValue("@Password", loginUser.Password);
                    var result = com.ExecuteNonQuery();
                    SqlDataReader rd = com.ExecuteReader();
                    GetAllUserModel response = new GetAllUserModel();
                    if (rd.Read())
                    {
                        response.UserId = rd["UserId"] == DBNull.Value ? default : rd.GetInt32("UserId");
                        response.Email = rd["Email"] == DBNull.Value ? default : rd.GetString("Email");
                        response.Password = rd["Password"] == DBNull.Value ? default : rd.GetString("Password");

                    }
                    return GenerateJWTToken(response.Email, response.UserId);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateJWTToken(string email, int userId)
        {
            try
            {
                // generate token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("email", email),
                    new Claim("userId",userId.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),

                    SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public bool ForgetPasswordUser(string email)
        {
            SqlConnection connection = new SqlConnection(connetionString);

            try
            {
                using (connection)
                {
                    connection.Open();
                    SqlCommand com = new SqlCommand("spForgetPasswordUser", connection);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Email", email);
                    var result = com.ExecuteNonQuery();
                    SqlDataReader rd = com.ExecuteReader();
                    GetAllUserModel response = new GetAllUserModel();
                    if (rd.Read())
                    {
                        response.UserId = rd["UserId"] == DBNull.Value ? default : rd.GetInt32("UserId");
                        response.Email = rd["Email"] == DBNull.Value ? default : rd.GetString("Email");

                    }
                    MessageQueue messageQueue;
                    //ADD MESSAGE TO QUEUE
                    if (MessageQueue.Exists(@".\Private$\FundooQueue"))
                    {
                        messageQueue = new MessageQueue(@".\Private$\FundooQueue");
                    }
                    else
                    {
                        messageQueue = MessageQueue.Create(@".\Private$\FundooQueue");
                    }
                    Message MyMessage = new Message();
                    MyMessage.Formatter = new BinaryMessageFormatter();
                    MyMessage.Body = GenerateJWTToken(email, response.UserId);
                    MyMessage.Label = "Forget Password Email";
                    messageQueue.Send(MyMessage);
                    Message msg = messageQueue.Receive();
                    msg.Formatter = new BinaryMessageFormatter();
                    EmailService.SendEmail(email, msg.Body.ToString());
                    messageQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(msmqQueue_ReceiveCompleted);

                    messageQueue.BeginReceive();
                    messageQueue.Close();
                    return true;


                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void msmqQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                EmailService.SendEmail(e.Message.ToString(), GenerateToken(e.Message.ToString()));
                queue.BeginReceive();
            }
            catch (MessageQueueException ex)
            {
                if (ex.MessageQueueErrorCode ==
                    MessageQueueErrorCode.AccessDenied)
                {
                    Console.WriteLine("Access is denied. " +
                        "Queue might be a system queue.");
                }
            }
        }

        private string GenerateToken(string email)
        {
            try
            {
                // generate token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("email", email)

                    }),
                    Expires = DateTime.UtcNow.AddHours(2),

                    SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}