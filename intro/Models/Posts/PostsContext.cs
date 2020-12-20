using System;
using System.Linq;
using intro.Models.WhiteLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace intro.Models.Posts
{
    public partial class PostsContext : DbContext
    {
        public PostsContext()
        {
        }

        public PostsContext(DbContextOptions<PostsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PostImage> PostImage { get; set; }
        public virtual DbSet<PostTagList> PostTagList { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostImage>(entity =>
            {
                entity.ToTable("post_image");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.IImage)
                    .HasColumnName("i_image");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.UId)
                    .HasColumnName("u_id")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<PostTagList>(entity =>
            {
                entity.HasKey(e => new { e.TId, e.PId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("post_tag_list");

                entity.HasIndex(e => e.PId)
                    .HasName("p_id");

                entity.Property(e => e.TId)
                    .HasColumnName("t_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.PId)
                    .HasColumnName("p_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.HasOne(d => d.P)
                    .WithMany(p => p.PostTagList)
                    .HasForeignKey(d => d.PId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("post_tag_list_ibfk_1");

                entity.HasOne(d => d.T)
                    .WithMany(p => p.PostTagList)
                    .HasForeignKey(d => d.TId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("post_tag_list_ibfk_2");
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.ToTable("posts");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.PAccountId)
                    .HasColumnName("p_account_id")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.PContent)
                    .IsRequired()
                    .HasColumnName("p_content")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.PCount)
                    .HasColumnName("p_count")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.PName)
                    .IsRequired()
                    .HasColumnName("p_name")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.PPhoto).HasColumnName("p_photo");
            });

            modelBuilder.Entity<Tags>(entity =>
            {
                entity.ToTable("tags");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.TName)
                    .IsRequired()
                    .HasColumnName("t_name")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
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
    }
}
