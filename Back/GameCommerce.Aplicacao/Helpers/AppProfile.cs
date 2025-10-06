using AutoMapper;
using GameCommerce.Aplicacao.Dtos;
using GameCommerce.Dominio;

namespace GameCommerce.Aplicacao.Helpers
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            // Configure seus mapeamentos aqui
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Cupom, CupomDto>().ReverseMap();
            CreateMap<Pedido, PedidoDto>().ReverseMap();
            CreateMap<ItemPedido, ItemPedidoDto>().ReverseMap();
            CreateMap<Produto, ProdutoDto>().ReverseMap();
            CreateMap<SiteInfo, SiteInfoDto>().ReverseMap();
            CreateMap<Subcategoria, SubcategoriaDto>().ReverseMap();
        }
    }
}
