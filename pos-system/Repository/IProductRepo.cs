﻿using pos_system.DTOs;
using pos_system.Models;

namespace pos_system.Repository
{
    public interface IProductRepo
    {
        ICrudRepo<TblProduct> GetRepo();
        Task<List<ProductDTO>> ProductDetailsDTO();
        Task<TblProduct> ProductDetailByID(string id);
        Task EditProduct(ProductFormModel data, string username);
        Task<ProductFormModel> EditData(string id);
    }
}
