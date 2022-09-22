using eShop.DAL;
using eShop.Models;
using eShop.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace eShop.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        public GenericUnit _unit = new GenericUnit();

        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var Cat = _unit.GetRepositoryInstance<Tbl_Category>().GetAllRecords();
            foreach(var item in Cat)
            {
                list.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text = item.CategoryName });
            }
            return list;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Categories()
        {
            List<Tbl_Category> allcategories = _unit.GetRepositoryInstance<Tbl_Category>().GetAllRecordsIQuerable().Where(i => i.IsDelete == false).ToList();
            return View(allcategories);
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        public ActionResult UpdateCategory(int categoryId=0)
        {
            CategoryDetail cd;
                if (categoryId !=0)
                {
                    cd = JsonConvert.DeserializeObject<CategoryDetail>(JsonConvert.SerializeObject(_unit.GetRepositoryInstance<Tbl_Category>()
                        .GetFirstorDefault(categoryId)));
                }
                else
                {
                    cd = new CategoryDetail();
                }
            return View("UpdateCategory",cd);
        }

        public ActionResult CategoryEdit(int categoryId)
        {
            return View(_unit.GetRepositoryInstance<Tbl_Category>().GetFirstorDefault(categoryId));
        }

        [HttpPost]
        public ActionResult CategoryEdit(Tbl_Category tbl)
        {
            
            _unit.GetRepositoryInstance<Tbl_Category>().Update(tbl);
            return RedirectToAction("Categories");
        }

        public ActionResult Product()
        {
            return View(_unit.GetRepositoryInstance<Tbl_Product>().GetProduct());
        }

        public ActionResult ProductEdit(int productId)
        {
            ViewBag.CateoryList = GetCategory();
            return View(_unit.GetRepositoryInstance<Tbl_Product>().GetFirstorDefault(productId));
        }

        [HttpPost]
        public ActionResult ProductEdit(Tbl_Product tbl, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);
                // file is uploaded
                file.SaveAs(path);
            }
            tbl.ProductImage = file != null ? pic : tbl.ProductImage;
            tbl.ModifiedDate = DateTime.Now;
            _unit.GetRepositoryInstance<Tbl_Product>().Update(tbl);
            return RedirectToAction("Product");
        }

        public ActionResult ProductAdd()
        {
            ViewBag.CategoryList = GetCategory();
            return View();
        }
        
        [HttpPost]
        public ActionResult ProductAdd(Tbl_Product tbl,HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);
                // file is uploaded
                file.SaveAs(path);
            }
            tbl.ProductImage = pic;
            tbl.CreatedDate = DateTime.Now;
            _unit.GetRepositoryInstance<Tbl_Product>().Add(tbl);
            return RedirectToAction("Product");
        }
    }
}