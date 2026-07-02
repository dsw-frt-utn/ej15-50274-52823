using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data
{
    public class Dsw2026Ej15DbContext: DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public Dsw2026Ej15DbContext(DbContextOptions<Dsw2026Ej15DbContext> options): base(options){
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(d => d.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(d => d.LicenseNumber)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasOne(d => d.Speciality)
                      .WithMany()
                      .HasForeignKey(d => d.SpecialityId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.Property(s => s.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(s => s.Description)
                      .HasMaxLength(500);
            });
        }
    }
}
