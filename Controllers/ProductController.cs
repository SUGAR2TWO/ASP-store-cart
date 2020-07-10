using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Bruhniki.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        public static Dictionary<string, Product> Products = new Dictionary<string, Product>();//"БД" продуктов        
        public static List<Product> Cart { get; set; } = new List<Product>();//"БД" корзины
        public static Dictionary<string, List<Product>> Orders = new Dictionary<string, List<Product>>();//"БД" заказов
        static ProductController()//Инициализация продуктов
        {
            for (int i = 0; i < 10; i++)
            {
                Products.Add(i.ToString(), new Product(name: "Product" + Convert.ToString(i), price: i));
            }
        }

        [Produces("application/json")]//Получение модели продуктов
        public IDictionary Index() => Products;

        [HttpPost("CartAdd")]
        [Consumes("application/json")]
        public IActionResult CartAdd([FromBody] string _id)//Добавление продуктов в корзину
        {
            if (Products.ContainsKey(_id))
            {
                Cart.Add(Products[_id]);
                return Ok("Product added");
            }
            else
                return BadRequest("No such products found in the list of products");
        }

        [HttpPost("CartRemove")]
        [Consumes("application/json")]
        public IActionResult CartRemove([FromBody] string _id)//Удаление продуктов из корзины
        {
            if (Cart.Contains(Products[_id]))
            {
                Cart.Remove(Products[_id]);
                return Ok("Product removed from cart");
            }
            else
                return BadRequest("No such products found in the cart");
        }

        [HttpGet("CartView")]
        [Produces("application/json")]
        public IList CartView() => Cart;//Посмотреть корзину


        [HttpPost("OrdersAdd")]
        [Produces("application/json")]
        public IActionResult OrderAdd()//Добавление заказа
        {
            if (Cart.Count > 0)
            {
                Orders.Add((Orders.Count + 1).ToString(), Cart);
                Cart = new List<Product>();
                return Ok("Product added");
            }
            else
                return BadRequest("Cart is empty");
        }

        [HttpPost("OrdersRemove")]
        [Consumes("application/json")]
        public IActionResult OrderAdd([FromBody] string _id)//Удаление заказа
        {
            if (Orders.ContainsKey((_id)))
            {
                Orders.Remove(_id);
                return Ok("Order removed");
            }
            else
                return BadRequest("No such orders found");
        }

        [HttpGet("OrdersView")]
        [Produces("application/json")]
        public IDictionary OrdersView() => Orders;//Посмотреть заказы
    }
}