using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace intro.Models.WhiteLists
{
    public partial class WhiteListContext : DbContext
    {
        public WhiteListContext()
        {
        }

        public WhiteListContext(DbContextOptions<WhiteListContext> options)
            : base(options)
        {
        }

        public WhiteListContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Whitelists> Whitelists { get; set; }
        public virtual DbSet<WlContent> WlContent { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Whitelists>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Name")
                    .IsUnique();

                entity.HasIndex(e => e.Route)
                    .HasName("Route")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Remarks)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Route)
                    .IsRequired()
                    .HasColumnType("varchar(60)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<WlContent>(entity =>
            {
                entity.ToTable("WL_Content");

                entity.HasIndex(e => new { e.Wid, e.Content })
                    .HasName("Wid")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
                entity.Property(e => e.Policy)
                    .IsRequired()
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Wid).HasColumnType("bigint(20)");

                entity.HasOne(d => d.W)
                    .WithMany(p => p.Source)
                    .HasForeignKey(d => d.Wid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WL_Content_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


        public override int SaveChanges()
        {
            try
            {
                AddAuitInfo();
                return base.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public async System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            try
            {
                AddAuitInfo();
                return await base.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

        }
        private void AddAuitInfo()
        {
            var entries = base.ChangeTracker.Entries()
                .Where(x => x.Entity is ITimeLogger && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((ITimeLogger)entry.Entity).Created = DateTime.UtcNow;
                }
                ((ITimeLogger)entry.Entity).Modified = DateTime.UtcNow;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();

            var sql = new Config.Sql();
            config.GetSection("Sql").Bind(sql);
            sql.UseSqlService(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
