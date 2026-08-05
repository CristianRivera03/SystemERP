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
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.IdCountryNavigation != null ? src.IdCountryNavigation.CountryName : null));

        CreateMap<UserDTO, User>()
            .ForMember(dest => dest.IdRoleNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdCountryNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.ActionLogs, opt => opt.Ignore());

        // Session
        CreateMap<User, SessionDTO>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.IdRoleNavigation != null ? src.IdRoleNavigation.RoleName : null))
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.IdCountryNavigation != null ? src.IdCountryNavigation.CountryName : null));

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

        // ProductType
        CreateMap<ProductType, ProductTypeDTO>().ReverseMap();

        // UnitMeasure
        CreateMap<UnitMeasure, UnitMeasureDTO>().ReverseMap();

        // Presentation
        CreateMap<Presentation, PresentationDTO>().ReverseMap();

        // Product
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.IdCategoryNavigation != null ? src.IdCategoryNavigation.Name : null))
            .ForMember(dest => dest.ProductTypeDescription, opt => opt.MapFrom(src => src.IdProductTypeNavigation != null ? src.IdProductTypeNavigation.Description : null))
            .ForMember(dest => dest.UnitMeasureDescription, opt => opt.MapFrom(src => src.IdUnitMeasureNavigation != null ? src.IdUnitMeasureNavigation.Description : null));

        CreateMap<ProductDTO, Product>()
            .ForMember(dest => dest.IdCategoryNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdProductTypeNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdUnitMeasureNavigation, opt => opt.Ignore());

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

        // Supplier
        CreateMap<Supplier, SupplierDTO>()
            .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District != null ? src.District.Name : null))
            .ForMember(dest => dest.MunicipalityName, opt => opt.MapFrom(src => (src.District != null && src.District.Municipality != null) ? src.District.Municipality.Name : null))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => (src.District != null && src.District.Municipality != null && src.District.Municipality.Department != null) ? src.District.Municipality.Department.Name : null));

        CreateMap<SupplierDTO, Supplier>()
            .ForMember(dest => dest.District, opt => opt.Ignore());
        #endregion

        #region Generic Catalogs
        CreateMap<Category, CatalogDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdCategory))
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
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Description));

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
