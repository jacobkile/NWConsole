using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using NWConsole.Model;

public class NWContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public void AddProduct(Product product)
    {
        this.Products.Add(product);
        this.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        this.Products.Remove(product);
        this.SaveChanges();
    }

    public void EditProduct(Product UpdatedProduct)
    {
        Product product = this.Products.Find(UpdatedProduct.ProductId);
        product.ProductName = UpdatedProduct.ProductName;
        this.SaveChanges();
    }

    public void AddCategory(Category category)
    {
        this.Categories.Add(category);
        this.SaveChanges();
    }

    public void DeleteCategory(Category category)
    {
        this.Categories.Remove(category);
        this.SaveChanges();
    }

    public void EditCategory(Category UpdatedCategory)
    {
        Category category = this.Categories.Find(UpdatedCategory.CategoryId);
        category.CategoryName = UpdatedCategory.CategoryName;
        this.SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration =  new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json");
            
        var config = configuration.Build();
        optionsBuilder.UseSqlServer(@config["BlogsConsole:ConnectionString"]);
    }
}