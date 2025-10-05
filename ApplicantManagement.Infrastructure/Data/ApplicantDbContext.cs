using ApplicantManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicantManagement.Infrastructure.Data;

public class ApplicantDbContext : DbContext
{
    public ApplicantDbContext(DbContextOptions<ApplicantDbContext> options)
        : base(options)
    {
    }

    public DbSet<Applicant> Applicants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Applicant>(entity =>
        {
            entity.HasKey(e => e.ID);

            entity.Property(e => e.Name)
                .HasMaxLength(100);

            entity.Property(e => e.FamilyName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Address)
                .HasMaxLength(200);

            entity.Property(e => e.CountryOfOrigin)
                .HasMaxLength(100);

            entity.Property(e => e.EmailAddress)
                .HasMaxLength(100);

            entity.Property(e => e.Age)
                .IsRequired();

            entity.Property(e => e.Hired)
                .IsRequired();
        });
    }
}
