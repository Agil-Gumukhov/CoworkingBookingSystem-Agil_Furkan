using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Coworking.APP.Domain
{
    public class CoworkingDb : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public CoworkingDb(DbContextOptions<CoworkingDb> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Branch>()
                .HasMany(b => b.Rooms)
                .WithOne(r => r.Branch)
                .HasForeignKey(r => r.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Branch>()
                .HasMany(b => b.Desks)
                .WithOne(d => d.Branch)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Bookings)
                .WithOne(b => b.Room)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Desk>()
                .HasMany(d => d.Bookings)
                .WithOne(b => b.Desk)
                .HasForeignKey(b => b.DeskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Branch>()
                .HasMany<Booking>()
                .WithOne(b => b.Branch)
                .HasForeignKey(b => b.BranchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}