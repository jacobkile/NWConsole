using NLog;
using System.Linq;
using NWConsole.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

string path = Directory.GetCurrentDirectory() + "\\nlog.config";

var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

try
{
    string choice;
    do
    {
        Console.WriteLine("Select Table to enter:");
        Console.WriteLine("1) Products");
        Console.WriteLine("2) Category");
        Console.WriteLine("Enter q to quit");
        choice = Console.ReadLine();
        Console.Clear();
        logger.Info("Option {choice} selected", choice);

        var db = new NWContext();

        if (choice == "1") //Products Table
        {
            Console.WriteLine("What Would You Like To Do:");
            Console.WriteLine("1) Add New Product");
            Console.WriteLine("2) Edit Product");
            Console.WriteLine("3) Display Products");
            Console.WriteLine("4) Display Specfic Product");
            choice = Console.ReadLine();
            Console.Clear();
            logger.Info("product option {choice} selected", choice);

            if (choice == "1") //Add Product
            {
                Product product = InputProduct(logger);
                if (product != null)
                {
                    db.AddProduct(product);
                    logger.Info("Product Added - {ProductName}", product.ProductName);
                }
            }

            else if (choice == "2") //Edit Product
            {
            Console.WriteLine("Enter the Product ID you want to edit:");
                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    var product = db.Products.FirstOrDefault(p => p.ProductId == productId);
                    if (product != null)
                    {
                        Console.WriteLine("Current Product Details:");
                        Console.WriteLine($"Product Name: {product.ProductName}");
                        Console.WriteLine($"Supplier ID: {product.SupplierId}");
                        Console.WriteLine($"Category ID: {product.CategoryId}");
                        Console.WriteLine($"Quantity Per Unit: {product.QuantityPerUnit}");
                        Console.WriteLine($"Unit Price: {product.UnitPrice}");
                        Console.WriteLine($"Units In Stock: {product.UnitsInStock}");
                        Console.WriteLine($"Units On Order: {product.UnitsOnOrder}");
                        Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
                        Console.WriteLine($"Discontinued: {product.Discontinued}");

                        Console.WriteLine("\nEnter New Product Details:");

                        Console.WriteLine("Enter Product Name:");
                        product.ProductName = Console.ReadLine();

                        Console.WriteLine("Enter Supplier ID:");
                        if (int.TryParse(Console.ReadLine(), out int supplierId))
                        {
                            product.SupplierId = supplierId;
                        }

                        Console.WriteLine("Enter Category ID:");
                        if (int.TryParse(Console.ReadLine(), out int categoryId))
                        {
                            product.CategoryId = categoryId;
                        }

                        Console.WriteLine("Enter Quantity Per Unit:");
                        product.QuantityPerUnit = Console.ReadLine();

                        Console.WriteLine("Enter Unit Price:");
                        if (decimal.TryParse(Console.ReadLine(), out decimal unitPrice))
                        {
                            product.UnitPrice = unitPrice;
                        }

                        Console.WriteLine("Enter Units In Stock:");
                        if (short.TryParse(Console.ReadLine(), out short unitsInStock))
                        {
                            product.UnitsInStock = unitsInStock;
                        }

                        Console.WriteLine("Enter Units On Order:");
                        if (short.TryParse(Console.ReadLine(), out short unitsOnOrder))
                        {
                            product.UnitsOnOrder = unitsOnOrder;
                        }

                        Console.WriteLine("Enter Reorder Level:");
                        if (short.TryParse(Console.ReadLine(), out short reorderLevel))
                        {
                            product.ReorderLevel = reorderLevel;
                        }

                        Console.WriteLine("Is this product discontinued? (true/false):");
                        if (bool.TryParse(Console.ReadLine(), out bool discontinued))
                        {
                            product.Discontinued = discontinued;
                        }

                        db.EditProduct(product);
                        logger.Info("Product Edited - Product ID: {ProductId}", product.ProductId);
                    }
                    else
                    {
                        Console.WriteLine("Product not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid Product ID.");
                }
            }

            else if (choice == "3") //Display All Product
            {
                Console.WriteLine("1) Display All Products");
                Console.WriteLine("2) Display All Discontiued Products");
                Console.WriteLine("3) Display All Active Products");
                choice = Console.ReadLine();
                if (choice =="1")
                {
                    var query = db.Products.OrderBy(p => p.ProductId);

                    Console.WriteLine($"{query.Count()} Products Returned");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.ProductName);
                    }
                }

                else if (choice == "2")
                {
                    var query = db.Products.Where(p => p.Discontinued).OrderBy(p => p.ProductId);

                    Console.WriteLine($"Discontinued Products Returned: {query.Count()}");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.ProductName);
                    }
                }

                else if (choice == "3")
                {
                    var query = db.Products.Where(p => !p.Discontinued).OrderBy(p => p.ProductId);

                    Console.WriteLine($"Discontinued Products Returned: {query.Count()}");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.ProductName);
                    }
                }
            }

            else if (choice == "4") //Display Specfic Product
            {
                var query = db.Products.OrderBy(p => p.ProductId);

                Console.WriteLine($"{query.Count()} Products Returned");
                foreach (var item in query)
                {
                    Console.WriteLine($"{item.ProductId}. {item.ProductName}");
                }

                Console.WriteLine("Enter the Product ID you want to view:");
                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    var product = db.Products.FirstOrDefault(p => p.ProductId == productId);
                    if (product != null)
                    {
                        Console.WriteLine($"Product ID: {product.ProductId}");
                        Console.WriteLine($"Product Name: {product.ProductName}");
                        Console.WriteLine($"Supplier ID: {product.SupplierId}");
                        Console.WriteLine($"Category ID: {product.CategoryId}");
                        Console.WriteLine($"Quantity Per Unit: {product.QuantityPerUnit}");
                        Console.WriteLine($"Unit Price: {product.UnitPrice}");
                        Console.WriteLine($"Units In Stock: {product.UnitsInStock}");
                        Console.WriteLine($"Units On Order: {product.UnitsOnOrder}");
                        Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
                        Console.WriteLine($"Discontinued: {product.Discontinued}");
                    }
                    else
                    {
                        Console.WriteLine("Product not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid Product ID.");
                }
                
            }

        }

        else if (choice =="2") //Category Table 
        {
            Console.WriteLine("What Would You Like To Do:");
            Console.WriteLine("1) Add New Category");
            Console.WriteLine("2) Edit Category");
            Console.WriteLine("3) Display All Categories");
            Console.WriteLine("4) Display All Categories with active Product data");
            Console.WriteLine("5) Display Specific Category with active product data");
            choice = Console.ReadLine();
            Console.Clear();
            logger.Info("Category option {choice} selected", choice);

            if (choice == "1")
            {

            }

            else if (choice == "2")
            {

            }

            else if (choice == "3")
            {
                var query = db.Categories.OrderBy(c => c.CategoryId);

                Console.WriteLine($"{query.Count()} Categories Returned");
                foreach (var item in query)
                {
                    Console.WriteLine($"{item.CategoryName}: {item.Description}");
                }
            }

            else if (choice == "4")
            {
               var query = db.Categories.Include(p => p.Products).OrderBy(c => c.CategoryId).ToList();

                foreach (var category in query)
                {
                    Console.WriteLine($"{category.CategoryName}:");

                    foreach (var product in category.Products.OrderBy(p => p.ProductName))
                    {
                        Console.WriteLine($"\t{product.ProductName}");
                    }

                    Console.WriteLine();
                } 
            }

            else if (choice == "5")
            {
                 
            }

        }

    } while (choice.ToLower() != "q");
}
catch (Exception ex)
{
    logger.Error(ex.Message);
}

