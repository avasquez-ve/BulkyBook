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
            CoverTypes = new CoverTypeRepository(DbContext);
            Products = new ProductRepository(DbContext);
        }
        public ICategoryRepository Categories { get; private set; }
        public ICoverTypeRepository CoverTypes { get; private set; }
        public IProductRepository Products { get; private set; }


        public void Save()
        {
            DbContext.SaveChanges();
        }
    }
}
