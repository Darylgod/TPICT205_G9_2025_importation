using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace module_admin_2.Models;

public partial class MyDbContext2 : DbContext
{
    public MyDbContext2()
    {
    }

    public MyDbContext2(DbContextOptions<MyDbContext2> options)
        : base(options)
    {
    }

    public virtual DbSet<Classe> Classes { get; set; }

    public virtual DbSet<Etudiant> Etudiants { get; set; }

    public virtual DbSet<Evaluation> Evaluations { get; set; }

    public virtual DbSet<Filiere> Filieres { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Niveau> Niveaus { get; set; }

    public virtual DbSet<Programme> Programmes { get; set; }

    public virtual DbSet<Specialite> Specialites { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Classe>(entity =>
        {
            entity.HasKey(e => e.IdClasse).HasName("PRIMARY");

            entity.ToTable("classe");

            entity.HasIndex(e => e.CodeGrade, "code_grade");

            entity.HasIndex(e => e.CodeNiveau, "code_niveau");

            entity.HasIndex(e => e.IdFiliere, "id_filiere");

            entity.HasIndex(e => e.IdSpecialite, "id_specialite");

            entity.HasIndex(e => e.IdTenant, "id_tenant");

            entity.Property(e => e.IdClasse)
                .HasColumnType("int(11)")
                .HasColumnName("id_classe");
            entity.Property(e => e.CodeGrade)
                .HasMaxLength(50)
                .HasColumnName("code_grade");
            entity.Property(e => e.CodeNiveau)
                .HasMaxLength(50)
                .HasColumnName("code_niveau");
            entity.Property(e => e.IdFiliere)
                .HasColumnType("int(11)")
                .HasColumnName("id_filiere");
            entity.Property(e => e.IdSpecialite)
                .HasColumnType("int(11)")
                .HasColumnName("id_specialite");
            entity.Property(e => e.IdTenant)
                .HasColumnType("int(11)")
                .HasColumnName("id_tenant");
        });

        modelBuilder.Entity<Etudiant>(entity =>
        {
            entity.HasKey(e => e.IdEtudiant).HasName("PRIMARY");

            entity.ToTable("etudiant");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.IdClasse, "id_classe");

            entity.HasIndex(e => e.IdTenant, "id_tenant");

            entity.HasIndex(e => e.Matricule, "matricule").IsUnique();

            entity.Property(e => e.IdEtudiant)
                .HasColumnType("int(11)")
                .HasColumnName("id_etudiant");
            entity.Property(e => e.Adresse)
                .HasColumnType("text")
                .HasColumnName("adresse");
            entity.Property(e => e.DateNaissance).HasColumnName("date_naissance");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.IdClasse)
                .HasColumnType("int(11)")
                .HasColumnName("id_classe");
            entity.Property(e => e.IdTenant)
                .HasColumnType("int(11)")
                .HasColumnName("id_tenant");
            entity.Property(e => e.Matricule)
                .HasMaxLength(50)
                .HasColumnName("matricule");
            entity.Property(e => e.Nom)
                .HasMaxLength(255)
                .HasColumnName("nom");
            entity.Property(e => e.Prenom)
                .HasMaxLength(255)
                .HasColumnName("prenom");
            entity.Property(e => e.Telephone)
                .HasMaxLength(20)
                .HasColumnName("telephone");
            entity.Property(e => e.VilleNaissance)
                .HasMaxLength(255)
                .HasColumnName("ville_naissance");
        });

        modelBuilder.Entity<Evaluation>(entity =>
        {
            entity.HasKey(e => e.IdEvaluation).HasName("PRIMARY");

            entity.ToTable("evaluation");

            entity.HasIndex(e => e.IdEtudiant, "id_etudiant");

            entity.HasIndex(e => e.IdProgramme, "id_programme");

            entity.HasIndex(e => e.IdTenant, "id_tenant");

            entity.Property(e => e.IdEvaluation)
                .HasColumnType("int(11)")
                .HasColumnName("id_evaluation");
            entity.Property(e => e.IdEtudiant)
                .HasColumnType("int(11)")
                .HasColumnName("id_etudiant");
            entity.Property(e => e.IdProgramme)
                .HasColumnType("int(11)")
                .HasColumnName("id_programme");
            entity.Property(e => e.IdTenant)
                .HasColumnType("int(11)")
                .HasColumnName("id_tenant");
            entity.Property(e => e.Note)
                .HasPrecision(5, 2)
                .HasColumnName("note");
        });

