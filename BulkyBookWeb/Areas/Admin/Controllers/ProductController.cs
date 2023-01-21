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

        public ProductController(IUnitOfWork UnitOfWorkDb, IConfiguration configuration)
        {
            UnitOfWorkDbContext = UnitOfWorkDb;
            ConfigurationContext = configuration;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upsert(int? id)
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
        public ActionResult Upsert(ProductVM product, IFormFile file)
        {
            int maxLengthForName = ConfigurationContext.GetValue<int>("Apptags:MaxLengthForName");
            //if (product.Title.Length > maxLengthForName)
            //{
            //    ModelState.AddModelError("Name", $"The input Name cannot have more than {maxLengthForName} characters.");
            //    TempData["ErrorMessage"] = "It was not possible to edit the cover type";
            //}
            if (ModelState.IsValid)
            {
                //UnitOfWorkDbContext.Products.Update(Product);
                UnitOfWorkDbContext.Save();
                TempData["successMessage"] = "Cover type edited successfully!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int? id)
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
        public ActionResult DeletePost(int? id)
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
    }
}
