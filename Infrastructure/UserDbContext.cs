using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Models;

namespace View.Infrastructure
{
    public abstract class UserDbContext : DbContext
    {
        public UserDbContext()
        {
            Database.EnsureCreated();
        }
        public virtual DbSet<AccountModel> Accounts { get; set; }
        public virtual DbSet<ProjectModel> Projects { get; set; }
        public virtual DbSet<ShareUserProject> ShareUserProjects { get; set; }
        public virtual DbSet<DrawingComponentModel> DrawingComponentModels { get; set; }

        protected abstract void InitialiseDatabase(DbContextOptionsBuilder optionsBuilder);
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            SQLitePCL.Batteries.Init();
            InitialiseDatabase(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountModel>(x => x.HasKey(y => y.AccountId))
                .Entity<ProjectModel>(x => x.HasKey(y => y.ProjectId))
                .Entity<ShareUserProject>(x => x.HasKey(y => y.ShareUserId))
                .Entity<DrawingComponentModel>(x => x.HasKey(y => y.DrawingComponentId));

            base.OnModelCreating(modelBuilder);
        }


    }
}
