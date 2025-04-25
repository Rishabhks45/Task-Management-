using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore;
using SimpleTaskTrackr.Models.SimpleTaskTrackModel;

namespace SimpleTaskTrackr.Data
{
    public class SimpleTaskTrackrDbContext : IdentityDbContext
    {
        public SimpleTaskTrackrDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TasksProperties> TasksProperties { get; set; }
        public DbSet<Userproperty> Userproperties { get; set; }
        public DbSet<TasksUpdatedBy> TaskUpdatedBys { get; set; }




    }

}
