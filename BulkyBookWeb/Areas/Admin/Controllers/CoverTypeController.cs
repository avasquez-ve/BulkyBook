using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork UnitOfWorkDbContext;
        private readonly IConfiguration ConfigurationContext;

        public CoverTypeController(IUnitOfWork UnitOfWorkDb, IConfiguration configuration)
        {
            UnitOfWorkDbContext = UnitOfWorkDb;
            ConfigurationContext = configuration;
        }

        public ActionResult Index()
        {
            IEnumerable<CoverType> coverTypeList = UnitOfWorkDbContext.CoverTypes.GetAll();
            return View(coverTypeList);
        }

        // GET: CoverTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CoverTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoverType coverType)
        {
            try
            {
                int maxLengthForName = ConfigurationContext.GetValue<int>("Apptags:MaxLengthForName");
                if (coverType.Name.Length > maxLengthForName)
                {
                    ModelState.AddModelError("Name", $"The input Name cannot have more than {maxLengthForName} characters.");
                    TempData["ErrorMessage"] = "It was not possible to create a new cover type";
                }
                if (ModelState.IsValid)
                {
                    UnitOfWorkDbContext.CoverTypes.Add(coverType);
                    UnitOfWorkDbContext.Save();
                    TempData["successMessage"] = "Cover type created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {

                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                #if DEBUG
                    TempData["ErrorMessage"] = ex.Message;
                #endif

                return View(coverType);
            }
        }

        // GET: CoverTypeController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var coverTypeFromDb = UnitOfWorkDbContext.CoverTypes.GetFirstOrDefault( x => x.Id == id);

            if (coverTypeFromDb == null)
            {
                return NotFound();
            }

            return View(coverTypeFromDb);
        }

        // POST: CoverTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CoverType coverType)
        {
            int maxLengthForName = ConfigurationContext.GetValue<int>("Apptags:MaxLengthForName");
            if (coverType.Name.Length > maxLengthForName)
            {
                ModelState.AddModelError("Name", $"The input Name cannot have more than {maxLengthForName} characters.");
                TempData["ErrorMessage"] = "It was not possible to edit the cover type";
            }
            if (ModelState.IsValid)
            {
                UnitOfWorkDbContext.CoverTypes.Update(coverType);
                UnitOfWorkDbContext.Save();
                TempData["successMessage"] = "Cover type edited successfully!";
                return RedirectToAction("Index");
            }
            else
            {

                return RedirectToAction("Index");
            }
        }

        // GET: CoverTypeController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            CoverType coverType = UnitOfWorkDbContext.CoverTypes.GetFirstOrDefault(x => x.Id == id);

            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        // POST: CoverTypeController/Delete/5
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

                var coverTypeFromDb = UnitOfWorkDbContext.CoverTypes.GetFirstOrDefault(x => x.Id == id);

                if (coverTypeFromDb == null)
                {
                    return NotFound();
                }

                UnitOfWorkDbContext.CoverTypes.Remove(coverTypeFromDb);
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
