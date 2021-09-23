using Friendlizer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friendlizer
{
    public class FriendsDbContext : DbContext
    {
        public FriendsDbContext(DbContextOptions<FriendsDbContext> options)
            : base(options)
        {
        }

        public DbSet<FriendsSet> FriendsSetItems { get; set; }
        public DbSet<Relation> Relations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Relation>().HasKey(table => new { table.FriendsSetId, table.FirstPersonId, table.SecondPersonId });
        }
    }
}
