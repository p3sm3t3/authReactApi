using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication20.Models;

namespace WebApplication20.Data
{
   public interface IUserRepository
    {
        User Create(User user);
        User GetByEmail(string email);
        User GetById(int Id);
       List<User>  GetAll();
    }
}
