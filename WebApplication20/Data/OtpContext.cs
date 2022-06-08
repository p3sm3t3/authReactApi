using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication20.Models;

namespace WebApplication20.Data
{
    public class OtpContext:DbContext
    {
        public OtpContext(DbContextOptions<OtpContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<OtpTableInfo>(entety =>
            //{
            //    entety.HasIndex(e => e.Email).IsUnique();
            //});
        }
        public DbSet<OtpTableInfo> OtpTableInfo { get; set; }
    }
}
