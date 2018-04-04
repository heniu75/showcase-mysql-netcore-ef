using Microsoft.EntityFrameworkCore;

namespace MySqlEfCoreConsole
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Book { get; set; }

        public DbSet<Publisher> Publisher { get; set; }

        //public DbSet<MetaData> MetaData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=library;user=root;password=Password101");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.ISBN);
                entity.Property(e => e.Title).IsRequired();
                entity.HasOne(d => d.Publisher)
                  .WithMany(p => p.Books);
            });

            //modelBuilder.Entity<MetaData>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.DataSeeded).IsRequired();
            //    entity.Property(e => e.StatusAt).IsRequired();
            //});
        }
    }
}
