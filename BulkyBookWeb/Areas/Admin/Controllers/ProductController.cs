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
            }

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = HostEnvironment.WebRootPath;

                if (file != null)
                {
                    string randomFileName = Guid.NewGuid().ToString();
                    string imageProductFolder = ConfigurationContext.GetValue<string>("Apptags:imageProductFolder");
                    var uploadFolder = Path.Combine(wwwRootPath, imageProductFolder);
                    var fileExtension = Path.GetExtension(file.FileName);

                    if (fileExtension != ".jpg" && fileExtension != ".png")
                    {
                        TempData["ErrorMessage"] = "Only images with .jpg or .png extensions are valid.";
                        return RedirectToAction("Index");
                    }

                    using (var fileStream = new FileStream(Path.Combine(uploadFolder, randomFileName + fileExtension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = $"{imageProductFolder}/{randomFileName}/{fileExtension}"; 

                }
                UnitOfWorkDbContext.Products.Add(productVM.Product);
                UnitOfWorkDbContext.Save();
                TempData["successMessage"] = "Product created successfully!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

        // GET: ProductController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            Product Product = UnitOfWorkDbContext.Products.GetFirstOrDefault(x => x.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // POST: ProductController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            try
            {
                if (id == null || id <= 0)
                {
                    return NotFound();
                }

                var ProductFromDb = UnitOfWorkDbContext.Products.GetFirstOrDefault(x => x.Id == id);

                if (ProductFromDb == null)
                {
                    return NotFound();
                }

                UnitOfWorkDbContext.Products.Remove(ProductFromDb);
                UnitOfWorkDbContext.Save();
                TempData["successMessage"] = "Cover type deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                #if DEBUG
                    TempData["ErrorMessage"] = ex.Message;
                #endif
                
                TempData["ErrorMessage"] = "It was not possible to delete the cover type";
                return RedirectToAction("Index");
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = UnitOfWorkDbContext.Products.GetAll("CoverType, Category");
            return Json(new { data = productList });
        }
        #endregion
    }
}
