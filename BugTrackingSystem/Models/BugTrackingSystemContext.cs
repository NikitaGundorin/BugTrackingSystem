using System;
using Microsoft.EntityFrameworkCore;

namespace BugTrackingSystem.Models
{
    public class BugTrackingSystemContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<BugChangelog> BugChangelogs { get; set; }

        public DbSet<Importance> Importances { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Status> Statuses { get; set; }

        public BugTrackingSystemContext(DbContextOptions<BugTrackingSystemContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
