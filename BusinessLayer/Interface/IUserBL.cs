using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public void AddUser(UserModel user);
        List<GetAllUserModel> GetAllUser();
        public string LoginUser(LoginUserModel loginUser);

        public bool ForgetPasswordUser(string email);

        public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel);
    }
}
