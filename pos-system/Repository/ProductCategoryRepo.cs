using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.DTOs;
using pos_system.Models;

namespace pos_system.Repository
{
    public class ProductCategoryRepo(AppDbContext context, IMapper mapper, ICrudRepo<TblProductCategory> repo) : IProductCategoryRepo
    {
        readonly AppDbContext _context = context;
        readonly ICrudRepo<TblProductCategory> _repo = repo;
        readonly IMapper _mapper = mapper;

        public ICrudRepo<TblProductCategory> GetRepo()
        {
            return _repo;
        }

        public async Task<List<ProductCategoryDTO>> ProductCategoriesDTO()
        {
            return await _context.TblProductCategory.ProjectTo<ProductCategoryDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
