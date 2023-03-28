using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork UnitOfWorkDbContext;
        private readonly IConfiguration ConfigurationContext;
        private readonly IWebHostEnvironment HostEnvironment;
        public string WWWRootPath { get { return HostEnvironment.WebRootPath; } }

        public ProductController(IUnitOfWork UnitOfWorkDb, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            UnitOfWorkDbContext = UnitOfWorkDb;
            ConfigurationContext = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = UnitOfWorkDbContext.Categories.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                CoverTypeList = UnitOfWorkDbContext.CoverTypes.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id == null || id <= 0)
            {
                //create
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = UnitOfWorkDbContext.Products.GetFirstOrDefault(x => x.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productObj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string randomFileName = Guid.NewGuid().ToString();
                    string imageProductFolder = ConfigurationContext.GetValue<string>("Apptags:imageProductFolder");
                    var uploadFolder = Path.Combine(WWWRootPath, imageProductFolder);
                    var fileExtension = Path.GetExtension(file.FileName);

                    if (fileExtension != ".jpg" && fileExtension != ".png")
                    {
                        TempData["ErrorMessage"] = "Only images with .jpg or .png extensions are valid.";
                        return RedirectToAction("Index");
                    }

                    if (!string.IsNullOrEmpty(productObj.Product.ImageUrl))
                    {
                        var oldPathFile = Path.Combine(WWWRootPath, productObj.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldPathFile))
                        {
                            System.IO.File.Delete(oldPathFile);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(uploadFolder, randomFileName + fileExtension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productObj.Product.ImageUrl = @$"images/products/{randomFileName}{fileExtension}"; 

                }

                if (productObj.Product.Id == 0)
                {
                    UnitOfWorkDbContext.Products.Add(productObj.Product);
                }
                else
                {
                    UnitOfWorkDbContext.Products.Update(productObj.Product);
                }

                UnitOfWorkDbContext.Save();
                TempData["successMessage"] = "Product created successfully!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = UnitOfWorkDbContext.Products.GetAll("CoverType, Category");
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null || id <= 0)
                {
                    return Json(new { success = false, message = "Error while deleting, must be specified an product id." });
                }

                var ProductFromDb = UnitOfWorkDbContext.Products.GetFirstOrDefault(x => x.Id == id);

                if (ProductFromDb == null)
                {
                    return Json(new { success = false, message = "Error while deleting, can't get the product." });
                }

                if (!string.IsNullOrEmpty(ProductFromDb.ImageUrl))
                {
                    var oldPathFile = Path.Combine(WWWRootPath, ProductFromDb.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldPathFile))
                    {
                        System.IO.File.Delete(oldPathFile);
                    }
                }

                UnitOfWorkDbContext.Products.Remove(ProductFromDb);
                UnitOfWorkDbContext.Save();

                return Json(new { success = true, message = "Product deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error while deleting. Exception Message: {ex.Message}" });
            }
        }
        #endregion
    }
}
