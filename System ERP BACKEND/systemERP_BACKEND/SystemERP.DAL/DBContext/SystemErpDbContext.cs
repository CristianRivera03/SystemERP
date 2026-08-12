using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SystemERP.Model;

namespace SystemERP.DAL.DBContext;

public partial class SystemErpDbContext : DbContext
{
    public SystemErpDbContext()
    {
    }

    public SystemErpDbContext(DbContextOptions<SystemErpDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActionLog> ActionLogs { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<InventoryStock> InventoryStocks { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Municipality> Municipalities { get; set; }

    public virtual DbSet<Presentation> Presentations { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductPresentation> ProductPresentations { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierContact> SupplierContacts { get; set; }

    public virtual DbSet<UnitMeasure> UnitMeasures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<WarehouseCategory> WarehouseCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<ActionLog>(entity =>
        {
            entity.HasKey(e => e.IdLog).HasName("action_logs_pkey");

            entity.ToTable("action_logs");

            entity.Property(e => e.IdLog)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_log");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .HasColumnName("action");
            entity.Property(e => e.ActionDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("action_date");
            entity.Property(e => e.AffectedTable)
                .HasMaxLength(50)
                .HasColumnName("affected_table");
            entity.Property(e => e.Details)
                .HasColumnType("jsonb")
                .HasColumnName("details");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.RecordId)
                .HasMaxLength(100)
                .HasColumnName("record_id");
            entity.Property(e => e.SourceIp).HasColumnName("source_ip");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ActionLogs)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("action_logs_id_user_fkey");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.IdBranch).HasName("branches_pkey");

            entity.ToTable("branches");

            entity.Property(e => e.IdBranch)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_branch");
            entity.Property(e => e.AddressComplement)
                .HasMaxLength(200)
                .HasColumnName("address_complement");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DistrictId)
                .HasMaxLength(4)
                .HasColumnName("district_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdCompany).HasColumnName("id_company");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");

            entity.HasOne(d => d.District).WithMany(p => p.Branches)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("branches_district_id_fkey");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.Branches)
                .HasForeignKey(d => d.IdCompany)
                .HasConstraintName("branches_id_company_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.IdCompany).HasName("companies_pkey");

            entity.ToTable("companies");

            entity.Property(e => e.IdCompany)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_company");
            entity.Property(e => e.AddressComplement)
                .HasMaxLength(200)
                .HasColumnName("address_complement");
            entity.Property(e => e.BusinessName)
                .HasMaxLength(150)
                .HasColumnName("business_name");
            entity.Property(e => e.CommercialLine1)
                .HasMaxLength(150)
                .HasColumnName("commercial_line_1");
            entity.Property(e => e.CommercialLine2)
                .HasMaxLength(150)
                .HasColumnName("commercial_line_2");
            entity.Property(e => e.CommercialLine3)
                .HasMaxLength(150)
                .HasColumnName("commercial_line_3");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DistrictId)
                .HasMaxLength(4)
                .HasColumnName("district_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LogoUrl).HasColumnName("logo_url");
            entity.Property(e => e.Nrc)
                .HasMaxLength(20)
                .HasColumnName("nrc");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.TaxId)
                .HasMaxLength(20)
                .HasColumnName("tax_id");
            entity.Property(e => e.TradeName)
                .HasMaxLength(150)
                .HasColumnName("trade_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.District).WithMany(p => p.Companies)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("companies_district_id_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdCountry).HasName("countries_pkey");

            entity.ToTable("countries");

            entity.HasIndex(e => e.CountryName, "countries_country_name_key").IsUnique();

            entity.Property(e => e.IdCountry).HasColumnName("id_country");
            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .HasColumnName("country_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.IdCustomer).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.IdCustomer)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_customer");
            entity.Property(e => e.AddressComplement)
                .HasMaxLength(150)
                .HasColumnName("address_complement");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DistrictId)
                .HasMaxLength(4)
                .HasColumnName("district_id");
            entity.Property(e => e.DocumentId)
                .HasMaxLength(20)
                .HasColumnName("document_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.TaxId)
                .HasMaxLength(20)
                .HasColumnName("tax_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.District).WithMany(p => p.Customers)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("customers_district_id_fkey");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.IdDepartment).HasName("departments_pkey");

            entity.ToTable("departments");

            entity.Property(e => e.IdDepartment)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("id_department");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.IdDistrict).HasName("districts_pkey");

            entity.ToTable("districts");

            entity.Property(e => e.IdDistrict)
                .HasMaxLength(4)
                .HasColumnName("id_district");
            entity.Property(e => e.MunicipalityId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("municipality_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.Municipality).WithMany(p => p.Districts)
                .HasForeignKey(d => d.MunicipalityId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("districts_municipality_id_fkey");
        });

        modelBuilder.Entity<InventoryStock>(entity =>
        {
            entity.HasKey(e => e.IdStock).HasName("inventory_stocks_pkey");

            entity.ToTable("inventory_stocks");

            entity.HasIndex(e => new { e.IdProduct, e.IdLocation }, "unique_product_location").IsUnique();

            entity.Property(e => e.IdStock)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_stock");
            entity.Property(e => e.IdLocation).HasColumnName("id_location");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("last_updated");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasColumnName("quantity");

            entity.HasOne(d => d.IdLocationNavigation).WithMany(p => p.InventoryStocks)
                .HasForeignKey(d => d.IdLocation)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("inventory_stocks_id_location_fkey");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.InventoryStocks)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("inventory_stocks_id_product_fkey");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.IdLocation).HasName("locations_pkey");

            entity.ToTable("locations");

            entity.Property(e => e.IdLocation)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_location");
            entity.Property(e => e.Aisle)
                .HasMaxLength(20)
                .HasColumnName("aisle");
            entity.Property(e => e.Capacity)
                .HasDefaultValue(0)
                .HasColumnName("capacity");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.IdWarehouse).HasColumnName("id_warehouse");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Level)
                .HasMaxLength(20)
                .HasColumnName("level");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Position)
                .HasMaxLength(20)
                .HasColumnName("position");
            entity.Property(e => e.Rack)
                .HasMaxLength(20)
                .HasColumnName("rack");

            entity.HasOne(d => d.IdWarehouseNavigation).WithMany(p => p.Locations)
                .HasForeignKey(d => d.IdWarehouse)
                .HasConstraintName("locations_id_warehouse_fkey");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.IdModule).HasName("modules_pkey");

