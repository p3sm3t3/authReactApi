using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication20.Models;

namespace WebApplication20.Data
{
   public interface IOtpRepository
    {
        OtpTableInfo Create(OtpTableInfo otpTableInfo);
        OtpTableInfo GetById(int Id);
    }
}
