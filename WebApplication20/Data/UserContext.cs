using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication20.Models;

namespace WebApplication20.Data
{
    public class UserContext :DbContext
    {

        public UserContext(DbContextOptions<UserContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entety =>
            {
                entety.HasIndex(e => e.Email).IsUnique();
            }); 
        }
        public DbSet<User> Users { get; set; }
    }
}
 