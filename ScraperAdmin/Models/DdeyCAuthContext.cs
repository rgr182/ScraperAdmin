using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ScraperAdmin.Models;

public partial class DdeyCAuthContext : DbContext
{
    public DdeyCAuthContext()
    {
    }

    public DdeyCAuthContext(DbContextOptions<DdeyCAuthContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<PasswordRecoveryRequest> PasswordRecoveryRequests { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserThread> UserThreads { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=158.222.102.253,49170;Database=DDEyC_Auth;User Id=Prueba;Password=sa;TrustServerCertificate=true;MultipleActiveResultSets=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC072FF7FE7D");

            entity.HasIndex(e => new { e.UserThreadId, e.Timestamp }, "IX_Messages_UserThreadId_Timestamp");

            entity.Property(e => e.Role).HasMaxLength(50);

            entity.HasOne(d => d.UserThread).WithMany(p => p.Messages)
                .HasForeignKey(d => d.UserThreadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_UserThreads");
        });

        modelBuilder.Entity<PasswordRecoveryRequest>(entity =>
        {
            entity.HasKey(e => e.PasswordRecoveryRequestId).HasName("PK__Password__18746D86335F1B62");

            entity.ToTable("PasswordRecoveryRequest");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.ExpirationTime).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.PasswordRecoveryRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_PasswordRecoveryRequest_Users");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Sessions__C9F49290EEB1077D");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Session");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CDD85074E");

            entity.Property(e => e.Gender).HasMaxLength(8);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<UserThread>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserThre__3214EC070E4BB776");

            entity.HasIndex(e => new { e.UserId, e.IsActive }, "IX_UserThreads_UserId_IsActive");

            entity.Property(e => e.ThreadId).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.UserThreads)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserThreads_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
