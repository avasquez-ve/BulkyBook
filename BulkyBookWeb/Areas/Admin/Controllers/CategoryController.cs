using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork UnitOfWorkDbContext;
        private readonly IConfiguration ConfigurationContext;
        public CategoryController(IUnitOfWork UnitOfWorkdb, IConfiguration configuration)
        {
            UnitOfWorkDbContext = UnitOfWorkdb;
            ConfigurationContext = configuration;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = UnitOfWorkDbContext.Categories.GetAll();
            return View(categoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            int maxLengthForName = ConfigurationContext.GetValue<int>("AppTags:MaxLengthForName");
            if (category.Name.Length > maxLengthForName)
            {
                ModelState.AddModelError("Name", $"The input Name cannot have more than {maxLengthForName} characters.");
                TempData["ErrorMessage"] = "It was not possible to create a new category";
            }
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The input DisplayOrder cannot exactly same as the input Name.");
                TempData["ErrorMessage"] = "It was not possible to create a new category";
            }
            if (ModelState.IsValid)
            {
                UnitOfWorkDbContext.Categories.Add(category);
                UnitOfWorkDbContext.Save();
                TempData["successMessage"] = "Category created successfully!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            //var categoryFromDb = DbContext.Categories.Find(id);
            //Diferent ways to return data using entity framework
            var categoryFromDb = UnitOfWorkDbContext.Categories.GetFirstOrDefault(x => x.Id == id);
            //var categoryFromDb = DbContext.Categories.SingleOrDefault(x => x.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            int maxLengthForName = ConfigurationContext.GetValue<int>("AppTags:MaxLengthForName");
            if (category.Name.Length > maxLengthForName)
            {
                ModelState.AddModelError("Name", $"The input Name cannot have more than {maxLengthForName} characters.");
            }
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The input DisplayOrder cannot exactly same as the input Name.");
            }
            if (ModelState.IsValid)
            {
                UnitOfWorkDbContext.Categories.Update(category);
                UnitOfWorkDbContext.Save();
                TempData["successMessage"] = "Category edited successfully!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var categoryFromDb = UnitOfWorkDbContext.Categories.GetFirstOrDefault(x => x.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = UnitOfWorkDbContext.Categories.GetFirstOrDefault(x => x.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            UnitOfWorkDbContext.Categories.Remove(categoryFromDb);
            UnitOfWorkDbContext.Save();
            TempData["successMessage"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
