﻿using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
       private IOrderRepository? _orderRepository;
        private Cart? _cart;
        public OrderController(IOrderRepository repository, Cart cart)
        {
                _orderRepository = repository;
            _cart = cart;
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (_cart.Lines.Count() == 0)
                ModelState.AddModelError("", "Sorry, your cart is empty!");

            if (ModelState.IsValid)
            {
                order.Lines = _cart.Lines.ToArray();
                _orderRepository.SaveOrder(order);
                _cart.Clear();
                return RedirectToPage("/Completed", new { orderId = order.OrderId });
            }
            else
                return View();
        }
    }
}