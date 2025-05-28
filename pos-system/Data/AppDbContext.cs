using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pos_system.Models;

namespace pos_system.Data;

public partial class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblProduct> TblProduct { get; set; }
    public virtual DbSet<TblProductCategory> TblProductCategory { get; set; }
    public virtual DbSet<TblProductVariant> TblProductVariant { get; set; }
    public virtual DbSet<TblVariantGroup> TblVariantGroup { get; set; }
    public virtual DbSet<TblVariantOption> TblVariantOption { get; set; }
    public virtual DbSet<TblProductAddon> TblProductAddon { get; set; }
    public virtual DbSet<TblOrder> TblOrder { get; set; }
    public virtual DbSet<TblOrderItem> TblOrderItem { get; set; }
    public virtual DbSet<TblOrderItemAddon> TblOrderItemAddon { get; set; }
    public virtual DbSet<TblOrderNumberTracker> TblOrderNumberTracker { get; set; }

    public async Task<int> SaveChangesAsync(string username, CancellationToken cancellationToken = default)
    {
        List<string> skipField = ["createat", "updateat"];
        var changedEntities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
            .ToList();

        foreach (var entity in changedEntities)
        {
            string entityName = entity.Metadata.ClrType.Name;

            var primaryKey = entity.Metadata.FindPrimaryKey();
            var logKeyName = new List<string>();
            var logKeyValue = new List<string>();
            if (primaryKey != null)
            {
                foreach (var keyProp in primaryKey.Properties)
                {
                    string keyName = keyProp.Name;
                    var keyValue = entity.Property(keyName).CurrentValue;

                    logKeyName.Add(keyName);
                    logKeyValue.Add(keyValue?.ToString() ?? "NULL");
                }
            }

            if (entity.State == EntityState.Added)
            {
                await this.Database.ExecuteSqlRawAsync("INSERT INTO tblLogAudit (LogAction, LogEntityName, LogKeyName, LogKeyValue, LogUsername, LogDateTime) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                    "Added", entityName, string.Join("?", logKeyName), string.Join("?", logKeyValue), username, DateTime.Now);
            }

            if (entity.State == EntityState.Modified)
            {
                var modifiedFields = entity.Properties.Where(x => x.IsModified);

                foreach (var field in modifiedFields)
                {
                    string fieldName = field.Metadata.Name;
                    if (skipField.Contains(fieldName.ToLower()))
                        continue;
                    var currentValue = field.OriginalValue;
                    var modifiedValue = field.CurrentValue;

                    await this.Database.ExecuteSqlRawAsync("INSERT INTO tblLogAudit (LogAction, LogEntityName, LogKeyName, LogKeyValue, LogUsername, LogFldName, LogFldOldValue, LogFldNewValue, LogDateTime) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})",
                    "Modified", entityName, string.Join("?", logKeyName), string.Join("?", logKeyValue), username, fieldName, currentValue, modifiedValue, DateTime.Now);
                }
            }

            if (entity.State == EntityState.Deleted)
            {
                await this.Database.ExecuteSqlRawAsync("INSERT INTO tblLogAudit (LogAction, LogEntityName, LogKeyName, LogKeyValue, LogUsername, LogDateTime) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                    "Deleted", entityName, string.Join("?", logKeyName), string.Join("?", logKeyValue), username, DateTime.Now);
            }

            if (entity.State == EntityState.Added)
            {
                var createAtProp = entity.Properties.FirstOrDefault(p => p.Metadata.Name.Equals("createat", StringComparison.OrdinalIgnoreCase));

                if (createAtProp is not null)
                {
                    createAtProp.CurrentValue = DateTime.Now;
                }
            }
            else if ((entity.State == EntityState.Modified && entity.Properties.Where(x => x.IsModified && !skipField.Contains(x.Metadata.Name.ToLower())).Any()))
            {
                var updateAtProp = entity.Properties.FirstOrDefault(p => p.Metadata.Name.Equals("updateat", StringComparison.OrdinalIgnoreCase));

                if (updateAtProp is not null)
                {
                    updateAtProp.CurrentValue = DateTime.Now;
                }
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__tblProdu__B40CC6EDB8236FAB");

            entity.ToTable("tblProduct");

            entity.Property(e => e.ProductId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ProductID");
            entity.Property(e => e.CategoryId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("CategoryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProductImage)
                .IsUnicode(false);
            entity.Property(e => e.ImageType)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.TblProducts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblProduc__Categ__73BA3083");
        });

        modelBuilder.Entity<TblProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__tblProdu__19093A2BF9827246");

            entity.ToTable("tblProductCategory");

            entity.Property(e => e.CategoryId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblProductVariant>(entity =>
        {
            entity.HasKey(e => e.VariantId).HasName("PK__tblProdu__0EA233E4A7B8783B");

            entity.ToTable("tblProductVariant");

            entity.Property(e => e.VariantId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("VariantID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ProductID");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SKU");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.TblProductVariants)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblProduc__Produ__7E37BEF6");
        });

        modelBuilder.Entity<TblVariantGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__tblVaria__149AF30A45778920");

            entity.ToTable("tblVariantGroup");

            entity.Property(e => e.GroupId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("GroupID");
            entity.Property(e => e.ProductId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ProductID");
            entity.Property(e => e.VariantName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.TblVariantGroups)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblVarian__Produ__76969D2E");
        });

        modelBuilder.Entity<TblVariantOption>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__tblVaria__92C7A1DFD8B9C901");

            entity.ToTable("tblVariantOption");

            entity.Property(e => e.OptionId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("OptionID");
            entity.Property(e => e.GroupId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("GroupID");
            entity.Property(e => e.Value)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Group).WithMany(p => p.TblVariantOptions)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblVarian__Group__797309D9");
        });

        modelBuilder.Entity<TblProductAddon>(entity =>
        {
            entity.HasKey(e => e.AddonId).HasName("PK__tblProdu__74289513EF806551");

            entity.ToTable("tblProductAddon");

            entity.Property(e => e.AddonId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("AddonID");
            entity.Property(e => e.AddonName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.ProductId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ProductID");
            entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("(getdate())").HasColumnType("datetime");
        });

        modelBuilder.Entity<TblOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__tblOrder__C3905BAF81A836DE");

            entity.ToTable("tblOrder");

            entity.Property(e => e.OrderId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("OrderID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(24, 6)");
        });

        modelBuilder.Entity<TblOrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__tblOrder__57ED06A1BDB0209E");

            entity.ToTable("tblOrderItem");

            entity.Property(e => e.OrderItemId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("OrderItemID");
            entity.Property(e => e.OrderId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("OrderID");
            entity.Property(e => e.ProductId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ProductID");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(24, 6)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(24, 6)");
            entity.Property(e => e.VariantId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("VariantID");

            entity.HasOne(d => d.Order).WithMany(p => p.TblOrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Order");
        });

        modelBuilder.Entity<TblOrderItemAddon>(entity =>
        {
            entity.HasKey(e => e.ItemAddonId).HasName("PK__tblOrder__41D7485D2833476D");

            entity.ToTable("tblOrderItemAddon");

            entity.Property(e => e.ItemAddonId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ItemAddonID");
            entity.Property(e => e.AddonId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("AddonID");
            entity.Property(e => e.AddonPrice).HasColumnType("decimal(24, 6)");
            entity.Property(e => e.OrderItemId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("OrderItemID");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(24, 6)");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.TblOrderItemAddons)
                .HasForeignKey(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemAddon_OrderItem");
        });

        modelBuilder.Entity<TblOrderNumberTracker>(entity =>
        {
            entity.HasKey(e => e.DateID);

            entity.ToTable("tblOrderNumberTracker");

            entity.Property(e => e.DateID).HasColumnName("DateID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