            entity.ToTable("modules");

            entity.Property(e => e.IdModule).HasColumnName("id_module");
            entity.Property(e => e.FrontendPath)
                .HasMaxLength(150)
                .HasColumnName("frontend_path");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .HasColumnName("icon");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Municipality>(entity =>
        {
            entity.HasKey(e => e.IdMunicipality).HasName("municipalities_pkey");

            entity.ToTable("municipalities");

            entity.Property(e => e.IdMunicipality)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("id_municipality");
            entity.Property(e => e.DepartmentId)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("department_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.Department).WithMany(p => p.Municipalities)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("municipalities_department_id_fkey");
        });

        modelBuilder.Entity<Presentation>(entity =>
        {
            entity.HasKey(e => e.IdPresentation).HasName("presentations_pkey");

            entity.ToTable("presentations");

            entity.Property(e => e.IdPresentation).HasColumnName("id_presentation");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.UnitQuantity)
                .HasDefaultValue(1)
                .HasColumnName("unit_quantity");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("products_pkey");

            entity.ToTable("products");

            entity.HasIndex(e => e.Barcode, "products_barcode_key").IsUnique();

            entity.HasIndex(e => e.InternalCode, "products_internal_code_key").IsUnique();

            entity.HasIndex(e => e.Sku, "products_sku_key").IsUnique();

            entity.Property(e => e.IdProduct)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_product");
            entity.Property(e => e.Barcode)
                .HasMaxLength(50)
                .HasColumnName("barcode");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Dimensions)
                .HasMaxLength(100)
                .HasColumnName("dimensions");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdProductType).HasColumnName("id_product_type");
            entity.Property(e => e.IdSubCategory).HasColumnName("id_sub_category");
            entity.Property(e => e.IdUnitMeasure).HasColumnName("id_unit_measure");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.InternalCode)
                .HasMaxLength(50)
                .HasColumnName("internal_code");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsTaxable)
                .HasDefaultValue(true)
                .HasColumnName("is_taxable");
            entity.Property(e => e.MinStock)
                .HasPrecision(10, 2)
                .HasDefaultValue(0m)
                .HasColumnName("min_stock");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.OriginalCode)
                .HasMaxLength(100)
                .HasColumnName("original_code");
            entity.Property(e => e.Presentation)
                .HasMaxLength(100)
                .HasColumnName("presentation");
            entity.Property(e => e.PurchaseUnitId).HasColumnName("purchase_unit_id");
            entity.Property(e => e.SaleUnitId).HasColumnName("sale_unit_id");
            entity.Property(e => e.Size)
                .HasMaxLength(50)
                .HasColumnName("size");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .HasColumnName("sku");
            entity.Property(e => e.TaxCode)
                .HasMaxLength(2)
                .HasDefaultValueSql("'20'::bpchar")
                .IsFixedLength()
                .HasColumnName("tax_code");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("products_id_category_fkey");

            entity.HasOne(d => d.IdProductTypeNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdProductType)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("products_id_product_type_fkey");

            entity.HasOne(d => d.IdSubCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdSubCategory)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("products_id_sub_category_fkey");

            entity.HasOne(d => d.IdUnitMeasureNavigation).WithMany(p => p.ProductIdUnitMeasureNavigations)
                .HasForeignKey(d => d.IdUnitMeasure)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("products_id_unit_measure_fkey");

            entity.HasOne(d => d.PurchaseUnit).WithMany(p => p.ProductPurchaseUnits)
                .HasForeignKey(d => d.PurchaseUnitId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("products_purchase_unit_id_fkey");

            entity.HasOne(d => d.SaleUnit).WithMany(p => p.ProductSaleUnits)
                .HasForeignKey(d => d.SaleUnitId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("products_sale_unit_id_fkey");
        });

        modelBuilder.Entity<ProductPresentation>(entity =>
        {
            entity.HasKey(e => e.IdProductPresentation).HasName("product_presentations_pkey");

            entity.ToTable("product_presentations");

            entity.Property(e => e.IdProductPresentation)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_product_presentation");
            entity.Property(e => e.IdPresentation).HasColumnName("id_presentation");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");

            entity.HasOne(d => d.IdPresentationNavigation).WithMany(p => p.ProductPresentations)
                .HasForeignKey(d => d.IdPresentation)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("product_presentations_id_presentation_fkey");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductPresentations)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("product_presentations_id_product_fkey");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.IdProductType).HasName("product_types_pkey");

            entity.ToTable("product_types");

            entity.Property(e => e.IdProductType).HasColumnName("id_product_type");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "roles_role_name_key").IsUnique();

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");

            entity.HasMany(d => d.IdModules).WithMany(p => p.IdRoles)
                .UsingEntity<Dictionary<string, object>>(
                    "RoleModule",
                    r => r.HasOne<Module>().WithMany()
                        .HasForeignKey("IdModule")
                        .HasConstraintName("role_modules_id_module_fkey"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("IdRole")
                        .HasConstraintName("role_modules_id_role_fkey"),
                    j =>
                    {
                        j.HasKey("IdRole", "IdModule").HasName("role_modules_pkey");
                        j.ToTable("role_modules");
                        j.IndexerProperty<int>("IdRole").HasColumnName("id_role");
                        j.IndexerProperty<int>("IdModule").HasColumnName("id_module");
                    });
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.IdSubCategory).HasName("sub_categories_pkey");

            entity.ToTable("sub_categories");

            entity.Property(e => e.IdSubCategory).HasColumnName("id_sub_category");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("sub_categories_id_category_fkey");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.IdSupplier).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.HasIndex(e => e.Code, "suppliers_code_key").IsUnique();

            entity.Property(e => e.IdSupplier)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_supplier");
            entity.Property(e => e.AddressComplement)
                .HasMaxLength(150)
                .HasColumnName("address_complement");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DistrictId)
                .HasMaxLength(4)
                .HasColumnName("district_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.TaxId)
                .HasMaxLength(20)
                .HasColumnName("tax_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .HasColumnName("website");

            entity.HasOne(d => d.District).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("suppliers_district_id_fkey");
        });

        modelBuilder.Entity<SupplierContact>(entity =>
        {
            entity.HasKey(e => e.IdSupplierContact).HasName("supplier_contacts_pkey");

            entity.ToTable("supplier_contacts");

            entity.Property(e => e.IdSupplierContact).HasColumnName("id_supplier_contact");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name");
            entity.Property(e => e.IdSupplier).HasColumnName("id_supplier");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");

            entity.HasOne(d => d.IdSupplierNavigation).WithMany(p => p.SupplierContacts)
                .HasForeignKey(d => d.IdSupplier)
                .HasConstraintName("supplier_contacts_id_supplier_fkey");
        });

        modelBuilder.Entity<UnitMeasure>(entity =>
        {
            entity.HasKey(e => e.IdUnitMeasure).HasName("unit_measures_pkey");

            entity.ToTable("unit_measures");

            entity.Property(e => e.IdUnitMeasure).HasColumnName("id_unit_measure");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.IdUser)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_user");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DocumentId)
                .HasMaxLength(20)
                .HasColumnName("document_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.IdBranch).HasColumnName("id_branch");
            entity.Property(e => e.IdCountry).HasColumnName("id_country");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdBranchNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdBranch)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_id_branch_fkey");

            entity.HasOne(d => d.IdCountryNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdCountry)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_id_country_fkey");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_id_role_fkey");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.IdWarehouse).HasName("warehouses_pkey");

            entity.ToTable("warehouses");

            entity.Property(e => e.IdWarehouse)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_warehouse");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IdBranch).HasColumnName("id_branch");
            entity.Property(e => e.IdWarehouseCategory).HasColumnName("id_warehouse_category");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.IdBranchNavigation).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.IdBranch)
                .HasConstraintName("warehouses_id_branch_fkey");

            entity.HasOne(d => d.IdWarehouseCategoryNavigation).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.IdWarehouseCategory)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("warehouses_id_warehouse_category_fkey");
        });

        modelBuilder.Entity<WarehouseCategory>(entity =>
        {
            entity.HasKey(e => e.IdWarehouseCategory).HasName("warehouse_categories_pkey");

            entity.ToTable("warehouse_categories");

            entity.Property(e => e.IdWarehouseCategory).HasColumnName("id_warehouse_category");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
