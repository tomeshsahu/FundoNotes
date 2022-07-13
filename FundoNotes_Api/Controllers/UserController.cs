using BusinessLayer.Interface;
using DatabaseLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FundoNotes_ADO.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IUserBL userBL;
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        [HttpPost("Register")]
        public IActionResult AddUser(UserModel user)
        {
            try
            {
                this.userBL.AddUser(user);
                return this.Ok(new { success = true, Message = "User Registration Sucessfull" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetAllUser")]
        public IActionResult GetAllUser()
        {
            try
            {
                List<GetAllUserModel> listOfUser = new List<GetAllUserModel>();
                listOfUser = this.userBL.GetAllUser();
                return Ok(new { sucess = true, Message = "Data Fetched Successfully...", data = listOfUser });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("Login")]
        public IActionResult LoginUser(LoginUserModel user)
        {
            try
            {
                string result = this.userBL.LoginUser(user);
                return Ok(new { success = true, Message = "Token Generated successfully", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("ForgetPasswordUser")]
        public IActionResult ForgetPasswordUser(string email)
        {
            try
            {
                bool result = this.userBL.ForgetPasswordUser(email);
                return Ok(new { success = true, Message = "Reset Password Link Send successfully", data = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                var identity = User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claims = identity.Claims;
                var email = claims.Where(p => p.Type == @"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").FirstOrDefault()?.Value;
                bool result = this.userBL.ResetPassword(email, resetPasswordModel);
                return Ok(new { success = true, Message = $"{email} your Password Updated successfully!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}