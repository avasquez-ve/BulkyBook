using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext DbContext;
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public void Save()
        {
            DbContext.SaveChanges();
        }

        public void Update(Category category)
        {
            DbContext.Categories.Update(category);
        }
    }
}
