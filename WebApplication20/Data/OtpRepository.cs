using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication20.Models;

namespace WebApplication20.Data
{
    public class OtpRepository : IOtpRepository
    {
        private readonly OtpContext _context;
        public OtpRepository(OtpContext context)
        {
            _context = context;
        }
        public OtpTableInfo Create(OtpTableInfo OtpTableInfo)
        {
            _context.OtpTableInfo.Add(OtpTableInfo);
            _context.SaveChanges();
            return OtpTableInfo;
        }

        public OtpTableInfo GetById(int Id)
        {

            return _context.OtpTableInfo.Where(u => u.userId == Convert.ToString(Id)).ToList().LastOrDefault(); ;
            
        }
    }
}
