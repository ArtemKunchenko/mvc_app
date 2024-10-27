
using Microsoft.EntityFrameworkCore;
using mvc_app.Models;

namespace mvc_app.Services
{
    public interface IServiceProducts
    {
        public Task<Product?> CreateAsync(Product? product);
        public Task<IEnumerable<Product>> ReadAsync();
        public Task<Product?> GetByIdAsync(int id);
        public Task<Product?> UpdateAsync(int id, Product? product);
        public Task<bool> DeleteAsync(int id);

    }

    public class ServiceProducts : IServiceProducts
    {
        private readonly ProductContext _productContext;
        private readonly ILogger<ServiceProducts> _logger;
        public ServiceProducts(
            ProductContext productContext,
            ILogger<ServiceProducts> logger
            )
        {
            _productContext = productContext;
            _logger = logger;
        }

        public async Task<Product?> CreateAsync(Product? product)
        {
            if (product == null)
            {
                _logger.LogWarning("Attempt is created product with null");
                return null;
            }
            await _productContext.Products.AddAsync(product);
            await _productContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productContext.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogInformation("Not found product");
                return false;
            }
            _productContext.Products.Remove(product);
            await _productContext.SaveChangesAsync();
            return true;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _productContext.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> ReadAsync()
        {
            return await _productContext.Products.ToListAsync();
        }

        public async Task<Product?> UpdateAsync(int id, Product? product)
        {
            if (product == null || id != product.Id)
            {
                _logger.LogWarning("if (product == null || id != product.Id)");
                return null;
            }
            try
            {
                _productContext.Products.Update(product);
                await _productContext.SaveChangesAsync();
                return product;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}