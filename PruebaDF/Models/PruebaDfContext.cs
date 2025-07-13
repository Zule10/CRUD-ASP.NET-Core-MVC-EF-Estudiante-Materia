using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PruebaDF.Models;

public partial class PruebaDfContext : DbContext
{
    public PruebaDfContext()
    {
    }

    public PruebaDfContext(DbContextOptions<PruebaDfContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MateriasEstudiante>(entity =>        {
            entity.HasKey(e => new { e.EstudianteId, e.MateriaId });
            entity.HasOne(e => e.Estudiante)
                  .WithMany(e => e.MateriasEstudiantes)
                  .HasForeignKey(e => e.EstudianteId);
            entity.HasOne(e => e.Materia)
                 .WithMany(e => e.MateriasEstudiantes)
                 .HasForeignKey(e => e.MateriaId);
            entity.ToTable("MateriasEstudiante");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.EstudianteId);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.ToTable("Estudiante");
        });
            
        modelBuilder.Entity<Materia>(entity =>        {
            entity.HasKey(e => e.MateriaId);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Creditos);
            entity.ToTable("Materia");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    
    public virtual DbSet<Estudiante> Estudiantes { get; set; }
    public virtual DbSet<Materia> Materias { get; set; }
    public virtual DbSet<MateriasEstudiante> MateriasEstudiantes { get; set; }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
