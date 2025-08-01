﻿using MailKit;
using Org.BouncyCastle.Utilities.Collections;
using pos_system.DTOs;
using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class OrderService(IOrderRepo orderRepo, IProductVariantRepo productVariantRepo, IProductRepo productRepo, IOrderNumberTrackerRepo orderNumberTrackerRepo, IProductService productService, IEmailService emailService, ISetupService setupService) : IOrderService
    {
        readonly IOrderRepo _orderRepo = orderRepo;
        readonly IProductVariantRepo _productVariantRepo = productVariantRepo;
        readonly IProductRepo _productRepo = productRepo;
        readonly IOrderNumberTrackerRepo _orderNumberTrackerRepo = orderNumberTrackerRepo;
        readonly IProductService _productService = productService;
        readonly IEmailService _emailService = emailService;
        readonly ISetupService _setupService = setupService;
        private string _username = "System";

        public void SetUsername(string username)
        {
            _username = username;
            _orderRepo.GetRepo().SetUsername(username);
            _productVariantRepo.GetRepo().SetUsername(username);
            _productRepo.GetRepo().SetUsername(username);
        }

        public async Task CreateOrder(TblOrder order)
        {
            try
            {
                order.OrderNumber = order.Type == "PreOrder" ? GeneratePreorderNumber(await _orderNumberTrackerRepo.GetOrderNumber()) : await _orderNumberTrackerRepo.GetOrderNumber();
                order.OrderDate = DateTime.Now;
                order.Status = "Success";
                order.TotalPrice = order.TblOrderItems.Sum(oi => oi.SubTotal);
                order.Cashier = _username;
                order.TblOrderItems = order.TblOrderItems.Select(oi =>
                {
                    oi.OrderId = order.OrderId;
                    oi.Notes = oi.Notes ?? "";
                    return oi;
                }).ToList();

                foreach (var item in order.TblOrderItems)
                {
                    var product = await _productRepo.GetRepo().GetById(item.ProductId).ConfigureAwait(false) ?? throw new ApplicationException("Product not found");
                    if (item.VariantId is null)
                    {
                        var productStock = product.ProductStock;
                        if (productStock < item.Quantity)
                            throw new ApplicationException($"Insufficient stock for product {product.ProductName}. Available: {productStock}, Requested: {item.Quantity}");
                        else if(item.Quantity <= 0)
                            throw new ApplicationException("Minimum purchase amount 1");
                    }else if(item.VariantId is not null)
                    {
                        var productVariant = await _productVariantRepo.GetRepo().GetById(item.VariantId).ConfigureAwait(false) ?? throw new ApplicationException("Product variant not found");
                        var variantStock = productVariant.VariantStock;
                        if (variantStock < item.Quantity)
                            throw new ApplicationException($"Insufficient stock for product variant {product.ProductName} ({productVariant.Sku}). Available: {variantStock}, Requested: {item.Quantity}");
                        else if (item.Quantity <= 0)
                            throw new ApplicationException("Minimum purchase amount 1");
                    }
                }

                await _orderRepo.GetRepo().Add(order);
                await ReduceProductStock(order).ConfigureAwait(false);
                await _orderNumberTrackerRepo.GenerateOrderNumber();

                if(order.Type == "PreOrder")
                {
                    var preOrderItemList = await _productService.GetOrderDescDTO(order).ConfigureAwait(false);
                    var setup = await _setupService.GetSetup().ConfigureAwait(false);
                    var data = new OrderDescDTO()
                    {
                        Username = _username,
                        TotalPrice = order.TotalPrice,
                        ScheduledAt = order.ScheduledAt,
                        items = preOrderItemList
                    };

                    await _emailService.SendPreOrderNotification(data, setup.Email).ConfigureAwait(false);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error creating order: " + ex.Message, ex);
            }
        }

        public string GeneratePreorderNumber(string sequenceNumber)
        {
            string prefix = "PO";
            string datePart = DateTime.Now.ToString("yyyyMM");
            string paddedSequence = sequenceNumber;

            return $"{prefix}-{datePart}-{paddedSequence}";
        }

        public async Task<List<TblOrder>> GetOrderHistory(string? fromDate, string? toDate)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;

                fromDate = new DateTime(year, month, DateTime.Now.Day).AddDays(-6).ToString("yyyy-MM-dd");
                toDate = new DateTime(year, month, DateTime.Now.Day).ToString("yyyy-MM-dd");
            }

            var history = await _orderRepo.GetOrdersByDate(fromDate, toDate).ConfigureAwait(false);
            history = history.Select(o => { o.TotalPrice = decimal.Round(o.TotalPrice, 0); return o; }).ToList();
            return history.OrderByDescending(o => o.OrderDate).ThenByDescending(o => o.OrderNumber).ToList();
        }

        private async Task ReduceProductStock(TblOrder order)
        {
            foreach (var item in order.TblOrderItems)
            {
                if (string.IsNullOrEmpty(item.VariantId))
                {
                    var product = await _productRepo.GetRepo().GetById(item.ProductId).ConfigureAwait(false);
                    product.ProductStock -= item.Quantity;
                    await _productRepo.GetRepo().Update(product).ConfigureAwait(false);
                }
                else
                {
                    var productVariant = await _productVariantRepo.GetRepo().GetById(item.VariantId).ConfigureAwait(false);
                    productVariant.VariantStock -= item.Quantity;
                    await _productVariantRepo.GetRepo().Update(productVariant).ConfigureAwait(false);
                }
            }
        }

        public async Task<PreOrderViewModel> GetPreOrder(string? fromDate, string? toDate, string status, string userId, string role)
        {
            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                var days = DateTime.DaysInMonth(year, month);

                fromDate = new DateTime(year, month, 1).ToString("yyyy-MM-dd");
                toDate = new DateTime(year, month, days).ToString("yyyy-MM-dd");
            }

            var data = await _orderRepo.GetPreOrder(fromDate, toDate, status, userId, role).ConfigureAwait(false);
            data = data.Select(o => { o.TotalPrice = decimal.Round(o.TotalPrice, 0); return o; }).ToList();

            return new PreOrderViewModel()
            {
                FromDate = fromDate,
                ToDate = toDate,
                Status = status,
                Orders = data,
            };
        }

        public async Task SendPreOrderFeedback(string orderId)
        {
            var order = await _orderRepo.GetRepo().GetById(orderId).ConfigureAwait(false);
            var setup = await _setupService.GetSetup().ConfigureAwait(false);
            await _emailService.SendPreOrderFeedbackNotification(order, setup.Email).ConfigureAwait(false);
        }

        public async Task UpdatePreOrderStatus(string orderId, string status, string? comment)
        {
            await _orderRepo.UpdatePreOrderStatus(orderId, status, comment).ConfigureAwait(false);
        }
    }
}
