using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Herramientas.Models;

public partial class PuerBancoBd1Context : DbContext
{
    public PuerBancoBd1Context()
    {
    }

    public PuerBancoBd1Context(DbContextOptions<PuerBancoBd1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountType> AccountTypes { get; set; }

    public virtual DbSet<ChangesHistory> ChangesHistories { get; set; }

    public virtual DbSet<CheckingAccount> CheckingAccounts { get; set; }

    public virtual DbSet<Movement> Movements { get; set; }

    public virtual DbSet<SavingAccount> SavingAccounts { get; set; }

    public virtual DbSet<TypeOfMovement> TypeOfMovements { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=JarolPc\\SQLEXPRESS;Database=PuerBancoBD1;Integrated Security=True;Persist Security Info=False;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountNumber).HasName("PK__Accounts__BE2ACD6EF77E579F");

            entity.Property(e => e.AccountNumber).ValueGeneratedNever();
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Type).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Accounts__TypeID__5535A963");

            entity.HasOne(d => d.User).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Accounts__UserID__5441852A");
        });

        modelBuilder.Entity<AccountType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__AccountT__516F039570A2C070");

            entity.ToTable("AccountType");

            entity.HasIndex(e => e.TypeName, "UQ__AccountT__D4E7DFA838721BDB").IsUnique();

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ChangesHistory>(entity =>
        {
            entity.HasKey(e => e.ChangesId).HasName("PK__ChangesH__A6F89D3FF1235120");

            entity.ToTable("ChangesHistory");

            entity.Property(e => e.ChangesId).HasColumnName("ChangesID");
            entity.Property(e => e.ChangeDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ChangeType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RecordId).HasColumnName("RecordID");
            entity.Property(e => e.TableAffected)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ChangesHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChangesHi__UserI__693CA210");
        });

        modelBuilder.Entity<CheckingAccount>(entity =>
        {
            entity.HasKey(e => e.AccountNumber).HasName("PK__Checking__BE2ACD6EFAEB92E1");

            entity.ToTable("Checking_Account");

            entity.Property(e => e.AccountNumber).ValueGeneratedNever();
            entity.Property(e => e.OverDraftLimit)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("OverDraft_Limit");

            entity.HasOne(d => d.AccountNumberNavigation).WithOne(p => p.CheckingAccount)
                .HasForeignKey<CheckingAccount>(d => d.AccountNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Checking___Accou__5CD6CB2B");
        });

        modelBuilder.Entity<Movement>(entity =>
        {
            entity.HasKey(e => e.IdTransaction).HasName("PK__Movement__45542F4508B475AD");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionTId).HasColumnName("TransactionT_ID");

            entity.HasOne(d => d.DestinyAccountNavigation).WithMany(p => p.MovementDestinyAccountNavigations)
                .HasForeignKey(d => d.DestinyAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Movements__Desti__6383C8BA");

            entity.HasOne(d => d.OriginAccountNavigation).WithMany(p => p.MovementOriginAccountNavigations)
                .HasForeignKey(d => d.OriginAccount)
                .HasConstraintName("FK__Movements__Origi__6477ECF3");

            entity.HasOne(d => d.TransactionT).WithMany(p => p.Movements)
                .HasForeignKey(d => d.TransactionTId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Movements__Trans__656C112C");
        });

        modelBuilder.Entity<SavingAccount>(entity =>
        {
            entity.HasKey(e => e.AccountNumber).HasName("PK__Saving_A__BE2ACD6E6514EAB9");

            entity.ToTable("Saving_Account");

            entity.Property(e => e.AccountNumber).ValueGeneratedNever();
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.AccountNumberNavigation).WithOne(p => p.SavingAccount)
                .HasForeignKey<SavingAccount>(d => d.AccountNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Saving_Ac__Accou__59063A47");
        });

        modelBuilder.Entity<TypeOfMovement>(entity =>
        {
            entity.HasKey(e => e.TransactionTId).HasName("PK__TypeOfMo__D093880EBA62D718");

            entity.ToTable("TypeOfMovement");

            entity.HasIndex(e => e.TypeTransId, "UQ__TypeOfMo__F8AA8C053E1EA6B3").IsUnique();

            entity.Property(e => e.TransactionTId).HasColumnName("TransactionT_ID");
            entity.Property(e => e.TypeTransId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TypeTransID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACB9F6E199");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534E3E49099").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash).HasMaxLength(64);
            entity.Property(e => e.UserType)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