logger.Info("Program ended");

static Product InputProduct(Logger logger)
{
    Product product = new Product();
    Console.WriteLine("Enter Product Name:");
    product.ProductName = Console.ReadLine();

    using (var db = new NWContext())
    {
    if (db.Products.Any(p => p.ProductName == product.ProductName))
        {
            Console.WriteLine("A product with the same name already exists.");
            return null;
        }
    }

    Console.WriteLine("Enter Supplier ID:");
    if (int.TryParse(Console.ReadLine(), out int supplierId))
        {
            product.SupplierId = supplierId;
        }

        Console.WriteLine("Enter Category ID:");
        if (int.TryParse(Console.ReadLine(), out int categoryId))
            {
                product.CategoryId = categoryId;
            }

        Console.WriteLine("Enter Quantity Per Unit:");
        product.QuantityPerUnit = Console.ReadLine();

        Console.WriteLine("Enter Unit Price:");
        if (decimal.TryParse(Console.ReadLine(), out decimal unitPrice))
            {
                product.UnitPrice = unitPrice;
            }

        Console.WriteLine("Enter Units in Stock:");
        if (short.TryParse(Console.ReadLine(), out short unitsInStock))
            {
                product.UnitsInStock = unitsInStock;
            }

        Console.WriteLine("Enter Units on Order:");
        if (short.TryParse(Console.ReadLine(), out short unitsOnOrder))
            {
                product.UnitsOnOrder = unitsOnOrder;
            }

        Console.WriteLine("Enter Reorder Level:");
        if (short.TryParse(Console.ReadLine(), out short reorderLevel))
            {
                product.ReorderLevel = reorderLevel;
            }

        Console.WriteLine("Is this product discontinued? (true/false):");
        if (bool.TryParse(Console.ReadLine(), out bool discontinued))
            {
                product.Discontinued = discontinued;
            }

        return product;
        }
static Category InputCategory(Logger logger)
        {
        Category category = new Category();

        Console.WriteLine("Enter Category Name:");
        category.CategoryName = Console.ReadLine();

        using (var db = new NWContext())
            {
                if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                {
                    Console.WriteLine("A category with the same name already exists.");
                    return null;
                }
            }

        Console.WriteLine("Enter Category Description:");
        category.Description = Console.ReadLine();

        return category;
        }
