using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.DTOs;
using pos_system.Models;

namespace pos_system.Repository
{
    public class ProductCategoryRepo : IProductCategoryRepo
    {
        readonly AppDbContext _context;
        readonly ICrudRepo<TblProductCategory> _repo;
        readonly IMapper _mapper;
        public ProductCategoryRepo(AppDbContext context, IMapper mapper, ICrudRepo<TblProductCategory> repo)
        {
            _context = context;
            _mapper = mapper;
            _repo = repo;
        }

        public ICrudRepo<TblProductCategory> GetRepo()
        {
            return _repo;
        }

        public async Task<List<ProductCategoryDTO>> GetProductCategoriesDTO()
        {
            return await _context.TblProductCategory.ProjectTo<ProductCategoryDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