        modelBuilder.Entity<Filiere>(entity =>
        {
            entity.HasKey(e => e.IdFiliere).HasName("PRIMARY");

            entity.ToTable("filiere");

            entity.HasIndex(e => e.CodeFiliere, "code_filiere").IsUnique();

            entity.HasIndex(e => e.IdTenant, "id_tenant");

            entity.Property(e => e.IdFiliere)
                .HasColumnType("int(11)")
                .HasColumnName("id_filiere");
            entity.Property(e => e.CodeFiliere)
                .HasMaxLength(50)
                .HasColumnName("code_filiere");
            entity.Property(e => e.IdTenant)
                .HasColumnType("int(11)")
                .HasColumnName("id_tenant");
            entity.Property(e => e.Intitule)
                .HasMaxLength(255)
                .HasColumnName("intitule");
            entity.Property(e => e.Tittle)
                .HasMaxLength(255)
                .HasColumnName("tittle");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.IdGrade).HasName("PRIMARY");

            entity.ToTable("grade");

            entity.HasIndex(e => e.CodeGrade, "code_grade").IsUnique();

            entity.HasIndex(e => e.IdTenant, "id_tenant");

            entity.Property(e => e.IdGrade)
                .HasColumnType("int(11)")
                .HasColumnName("id_grade");
            entity.Property(e => e.CodeGrade)
                .HasMaxLength(50)
                .HasColumnName("code_grade");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IdTenant)
                .HasColumnType("int(11)")
                .HasColumnName("id_tenant");
        });

        modelBuilder.Entity<Niveau>(entity =>
        {
            entity.HasKey(e => e.IdNiveau).HasName("PRIMARY");

            entity.ToTable("niveau");

            entity.HasIndex(e => e.CodeNiveau, "code_niveau").IsUnique();

            entity.HasIndex(e => e.IdTenant, "id_tenant");

            entity.Property(e => e.IdNiveau)
                .HasColumnType("int(11)")
                .HasColumnName("id_niveau");
            entity.Property(e => e.CodeNiveau)
                .HasMaxLength(50)
                .HasColumnName("code_niveau");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IdTenant)
                .HasColumnType("int(11)")
                .HasColumnName("id_tenant");
        });

        modelBuilder.Entity<Programme>(entity =>
        {
            entity.HasKey(e => e.IdProgramme).HasName("PRIMARY");

            entity.ToTable("programme");

            entity.HasIndex(e => e.IdClasse, "id_classe");

            entity.HasIndex(e => e.IdTenant, "id_tenant");

            entity.Property(e => e.IdProgramme)
                .HasColumnType("int(11)")
                .HasColumnName("id_programme");
            entity.Property(e => e.IdClasse)
                .HasColumnType("int(11)")
                .HasColumnName("id_classe");
            entity.Property(e => e.IdTenant)
                .HasColumnType("int(11)")
                .HasColumnName("id_tenant");
            entity.Property(e => e.Nom)
                .HasMaxLength(255)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<Specialite>(entity =>
        {
            entity.HasKey(e => e.IdSpecialite).HasName("PRIMARY");

            entity.ToTable("specialite");

            entity.HasIndex(e => e.CodeSpecialite, "code_specialite").IsUnique();

            entity.HasIndex(e => e.IdTenant, "id_tenant");

            entity.Property(e => e.IdSpecialite)
                .HasColumnType("int(11)")
                .HasColumnName("id_specialite");
            entity.Property(e => e.CodeSpecialite)
                .HasMaxLength(50)
                .HasColumnName("code_specialite");
            entity.Property(e => e.IdTenant)
                .HasColumnType("int(11)")
                .HasColumnName("id_tenant");
            entity.Property(e => e.Intitule)
                .HasMaxLength(255)
                .HasColumnName("intitule");
            entity.Property(e => e.Tittle)
                .HasMaxLength(255)
                .HasColumnName("tittle");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
