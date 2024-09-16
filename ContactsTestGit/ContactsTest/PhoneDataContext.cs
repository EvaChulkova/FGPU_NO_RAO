using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsTest
{
    public class PhoneDataContext : DbContext
    {
        public DbSet<MainData> MainData { get; set; } = null;
        public DbSet<Phones> Phones { get; set; } = null;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = localhost; Database = Phone; Integrated Security = True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Phones>()
                .HasOne(p => p.data)
                .WithMany(b => b.phoneNumber).HasForeignKey(t => t.id_fk).HasPrincipalKey(t => t.id);

                
            base.OnModelCreating(modelBuilder);
        }
    }
}
