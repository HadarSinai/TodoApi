using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TodoApi;

public partial class ToDoDbContext : DbContext
{
    public ToDoDbContext()
    {
    }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=DefaultConnection", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.22-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Iditem).HasName("PRIMARY");

            entity.ToTable("item");

            entity.Property(e => e.Iditem).HasColumnName("iditem");
            entity.Property(e => e.Iscomplate).HasColumnName("iscomplate");
            entity.Property(e => e.Taskname)
                .HasMaxLength(100)
                .HasColumnName("taskname");
        });

      
        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
