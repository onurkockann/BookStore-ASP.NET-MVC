using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BookStore.Models
{
    public partial class Model : DbContext
    {
        public Model()
            : base("name=Model1")
        {
        }

        public virtual DbSet<author> authors { get; set; }
        public virtual DbSet<book> books { get; set; }
        public virtual DbSet<cart> carts { get; set; }
        public virtual DbSet<creditCard> creditCards { get; set; }
        public virtual DbSet<genre> genres { get; set; }
        public virtual DbSet<orderedItem> orderedItems { get; set; }
        public virtual DbSet<order> orders { get; set; }
        public virtual DbSet<publisher> publishers { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<author>()
                .Property(e => e.aName)
                .IsUnicode(false);

            modelBuilder.Entity<author>()
                .Property(e => e.aDescription)
                .IsUnicode(false);

            modelBuilder.Entity<author>()
                .Property(e => e.aImage)
                .IsUnicode(false);

            modelBuilder.Entity<author>()
                .Property(e => e.aLink)
                .IsUnicode(false);

            modelBuilder.Entity<author>()
                .HasMany(e => e.books)
                .WithRequired(e => e.author1)
                .HasForeignKey(e => e.author);

            modelBuilder.Entity<book>()
                .Property(e => e.isbn)
                .HasPrecision(13, 0);

            modelBuilder.Entity<book>()
                .Property(e => e.bName)
                .IsUnicode(false);

            modelBuilder.Entity<book>()
                .Property(e => e.detail)
                .IsUnicode(false);

            modelBuilder.Entity<book>()
                .Property(e => e.bImage)
                .IsUnicode(false);

            modelBuilder.Entity<book>()
                .HasMany(e => e.carts)
                .WithOptional(e => e.book)
                .HasForeignKey(e => e.bookID);

            modelBuilder.Entity<book>()
                .HasMany(e => e.orderedItems)
                .WithRequired(e => e.book)
                .HasForeignKey(e => e.bookId);

            modelBuilder.Entity<cart>()
                .Property(e => e.bookID)
                .HasPrecision(13, 0);

            modelBuilder.Entity<creditCard>()
                .Property(e => e.cardNumber)
                .HasPrecision(16, 0);

            modelBuilder.Entity<genre>()
                .Property(e => e.gName)
                .IsUnicode(false);

            modelBuilder.Entity<genre>()
                .Property(e => e.gDescription)
                .IsUnicode(false);

            modelBuilder.Entity<genre>()
                .HasMany(e => e.books)
                .WithRequired(e => e.genre1)
                .HasForeignKey(e => e.genre);

            modelBuilder.Entity<orderedItem>()
                .Property(e => e.bookId)
                .HasPrecision(13, 0);

            modelBuilder.Entity<order>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<publisher>()
                .Property(e => e.pName)
                .IsUnicode(false);

            modelBuilder.Entity<publisher>()
                .Property(e => e.pLogo)
                .IsUnicode(false);

            modelBuilder.Entity<publisher>()
                .HasMany(e => e.books)
                .WithRequired(e => e.publisher1)
                .HasForeignKey(e => e.publisher);

            modelBuilder.Entity<user>()
                .Property(e => e.firstname)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.lastname)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.phone)
                .HasPrecision(13, 0);
        }
    }
}
