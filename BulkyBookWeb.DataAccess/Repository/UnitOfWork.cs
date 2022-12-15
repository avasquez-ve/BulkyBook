using BulkyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext DbContext;
        public UnitOfWork(ApplicationDbContext dbContext) 
        {
            DbContext = dbContext;
            Categories = new CategoryRepository(DbContext);
        }
        public ICategoryRepository Categories { get; private set; }

        public void Save()
        {
            DbContext.SaveChanges();
        }
    }
}
