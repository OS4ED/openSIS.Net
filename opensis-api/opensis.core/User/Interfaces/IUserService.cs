using opensis.data.Models;
using opensis.data.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace opensis.core.User.Interfaces
{
    public interface IUserService
    {
        
        public LoginViewModel ValidateUserLogin(LoginViewModel ObjModel);
        public CheckUserEmailAddressViewModel CheckUserLoginEmail(CheckUserEmailAddressViewModel checkUserEmailAddressViewModel);
        public LoginViewModel RefreshToken(LoginViewModel ObjModel);
    }
}
