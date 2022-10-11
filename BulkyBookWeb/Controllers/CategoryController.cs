using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IConfiguration ConfigurationContext;
        public CategoryController(ApplicationDbContext db, IConfiguration configuration)
        {
            DbContext = db;
            ConfigurationContext = configuration;
        }

        public IActionResult Index()
        {
            List<Models.Category> categoryList = DbContext.Categories.ToList();
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
            }
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The input DisplayOrder cannot exactly same as the input Name.");
            }
            if (ModelState.IsValid)
            {
                DbContext.Add(category);
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }  
            return View(category);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = DbContext.Categories.Find(id);
            //Diferent ways to return data using entity framework
            //var categoryFromDb = DbContext.Categories.FirstOrDefault(x => x.Id == categoryId);
            //var categoryFromDb = DbContext.Categories.SingleOrDefault(x => x.Id == categoryId);

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
                DbContext.Update(category);
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = DbContext.Categories.Find(id);

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

            var categoryFromDb = DbContext.Categories.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            DbContext.Remove(categoryFromDb);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }



        //// GET: CategoryController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}


        //// POST: CategoryController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: CategoryController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: CategoryController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: CategoryController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: CategoryController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
