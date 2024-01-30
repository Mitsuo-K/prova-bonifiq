using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class ProductService
	{
		TestDbContext _ctx;
        private const int PageLimit = 10;

        public ProductService(TestDbContext ctx)
		{
			_ctx = ctx;
		}

		public ProductList  ListProducts(int page)
		{
            int skipCount = (page - 1) * PageLimit;
            var products = _ctx.Products.Skip(skipCount).Take(PageLimit).ToList();

            return new ProductList
            {
                HasNext = products.Count == PageLimit,
                TotalCount = _ctx.Customers.Count(),
                Products = products
            };
		}

	}
}
