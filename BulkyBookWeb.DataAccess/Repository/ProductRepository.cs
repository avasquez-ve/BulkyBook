using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext DbContext;
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public void Update(Product product)
        {
            DbContext.Products.Update(product);//It updates all data of your object
            var productFromdB = DbContext.Products.FirstOrDefault(x => x.Id == product.Id);
            if (productFromdB != null)
            {
                productFromdB.Title = product.Title;
                productFromdB.ISBN = product.ISBN;
                productFromdB.Price = product.Price;
                productFromdB.PriceFor50 = product.PriceFor50;
                productFromdB.PriceFor100 = product.PriceFor100;
                productFromdB.ListPrice = product.ListPrice;
                productFromdB.Description = product.Description;
                productFromdB.CategoryId = product.CategoryId;
                productFromdB.Author = product.Author;
                productFromdB.CoverTypeId = product.CoverTypeId;
                if (product.ImageUrl != null)
                {
                    productFromdB.ImageUrl = product.ImageUrl;
                }

            }
        }
    }
}
