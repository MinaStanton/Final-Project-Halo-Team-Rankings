using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GCBlueTeamFinalProject.Models
{
    public partial class HaloRankDbContext : DbContext
    {
        public HaloRankDbContext()
        {
        }

        public HaloRankDbContext(DbContextOptions<HaloRankDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Gamers> Gamers { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:halorank.database.windows.net,1433;Database=HaloRankDb;User ID=BlueTeam;Password=Sierra-117;Encrypt=true;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Gamers>(entity =>
            {
                entity.Property(e => e.GameTypeIntId).HasColumnName("GameTypeIntID");

                entity.Property(e => e.GameTypeNvarCharId)
                    .HasColumnName("GameTypeNVarCharID")
                    .HasMaxLength(50);

                entity.Property(e => e.Gamertag)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Images).HasMaxLength(250);

                entity.Property(e => e.Kdaratio).HasColumnName("KDARatio");

                entity.Property(e => e.Kdratio).HasColumnName("KDRatio");

                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.Property(e => e.TotalTimePlayed).HasMaxLength(25);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Gamers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Gamers__UserID__619B8048");
            });

            modelBuilder.Entity<Teams>(entity =>
            {
                entity.Property(e => e.AvgKdaratio).HasColumnName("AvgKDARatio");

                entity.Property(e => e.AvgKdratio).HasColumnName("AvgKDRatio");

                entity.Property(e => e.AvgWlratio).HasColumnName("AvgWLRatio");

                entity.Property(e => e.Images).HasMaxLength(250);

                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.Property(e => e.Player1)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Player2)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Player3).HasMaxLength(15);

                entity.Property(e => e.Player4).HasMaxLength(15);

                entity.Property(e => e.Player5).HasMaxLength(15);

                entity.Property(e => e.Player6).HasMaxLength(15);

                entity.Property(e => e.Player7).HasMaxLength(15);

                entity.Property(e => e.Player8).HasMaxLength(15);

                entity.Property(e => e.TeamName).HasMaxLength(25);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Teams__UserID__6477ECF3");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.Gamertag)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Gender).HasMaxLength(15);

                entity.Property(e => e.Images).HasMaxLength(250);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(450);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Users__UserID__5EBF139D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
