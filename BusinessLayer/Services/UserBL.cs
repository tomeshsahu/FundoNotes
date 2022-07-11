using BusinessLayer.Interface;
using DatabaseLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;


namespace BusinessLayer.Services

{
    public class UserBL : IUserBL
    {
        IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }
        public void AddUser(UserModel user)
        {
            try
            {
                this.userRL.AddUser(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetAllUserModel> GetAllUser()
        {
            return this.userRL.GetAllUser();
        }

        public string LoginUser(LoginUserModel loginUser)
        {
            try
            {
                return this.userRL.LoginUser(loginUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ForgetPasswordUser(string email)
        {
            try
            {
                return this.userRL.ForgetPasswordUser(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}