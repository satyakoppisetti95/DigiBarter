using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DigiBarter.Interfaces
{
    public interface IAuth
    {
        Task<string> LoginWithEmailPassword(string email, string password);

        Task<string> SignupWithEmailPassword(string email, string password);

        bool IsUserLoggedIn();

        void SignOut();

        string getCurrentUserUID();
    }
}
