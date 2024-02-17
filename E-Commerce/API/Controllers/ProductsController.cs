using API.DTOs;
using API.Helpers;
using API.ResponseModule;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        [HttpGet]
        [Cached(100)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery] ProductSpecsParams productParams)
        {
            var countSpecs = new ProductWithFiltersForCountSpecifications(productParams);

            var totalCount = await unitOfWork.Repository<Product>().CountAsync(countSpecs);

            productParams.PageIndex = MaxPageIndex.SetMaxPageIndex(productParams, totalCount);

            var spec = new ProductWithBrandAndCategorySpecifications(productParams);
            var products = await unitOfWork.Repository<Product>().GetAllWithSpecifications(spec);

            var mappedProducts = mapper.Map<IReadOnlyList<ProductDTO>>(products);
            var pagenatedProducts = new Pagination<ProductDTO>(productParams.PageIndex, productParams.PageSize, totalCount, mappedProducts);
            return Ok(pagenatedProducts);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int? id)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(id);
            var product = await unitOfWork.Repository<Product>().GetEntityWithSpecitfications(specs);

            if (product is null)
                return NotFound();

            var mappedProduct = mapper.Map<ProductDTO>(product);
            return Ok(mappedProduct);
        }

        [Cached(100)]
        [HttpGet("GetBrands")]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrands()
        {
            var brands = await unitOfWork.Repository<Brand>().GetAllAsync();
            return Ok(brands);
        }

        [Cached(100)]
        [HttpGet("GetCategories")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategories()
        {
            var categories = await unitOfWork.Repository<Category>().GetAllAsync();
            return Ok(categories);
        }


    }
}
