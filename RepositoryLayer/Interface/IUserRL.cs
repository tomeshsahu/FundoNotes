using DatabaseLayer;

using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public void AddUser(UserModel user);
        List<GetAllUserModel> GetAllUser();

        public string LoginUser(LoginUserModel loginUser);

        public bool ForgetPasswordUser(string email);
    }
}
