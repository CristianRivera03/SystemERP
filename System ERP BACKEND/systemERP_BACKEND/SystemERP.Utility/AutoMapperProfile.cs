using AutoMapper;
using System.Net;
using SystemERP.DTO;
using SystemERP.DTO.Catalog;
using SystemERP.DTO.Entities;
using SystemERP.DTO.Location;
using SystemERP.DTO.Products;
using SystemERP.DTO.Security;
using SystemERP.Model;

namespace SystemERP.Utility;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Security
        // Role
        CreateMap<Role, RoleDTO>().ReverseMap();

        // User
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.IdRoleNavigation != null ? src.IdRoleNavigation.RoleName : null))
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.IdCountryNavigation != null ? src.IdCountryNavigation.CountryName : null))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.IdBranchNavigation != null ? src.IdBranchNavigation.Name : null));

        CreateMap<UserDTO, User>()
            .ForMember(dest => dest.IdRoleNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdCountryNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdBranchNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.ActionLogs, opt => opt.Ignore());

        // Session
        CreateMap<User, SessionDTO>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.IdRoleNavigation != null ? src.IdRoleNavigation.RoleName : null))
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.IdCountryNavigation != null ? src.IdCountryNavigation.CountryName : null))
            .ForMember(dest => dest.Modules, opt => opt.MapFrom(src => (src.IdRoleNavigation != null && src.IdRoleNavigation.IdModules != null) 
                ? src.IdRoleNavigation.IdModules.Where(m => m.IsActive == null || m.IsActive == true) 
                : new List<Module>()));

        // Module
        CreateMap<Module, ModuleDTO>().ReverseMap();

        // ActionLog
        CreateMap<ActionLog, ActionLogDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.IdUserNavigation != null ? $"{src.IdUserNavigation.FirstName} {src.IdUserNavigation.LastName}" : null))
            .ForMember(dest => dest.SourceIp, opt => opt.MapFrom(src => src.SourceIp != null ? src.SourceIp.ToString() : null));

        CreateMap<ActionLogDTO, ActionLog>()
            .ForMember(dest => dest.SourceIp, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.SourceIp) ? IPAddress.Parse(src.SourceIp) : null))
            .ForMember(dest => dest.IdUserNavigation, opt => opt.Ignore());
        #endregion

        #region Location
        // Country
        CreateMap<Country, CountryDTO>().ReverseMap();

        // Department
        CreateMap<Department, DepartmentDTO>().ReverseMap();

        // Municipality
        CreateMap<Municipality, MunicipalityDTO>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null));

        CreateMap<MunicipalityDTO, Municipality>()
            .ForMember(dest => dest.Department, opt => opt.Ignore());

        // District
        CreateMap<District, DistrictDTO>()
            .ForMember(dest => dest.MunicipalityName, opt => opt.MapFrom(src => src.Municipality != null ? src.Municipality.Name : null));

        CreateMap<DistrictDTO, District>()
            .ForMember(dest => dest.Municipality, opt => opt.Ignore());
        #endregion

        #region Products
        // Category
        CreateMap<Category, CategoryDTO>().ReverseMap();

        // SubCategory
        CreateMap<SubCategory, SubCategoryDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.IdCategoryNavigation != null ? src.IdCategoryNavigation.Name : null));

        CreateMap<SubCategoryDTO, SubCategory>()
            .ForMember(dest => dest.IdCategoryNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore());

        // ProductType
        CreateMap<ProductType, ProductTypeDTO>().ReverseMap();

        // UnitMeasure
        CreateMap<UnitMeasure, UnitMeasureDTO>().ReverseMap();

        // Presentation
        CreateMap<Presentation, PresentationDTO>().ReverseMap();

        // Product
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.IdCategoryNavigation != null ? src.IdCategoryNavigation.Name : null))
            .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.IdSubCategoryNavigation != null ? src.IdSubCategoryNavigation.Name : null))
            .ForMember(dest => dest.ProductTypeDescription, opt => opt.MapFrom(src => src.IdProductTypeNavigation != null ? src.IdProductTypeNavigation.Description : null))
            .ForMember(dest => dest.UnitMeasureDescription, opt => opt.MapFrom(src => src.IdUnitMeasureNavigation != null ? src.IdUnitMeasureNavigation.Description : null))
            .ForMember(dest => dest.PurchaseUnitName, opt => opt.MapFrom(src => src.PurchaseUnit != null ? (src.PurchaseUnit.Name ?? src.PurchaseUnit.Description) : null))
            .ForMember(dest => dest.SaleUnitName, opt => opt.MapFrom(src => src.SaleUnit != null ? (src.SaleUnit.Name ?? src.SaleUnit.Description) : null));

        CreateMap<ProductDTO, Product>()
            .ForMember(dest => dest.IdCategoryNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdSubCategoryNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdProductTypeNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdUnitMeasureNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.PurchaseUnit, opt => opt.Ignore())
            .ForMember(dest => dest.SaleUnit, opt => opt.Ignore())
            .ForMember(dest => dest.InventoryStocks, opt => opt.Ignore())
            .ForMember(dest => dest.ProductPresentations, opt => opt.Ignore());

        // ProductPresentation
        CreateMap<ProductPresentation, ProductPresentationDTO>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.IdProductNavigation != null ? src.IdProductNavigation.Name : null))
            .ForMember(dest => dest.PresentationName, opt => opt.MapFrom(src => src.IdPresentationNavigation != null ? src.IdPresentationNavigation.Name : null));

        CreateMap<ProductPresentationDTO, ProductPresentation>()
            .ForMember(dest => dest.IdProductNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdPresentationNavigation, opt => opt.Ignore());
        #endregion

        #region Entities
        // Customer
        CreateMap<Customer, CustomerDTO>()
            .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District != null ? src.District.Name : null))
            .ForMember(dest => dest.MunicipalityName, opt => opt.MapFrom(src => (src.District != null && src.District.Municipality != null) ? src.District.Municipality.Name : null))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => (src.District != null && src.District.Municipality != null && src.District.Municipality.Department != null) ? src.District.Municipality.Department.Name : null));

        CreateMap<CustomerDTO, Customer>()
            .ForMember(dest => dest.District, opt => opt.Ignore());

        // SupplierContact
        CreateMap<SupplierContact, SupplierContactDTO>().ReverseMap();

        // Supplier
        CreateMap<Supplier, SupplierDTO>()
            .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District != null ? src.District.Name : null))
            .ForMember(dest => dest.MunicipalityName, opt => opt.MapFrom(src => (src.District != null && src.District.Municipality != null) ? src.District.Municipality.Name : null))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => (src.District != null && src.District.Municipality != null && src.District.Municipality.Department != null) ? src.District.Municipality.Department.Name : null))
            .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.SupplierContacts));

        CreateMap<SupplierDTO, Supplier>()
            .ForMember(dest => dest.District, opt => opt.Ignore())
            .ForMember(dest => dest.SupplierContacts, opt => opt.Ignore());

        // Company
        CreateMap<Company, SystemERP.DTO.Entities.CompanyDTO>()
            .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District != null ? src.District.Name : null));

        CreateMap<SystemERP.DTO.Entities.CompanyDTO, Company>()
            .ForMember(dest => dest.District, opt => opt.Ignore())
            .ForMember(dest => dest.Branches, opt => opt.Ignore());

        // Branch
        CreateMap<Branch, SystemERP.DTO.Entities.BranchDTO>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.IdCompanyNavigation != null ? src.IdCompanyNavigation.BusinessName : null))
            .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District != null ? src.District.Name : null))
            .ForMember(dest => dest.MunicipalityId, opt => opt.MapFrom(src => src.District != null ? src.District.MunicipalityId : null))
            .ForMember(dest => dest.MunicipalityName, opt => opt.MapFrom(src => (src.District != null && src.District.Municipality != null) ? src.District.Municipality.Name : null))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => (src.District != null && src.District.Municipality != null) ? src.District.Municipality.DepartmentId : null))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => (src.District != null && src.District.Municipality != null && src.District.Municipality.Department != null) ? src.District.Municipality.Department.Name : null));

        CreateMap<SystemERP.DTO.Entities.BranchDTO, Branch>()
            .ForMember(dest => dest.IdCompanyNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.District, opt => opt.Ignore())
            .ForMember(dest => dest.Users, opt => opt.Ignore())
            .ForMember(dest => dest.Warehouses, opt => opt.Ignore());

        // Warehouse & Location
        CreateMap<WarehouseCategory, SystemERP.DTO.Inventory.WarehouseCategoryDTO>().ReverseMap();

        CreateMap<Warehouse, SystemERP.DTO.Inventory.WarehouseDTO>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.IdBranchNavigation != null ? src.IdBranchNavigation.Name : null))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.IdWarehouseCategoryNavigation != null ? src.IdWarehouseCategoryNavigation.Name : null));

        CreateMap<SystemERP.DTO.Inventory.WarehouseDTO, Warehouse>()
            .ForMember(dest => dest.IdBranchNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdWarehouseCategoryNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.Locations, opt => opt.Ignore());

        CreateMap<Location, SystemERP.DTO.Inventory.LocationDTO>()
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.IdWarehouseNavigation != null ? src.IdWarehouseNavigation.Name : null));

        CreateMap<SystemERP.DTO.Inventory.LocationDTO, Location>()
            .ForMember(dest => dest.IdWarehouseNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.InventoryStocks, opt => opt.Ignore());

        CreateMap<InventoryStock, SystemERP.DTO.Inventory.InventoryStockDTO>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.IdProductNavigation != null ? src.IdProductNavigation.Name : null))
            .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.IdProductNavigation != null ? src.IdProductNavigation.InternalCode : null))
            .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.IdLocationNavigation != null ? src.IdLocationNavigation.Code : null))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => (src.IdLocationNavigation != null && src.IdLocationNavigation.IdWarehouseNavigation != null) ? src.IdLocationNavigation.IdWarehouseNavigation.Name : null));

        CreateMap<SystemERP.DTO.Inventory.InventoryStockDTO, InventoryStock>()
            .ForMember(dest => dest.IdProductNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdLocationNavigation, opt => opt.Ignore());
        #endregion

        #region Generic Catalogs
        CreateMap<Category, CatalogDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdCategory))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<SubCategory, CatalogDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdSubCategory))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<Role, CatalogDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdRole))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName));

        CreateMap<Country, CatalogDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdCountry))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CountryName));

        CreateMap<ProductType, CatalogDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProductType))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Description));

        CreateMap<UnitMeasure, CatalogDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdUnitMeasure))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name ?? src.Description));

        CreateMap<Presentation, CatalogDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdPresentation))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<Department, CatalogDTO<string>>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdDepartment))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<Municipality, CatalogDTO<string>>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdMunicipality))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<District, CatalogDTO<string>>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdDistrict))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        #endregion
    }
}
