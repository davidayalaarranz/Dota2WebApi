using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using DataModel.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using static DataAccessLibrary.Data.AppConfiguration;

namespace DataAccessLibrary.Data
{
    public class Dota2AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public Dota2AppDbContext() : base() { }
        public Dota2AppDbContext(DbContextOptions<Dota2AppDbContext> options) : base(options) { }
        
        public DbSet<AppConfigurationItem> AppConfiguration { get; set; }
        public DbSet<LogEntity> Log { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<PatchVersion> PatchVersions { get; set; }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<HeroAbility> HeroAbilities { get; set; }
        public DbSet<HeroItem> HeroItems { get; set; }
        public DbSet<HeroItemComponent> HeroItemComponents { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchPlayerAbilityUpgrade> MatchPlayerAbilityUpgrades { get; set; }
        public DbSet<BuildAbilityUpgrade> BuildAbilityUpgrades { get; set; }
        public DbSet<Build> Builds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-O3VAKJL\\SQLEXPRESS2017;Database=Dota2WebApiDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppConfigurationItem>().ToTable("AppConfiguration");
            builder.Entity<LogEntity>().ToTable("Log");
            builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
            builder.Entity<PatchVersion>().ToTable("PatchVersions");
            builder.Entity<Hero>().ToTable("Heroes");
            builder.Entity<Ability>().ToTable("Abilities");
            builder.Entity<HeroItem>().ToTable("HeroItems");
            builder.Entity<HeroItemComponent>().ToTable("HeroItemComponents");
            builder.Entity<Match>().ToTable("Matches");
            builder.Entity<MatchPlayer>().ToTable("MatchPlayers");
            builder.Entity<Pick>().ToTable("Picks");
            builder.Entity<Ban>().ToTable("Bans");
            builder.Entity<Build>().ToTable("Builds");
            builder.Entity<MatchPlayerHeroItemUpgrade>().ToTable("MatchPlayerHeroItemUpgrades");

            ///Hero
            builder.Entity<Hero>()
                .HasKey(k => new { k.HeroId, k.PatchVersionId });

            /// Ability
            builder.Entity<Ability>()
                .HasKey(k => new { k.AbilityId, k.PatchVersionId });
            //builder.Entity<Ability>()
            //.Property(e => e.NotesList)
            //.HasConversion(
            //    v => string.Join(',', v),
            //    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            /// HeroItem
            builder.Entity<HeroItem>()
                .HasKey(k => new { k.HeroItemId, k.PatchVersionId });
            builder.Entity<HeroItem>()
                .HasOne(hi => hi.PatchVersion)
                .WithMany(pv => pv.HeroItems)
                .HasForeignKey(hi => hi.PatchVersionId);

            /// PickBan
            builder.Entity<Pick>()
                .HasKey(k => new { k.MatchId, k.Order });
            builder.Entity<Ban>()
                .HasKey(k => new { k.MatchId, k.Order });

            /// HeroItemUpgrade
            builder.Entity<MatchPlayerHeroItemUpgrade>()
                .HasOne(hiu => hiu.HeroItem)
                .WithMany(hi => hi.MatchPlayerUpgrades)
                .HasForeignKey(hiu => new { hiu.HeroItemId, hiu.PatchVersionId })
                .OnDelete(DeleteBehavior.NoAction);


            /// MatchPlayerAbilityUpgrade
            builder.Entity<MatchPlayerAbilityUpgrade>()
                .HasKey(k => new { k.MatchPlayerMatchId, k.MatchPlayerPlayerId, k.MatchPlayerPlayerSlot, k.Level });
            builder.Entity<MatchPlayerAbilityUpgrade>()
                .HasOne(hau => hau.Ability)
                .WithMany(a => a.MatchPlayerUpgrades)
                .HasForeignKey(k => new { k.AbilityId, k.PatchVersionId })
                .OnDelete(DeleteBehavior.NoAction);

            /// BuildAbilityUpgrade
            builder.Entity<BuildAbilityUpgrade>()
                .HasKey(k => new { k.BuildId, k.Level });
            builder.Entity<BuildAbilityUpgrade>()
               .HasOne(hau => hau.Ability)
               .WithMany(a => a.BuildUpgrades)
               .HasForeignKey(k => new { k.AbilityId, k.PatchVersionId })
               .OnDelete(DeleteBehavior.NoAction);



            /// Build
            builder.Entity<Build>()
                .HasOne(b => b.User)
                .WithMany(u => u.Builds)
                .HasForeignKey(b => b.UserId);

            /// HeroAbility
            builder.Entity<HeroAbility>()
                .HasKey(t => new { t.HeroAbilityId });
            builder.Entity<HeroAbility>()
                .HasOne(ha => ha.Hero)
                .WithMany(h => h.HeroAbilities)
                .HasForeignKey(k => new { k.HeroId, k.HeroPatchVersionId })
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<HeroAbility>()
                .HasOne(ha => ha.Ability)
                .WithMany(a => a.Heroes)
                .HasForeignKey(k => new { k.AbilityId, k.AbilityPatchVersionId })
                .OnDelete(DeleteBehavior.NoAction);

            /// MatchPlayer
            builder.Entity<MatchPlayer>()
                .HasKey(k => new { k.MatchId, k.PlayerId, k.PlayerSlot });
            builder.Entity<MatchPlayer>()
                .HasOne(mp => mp.Match)
                .WithMany(m => m.MatchPlayers)
                .HasForeignKey(mp => mp.MatchId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<MatchPlayer>()
                .HasOne(mp => mp.Player)
                .WithMany(p => p.Matches)
                .HasForeignKey(mp => mp.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<MatchPlayer>()
                .HasOne(mp => mp.Hero)
                .WithMany(h => h.Matches)
                .HasForeignKey(mp => new { mp.HeroId, mp.PatchVersionId })
                .OnDelete(DeleteBehavior.Cascade);
            
            /// HeroItem
            builder.Entity<HeroItemComponent>()
                .HasKey(t => new { t.ComponentId, t.ComponentPatchVersionId, t.HeroItemId, t.HeroItemPatchVersionId });
            builder.Entity<HeroItemComponent>()
                .HasOne(t => t.Component)
                .WithMany(c => c.IsComponentOf)
                .HasForeignKey(t => new { t.ComponentId, t.ComponentPatchVersionId })
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<HeroItemComponent>()
                .HasOne(t => t.HeroItem)
                .WithMany(c => c.Components)
                .HasForeignKey(t => new { t.HeroItemId, t.HeroItemPatchVersionId })
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
