using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DataAccessLibrary.Data
{
    public class Dota2AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public Dota2AppDbContext() : base() { }
        public Dota2AppDbContext(DbContextOptions<Dota2AppDbContext> options) : base(options) { }

        public DbSet<LogEntity> Log { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<HeroItem> HeroItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-O3VAKJL\\SQLEXPRESS2017;Database=Dota2WebApiDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LogEntity>().ToTable("Log");
            builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
            builder.Entity<Hero>().ToTable("Heroes");
            builder.Entity<HeroItem>().ToTable("HeroItems");
            builder.Entity<HeroItemComponent>().ToTable("HeroItemComponents");

            //builder.Entity<HeroItem>()
            //    .HasMany<HeroItemComponent>(hi => hi.Componentes);

            builder.Entity<HeroItemComponent>()
                .HasKey(t => new { t.ComponentId, t.HeroItemId });
            builder.Entity<HeroItemComponent>()
                .HasOne(t => t.Component)
                .WithMany(c => c.IsComponentOf)
                .HasForeignKey(t => t.ComponentId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<HeroItemComponent>()
                .HasOne(t => t.HeroItem)
                .WithMany(c => c.Components)
                .HasForeignKey(t => t.HeroItemId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
