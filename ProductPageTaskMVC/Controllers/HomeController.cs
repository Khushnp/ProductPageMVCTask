using Microsoft.AspNetCore.Mvc;
using ProductPageTaskMVC.DBModel;
using ProductPageTaskMVC.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public ActionResult Create(ProductModel product,IFormFile imageDataFile)
        {
            var Data = new Product();
            if (imageDataFile != null && imageDataFile.Length > 0)
            {                
                using (var memoryStream = new MemoryStream())
                {
                    imageDataFile.CopyTo(memoryStream);
                    Data.Image = memoryStream.ToArray();
                }
                Data.Name = product.Name; Data.Description = product.Description;
                Data.Amount = product.Amount;
            }

            _dbContext.Products.Add(Data);
            _dbContext.SaveChanges();

            TempData["Message"] = "Product Inserted Successfully";

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(long Id)
        {
            var product = _dbContext.Products.Where(x=> x.Id == Id).FirstOrDefault();
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(ProductModel data, IFormFile imageDataFile)
        {
            var product = _dbContext.Products.Where(x => x.Id == data.Id).FirstOrDefault();
            if(product != null)
            {
                product.Amount = data.Amount;
                product.Description = data.Description;
                product.Name = data.Name;
                if (imageDataFile != null && imageDataFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        imageDataFile.CopyTo(memoryStream);
                        product.Image = memoryStream.ToArray();
                    }
                }
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