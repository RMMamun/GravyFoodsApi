namespace GravyFoodsApi.Mappings
{
    using AutoMapper;
    using GravyFoodsApi.Models;        // Your entity namespace
    using GravyFoodsApi.DTOs;          // Your DTO namespace
    using GravyFoodsApi.Models.DTOs;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Entity → DTO
            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault() != null
                                                                      ? src.Images.FirstOrDefault().ImageUrl
                                                                      : null))
            .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.Unit != null ? src.Unit.UnitId : null))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : null))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.CompanyName : null));


            // DTO → Entity
            CreateMap<ProductDto, Product>();


            //If property names differ between source and destination, use ForMember to map them explicitly
            //CreateMap<Product, ProductDto>()
            //            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand!.Name));



            // Entity → DTO
            CreateMap<ProductStock, ProductStockDto>();
            // DTO → Entity
            CreateMap<ProductStockDto, ProductStock>();

        }
    }

}
