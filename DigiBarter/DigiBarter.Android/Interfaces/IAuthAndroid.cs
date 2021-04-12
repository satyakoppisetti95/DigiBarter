using Android.Gms.Extensions;
using DigiBarter.Droid.Interfaces;
using DigiBarter.Interfaces;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(IAuthAndroid))]
namespace DigiBarter.Droid.Interfaces
{
    public class IAuthAndroid : IAuth
    {
        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = await user.User.GetIdToken(false).AsAsync<GetTokenResult>();
                return token.Token;
            }
            
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return e.Message;

            }
            catch (System.Exception e)
            {
                return e.Message;
            }
        }

        public async Task<string> SignupWithEmailPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var token = await user.User.GetIdToken(false).AsAsync<GetTokenResult>();

                
                return token.Token;
            }
            catch (Exception e) { 

                return e.Message;
            }
        }

        public  bool IsUserLoggedIn()
        {
            return FirebaseAuth.Instance.CurrentUser != null;
        }

        public string getCurrentUserUID()
        {
            return FirebaseAuth.Instance.CurrentUser.Uid;
        }

        public void SignOut()
        {
            FirebaseAuth.Instance.SignOut();
        }
    }
}