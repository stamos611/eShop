﻿using eShop.DAL;
using eShop.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace eShop.Controllers
{
    [AllowAnonymous]   
    public class HomeController : Controller
    {
        DbeShopEntities ctx = new DbeShopEntities();
        public ActionResult Index(string search,int? page)
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            return View(model.CreateModel(search,4,page));
        }
        public ActionResult GetProduct(int productId=0)
        {
            var product = ctx.Tbl_Product.Find(productId);
            return View(product);
        }
        public ActionResult Checkout()
        {
            return View();
        }
        public ActionResult CheckoutDetails()
        {
            return View();
        }
        public ActionResult DecreaseQty(int productId)
        {
            if (Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];              
                var product = ctx.Tbl_Product.Find(productId);
                foreach (var item in cart)
                {
                    if (item.Product.ProductId == productId)
                    {
                        int prevQty = item.Quantity;
                        if (prevQty > 0)
                        {
                            cart.Remove(item);
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = prevQty - 1
                            });
                        }
                        break;
                        
                    }
                }
                Session["cart"] = cart;
            }
            return Redirect("Checkout");
        }
        public ActionResult IncreaseQty(int productId)
        {
            if (Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var product = ctx.Tbl_Product.Find(productId);
                foreach (var item in cart)
                {
                    if (item.Product.ProductId == productId)
                    {
                        int prevQty = item.Quantity;
                        if (prevQty >= 0)
                        {
                            cart.Remove(item);
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = prevQty + 1
                            });
                        }
                        break;

                    }
                }
                Session["cart"] = cart;
            }
            return Redirect("Checkout");
        }
        public ActionResult AddToCart(int productId)
        {

            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                var product = ctx.Tbl_Product.Find(productId);
                cart.Add(new Item()
                {
                    Product = product,
                    Quantity = 1
                });
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var count = cart.Count();
                var product = ctx.Tbl_Product.Find(productId);
                for(int i = 0; i < count; i++)
                {
                    if (cart[i].Product.ProductId == productId)
                    {
                        int prevQty = cart[i].Quantity;
                        cart.Remove(cart[i]);
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = prevQty + 1
                        });
                        break;
                    }
                    else
                    {
                        var prd = cart.Where(x => x.Product.ProductId == productId).SingleOrDefault();
                        if (prd == null)
                        {
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = 1
                            });
                        }
                    }
                }
                Session["cart"] = cart;
            }
            return Redirect("Index");
        }
        public ActionResult RemoveFromCart(int productId)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            foreach(var item in cart)
            {
                if (item.Product.ProductId == productId)
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["cart"] = cart;
            return Redirect("Index");
        }

        public ActionResult NavBar()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return PartialView("_LoggedInNavbar");
            }
            return PartialView("_Navbar");
        }
    }
}