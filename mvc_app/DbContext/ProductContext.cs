


using Microsoft.EntityFrameworkCore;
using mvc_app.Models;

//namespace mvc_app
//{
    public class ProductContext:DbContext
    {

        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
    }
//}
