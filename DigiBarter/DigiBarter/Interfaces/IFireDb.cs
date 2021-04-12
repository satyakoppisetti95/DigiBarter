using DigiBarter.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DigiBarter.Interfaces
{
    public interface IFireDb
    {
        Task<Authenticateduser> getUserProfile();
    }
}
