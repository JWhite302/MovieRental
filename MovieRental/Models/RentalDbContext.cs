using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MovieRental.Models;

public partial class RentalDbContext : DbContext
{
    public RentalDbContext()
    {
    }

    public RentalDbContext(DbContextOptions<RentalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Rental> Rentals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=C:\\Users\\jakob\\source\\sql\\RentalDB.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("INT");
            entity.Property(e => e.FirstName).HasColumnType("VARCHAR(25)");
            entity.Property(e => e.LastName).HasColumnType("VARCHAR(25)");
            entity.Property(e => e.MembershipDate).HasColumnType("DATE");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movie");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("INT");
            entity.Property(e => e.Genre).HasColumnType("VARCHAR(25)");
            entity.Property(e => e.ReleaseYear).HasColumnType("INT");
            entity.Property(e => e.Title).HasColumnType("VARCHAR(50)");
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("INT");
            entity.Property(e => e.CustomerId).HasColumnType("INT");
            entity.Property(e => e.MovieId).HasColumnType("INT");
            entity.Property(e => e.RentalDate).HasColumnType("DATE");
            entity.Property(e => e.ReturnDate).HasColumnType("DATE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
