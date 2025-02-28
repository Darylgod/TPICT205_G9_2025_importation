using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace module_user.Models;

public partial class BonitaContext : DbContext
{
    public BonitaContext()
    {
    }

    public BonitaContext(DbContextOptions<BonitaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserContactinfo> UserContactinfos { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserMembership> UserMemberships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Role>(entity =>
        {

            // Configuration de la clé primaire composite de Role
            modelBuilder.Entity<Role>()
                .HasKey(r => new { r.TenantId, r.Id });

            //
            entity.HasKey(e => new { e.TenantId, e.Id })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("role");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(155)
                .HasDefaultValueSql("'SYSTEM'")
                .HasColumnName("createBy");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Displayname)
                .HasMaxLength(155)
                .HasColumnName("displayname");
            entity.Property(e => e.LastUpdate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("lastUpdate");
            entity.Property(e => e.Name)
                .HasMaxLength(155)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tenant");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(155)
                .HasDefaultValueSql("'SYSTEM'")
                .HasColumnName("createBy");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
        });

        modelBuilder.Entity<User>(entity =>
        {
            modelBuilder.Entity<User>()


                        .HasKey(u => new { u.TenantId, u.Id });

            //
            entity.HasKey(e => new { e.TenantId, e.Id })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("user");

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(155)
                .HasDefaultValueSql("'SYSTEM'")
                .HasColumnName("createBy");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Enable)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("enable");
            entity.Property(e => e.FirstName)
                .HasMaxLength(155)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(155)
                .HasColumnName("last_name");
            entity.Property(e => e.LastUpdate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("lastUpdate");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Title)
                .HasMaxLength(55)
                .HasColumnName("title");
            entity.Property(e => e.Username)
                .HasMaxLength(155)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserContactinfo>(entity =>
        {

            modelBuilder.Entity<UserContactinfo>()
               .HasOne(uc => uc.User)
               .WithMany(u => u.UserContactinfos)
                 .HasForeignKey(uc => new { uc.TenantId, uc.Userid })
                .HasPrincipalKey(u => new { u.TenantId, u.Id });
            //en haut ce que jai ajouter
            entity.HasKey(e => new { e.TenantId, e.Id })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("user_contactinfo");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.Userid, "userid");

            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(155)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(155)
                .HasDefaultValueSql("'System'")
                .HasColumnName("createBy");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Email)
                .HasMaxLength(155)
                .HasColumnName("email");
            entity.Property(e => e.LastUpdate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("lastUpdate");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.WhatsApp).HasMaxLength(50);
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => new { e.TenantId, e.Id })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("user_login");

            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(155)
                .HasDefaultValueSql("'SYSTEM'")
                .HasColumnName("createBy");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Displayname)
                .HasMaxLength(155)
                .HasColumnName("displayname");
            entity.Property(e => e.LastUpdate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("lastUpdate");
            entity.Property(e => e.Name)
                .HasMaxLength(155)
                .HasColumnName("name");
        });

        modelBuilder.Entity<UserMembership>(entity =>
        {
            // Configuration de la relation entre UserMembership et User
            modelBuilder.Entity<UserMembership>()
                .HasOne(um => um.User)
                .WithMany(u => u.UserMemberships)
                .HasForeignKey(um => new { um.TenantId, um.Userid })
                .HasPrincipalKey(u => new { u.TenantId, u.Id });
            // Configuration de la clé primaire composite de UserMembership (si ce n'est pas déjà fait)
            modelBuilder.Entity<UserMembership>()
                .HasKey(um => new { um.TenantId, um.Id });
            // Configuration de la relation entre UserMembership et Role
            modelBuilder.Entity<UserMembership>()
                .HasOne(um => um.Role)
                .WithMany(r => r.UserMemberships)
                .HasForeignKey(um => new { um.TenantId, um.Roleid })  // ici, on réutilise tenant_id de user_membership
                .HasPrincipalKey(r => new { r.TenantId, r.Id });
            //ejout par moi meme en haut
            entity.HasKey(e => new { e.TenantId, e.Id })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("user_membership");

            entity.HasIndex(e => e.Roleid, "roleid");

            entity.HasIndex(e => e.Userid, "userid");

            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.AssignBy)
                .HasMaxLength(155)
                .HasDefaultValueSql("'System'")
                .HasColumnName("assignBy");
            entity.Property(e => e.AssignDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("assignDate");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


  
}
