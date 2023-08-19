using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProductPageTaskMVC.DBModel;
using ProductPageTaskMVC.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProductPageTaskMVC.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SimpleMvcdbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, SimpleMvcdbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index() //this is importsnt function
        {
            var listOfProducts = _dbContext.Products.ToList();
            return View(listOfProducts);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            TempData["Message"] = "Product Inserted Successfully";

            return RedirectToAction("Index"); // Redirect to the Create view
        }


        [HttpGet]
        public ActionResult Edit(long Id)
        {
            var product = _dbContext.Products.Where(x=> x.Id == Id).FirstOrDefault();
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product data)
        {
            var product = _dbContext.Products.Where(x => x.Id == data.Id).FirstOrDefault();
            if(product != null)
            {
                product.Amount = data.Amount;
                product.Description = data.Description;
                product.Name = data.Name;
                product.Image = data.Image;
                _dbContext.SaveChanges();
            }
            TempData["Message"] = "Product Edited Successfully";

            return RedirectToAction("Index"); // Redirect to the Index view
        }

        public ActionResult Details(long Id)
        {
            var product = _dbContext.Products.Where(x => x.Id == Id).FirstOrDefault();
            return View(product);
        }


        public ActionResult Delete(long Id)
        {
            var product = _dbContext.Products.Where(x => x.Id == Id).FirstOrDefault();
            if(product != null)
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
                TempData["Message"] = "Product Deleted Successfully";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Product Not Deleted";

            return RedirectToAction("Index"); // Redirect to the Index view

        }



















        //Below Functions are not needed in this project they just their 
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}