using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BandAidMySql.Models
{
    public partial class bandaidContext : DbContext
    {
        public bandaidContext()
        {
        }

        public bandaidContext(DbContextOptions<bandaidContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<Statuses> Statuses { get; set; }
        public virtual DbSet<Userroles> Userroles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=root;database=bandaid");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Events>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.ToTable("events", "bandaid");

                entity.HasIndex(e => e.StatusId)
                    .HasName("FK_Event_Status_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("_idx");

                entity.Property(e => e.EventId).HasColumnType("int(11)");

                entity.Property(e => e.Adress)
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("longtext");

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.StatusId).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Event_Status");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Event_User");
            });

            modelBuilder.Entity<Reviews>(entity =>
            {
                entity.HasKey(e => e.ReviewId);

                entity.ToTable("reviews", "bandaid");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_Review_User");

                entity.Property(e => e.ReviewId).HasColumnType("int(11)");

                entity.Property(e => e.Description).HasColumnType("longtext");

                entity.Property(e => e.Rate).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Review_User");
            });

            modelBuilder.Entity<Statuses>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.ToTable("statuses", "bandaid");

                entity.Property(e => e.StatusId).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Userroles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("userroles", "bandaid");

                entity.Property(e => e.RoleId).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("users", "bandaid");

                entity.HasIndex(e => e.Email)
                    .HasName("Email_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.RoleId)
                    .HasName("FK_User_UserRole");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.Property(e => e.ActivationCode)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasColumnType("longtext");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.Facebook).HasColumnType("mediumtext");

                entity.Property(e => e.Instagram).HasColumnType("mediumtext");

                entity.Property(e => e.IsEmailVerified).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PassHash)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostCode)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileImg).HasColumnType("longtext");

                entity.Property(e => e.RoleId).HasColumnType("int(11)");

                entity.Property(e => e.Street)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Youtube).HasColumnType("mediumtext");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_UserRole");
            });
        }
    }
}
