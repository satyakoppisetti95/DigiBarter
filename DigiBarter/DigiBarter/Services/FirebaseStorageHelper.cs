using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Firebase.Storage;

namespace DigiBarter.Services
{
    public class FirebaseStorageHelper
    {
        FirebaseStorage firebaseStorage = new FirebaseStorage("digibarter-e1298.appspot.com");

        public async Task<string> UploadFile(Stream fileStream, string fileName,string folder)
        {
            var imageUrl = await firebaseStorage
                .Child(folder)
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;
        }

        public async Task<string> GetFile(string fileName,string folder)
        {
            return await firebaseStorage
                .Child(folder)
                .Child(fileName)
                .GetDownloadUrlAsync();
        }
        public async Task DeleteFile(string fileName, string folder)
        {
            await firebaseStorage
                 .Child(folder)
                 .Child(fileName)
                 .DeleteAsync();

        }
    }
}

