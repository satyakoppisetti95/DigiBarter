using DigiBarter.Droid.Interfaces;
using DigiBarter.Interfaces;
using DigiBarter.Models;
using Firebase.Auth;
using Plugin.CloudFirestore;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(IAuthAndroid))]
namespace DigiBarter.Droid.Interfaces
{
    class FireDbAndroid : IFireDb
    {
        public async Task<Authenticateduser> getUserProfile()
        {
            var document = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("users")
                                         .Document(FirebaseAuth.Instance.CurrentUser.Uid)
                                         .GetAsync();
            return document.ToObject<Authenticateduser>();
        }
    }
}