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
            Console.WriteLine("3) Display All Products");
            Console.WriteLine("4) Display Specfic Product");
            choice = Console.ReadLine();
            Console.Clear();
            logger.Info("product option {choice} selected", choice);

            if (choice == "1") //Add Product
            {
                Product product = InputProduct(db, logger);
                if (product != null)
                {
                    db.AddProduct(product);
                    logger.Info("Product Added - {ProductName}", product.ProductName);
                }
            }

            else if (choice == "2") //Edit Product
            {

            }

            else if (choice == "3") //Display All Product
            {
                var query = db.Products.OrderBy(p => p.ProductId);

                Console.WriteLine($"{query.Count()} Products Returned");
                foreach (var item in query)
                {
                    Console.WriteLine(item.ProductName);
                }
            }

            else if (choice == "4") //Display Specfic Product
            {
                
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
                    Console.WriteLine(item.CategoryName, item.Description);
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

Product InputProduct(NWContext db, Logger logger)
{
    Product product = new Product();

    Console.WriteLine("Enter Product Name:");
    product.ProductName = Console.ReadLine();

    // Check if a product with the same name already exists
    if (db.Products.Any(p => p.ProductName == product.ProductName))
    {
        Console.WriteLine("A product with the same name already exists.");
        return null; // You can return null or handle this case as needed
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

// try
// {
//     var db = new NWContext();
//     string choice;
//     do
//     {
//         Console.WriteLine("1) Display Categories");
//         Console.WriteLine("2) Add Category");
//         Console.WriteLine("3) Display Category and related products");
//         Console.WriteLine("4) Display all Categories and their related products");
//         Console.WriteLine("\"q\" to quit");
//         choice = Console.ReadLine();
//         Console.Clear();
//         logger.Info($"Option {choice} selected");
//         if (choice == "1")
//         {
//             var query = db.Categories.OrderBy(p => p.CategoryName);

//             Console.ForegroundColor = ConsoleColor.Green;
//             Console.WriteLine($"{query.Count()} records returned");
//             Console.ForegroundColor = ConsoleColor.Magenta;
//             foreach (var item in query)
//             {
//                 Console.WriteLine($"{item.CategoryName} - {item.Description}");
//             }
//             Console.ForegroundColor = ConsoleColor.White;
//         }
//         else if (choice == "2")
//         {
//             Category category = new Category();
//             Console.WriteLine("Enter Category Name:");
//             category.CategoryName = Console.ReadLine();
//             Console.WriteLine("Enter the Category Description:");
//             category.Description = Console.ReadLine();
//             ValidationContext context = new ValidationContext(category, null, null);
//             List<ValidationResult> results = new List<ValidationResult>();

//             var isValid = Validator.TryValidateObject(category, context, results, true);
//             if (isValid)
//             {
//                 // check for unique name
//                 if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
//                 {
//                     // generate validation error
//                     isValid = false;
//                     results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
//                 }
//                 else
//                 {
//                     logger.Info("Validation passed");
//                     // TODO: save category to db
//                 }
//             }
//             if (!isValid)
//             {
//                 foreach (var result in results)
//                 {
//                     logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
//                 }
//             }
//         }
//         else if (choice == "3")
//         {
//             var query = db.Categories.OrderBy(p => p.CategoryId);

//             Console.WriteLine("Select the category whose products you want to display:");
//             Console.ForegroundColor = ConsoleColor.DarkRed;
//             foreach (var item in query)
//             {
//                 Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
//             }
//             Console.ForegroundColor = ConsoleColor.White;
//             int id = int.Parse(Console.ReadLine());
//             Console.Clear();
//             logger.Info($"CategoryId {id} selected");
//             Category category = db.Categories.Include("Products").FirstOrDefault(c => c.CategoryId == id);            
//             Console.WriteLine($"{category.CategoryName} - {category.Description}");
//             foreach (Product p in category.Products)
//             {
//                 Console.WriteLine($"\t{p.ProductName}");
//             }
//         }
//         else if (choice == "4")
//         {
//             var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
//             foreach (var item in query)
//             {
//                 Console.WriteLine($"{item.CategoryName}");
//                 foreach (Product p in item.Products)
//                 {
//                     Console.WriteLine($"\t{p.ProductName}");
//                 }
//             }
//         }
//         else if (choice == "")
//         {
           
//             Console.WriteLine("1) View All Products");
//             Console.WriteLine("2) View All Dicontinued Products");
//             Console.WriteLine("3) View All Active Products");
//             choice = Console.ReadLine();

//             if (choice == "1")
//             {
//                 var query = db.Products.OrderBy(p => p.ProductId);
//                 foreach (var item in query)
//                 {
//                     Console.WriteLine($"{item.ProductName}");
//                 }
//             }
//             else if (choice == "2")
//             {
//                 var query = db.Products.OrderBy(p => p.ProductId).Where(p.discontinued != true);
//                 foreach (var item in query)
//                 {
//                     Console.WriteLine($"{item.ProductName}");
//                 }
//             }
//             else if (choice == "3")
//             {
//                 var query = db.Products.OrderBy(p => p.ProductId).Where(p.discontinued != false);
//                 foreach (var item in query)
//                 {
//                     Console.WriteLine($"{item.ProductName}");
//                 }
//             }
//         }
//         else if (choice == "5")
//         {
//             Product product = new Product();
//             Console.WriteLine("Enter Product Name:");
//             product.ProductName = Console.ReadLine();
//             Console.WriteLine("Enter a Supplier ID:");
//             product.SupplierId = Convert.ToInt32(Console.ReadLine());
//             Console.WriteLine("Enter a Cateregory ID:");
//             product.CategoryId = Convert.ToInt32(Console.ReadLine());
//             Console.WriteLine("Enter Quantity per unit:");
//             product.QuantityPerUnit = Console.ReadLine();
//             Console.WriteLine("Enter unit price:");
//             product.UnitPrice = Convert.ToDecimal(Console.ReadLine());
//             Console.WriteLine("enter units in stock:");
//             product.UnitsInStock = Convert.ToShort(Console.ReadLine());
//             Console.WriteLine("Enter units on order:");
//             product.UnitsOnOrder = Convert.toshort(Console.ReadLine());
//             Console.WriteLine("Enter reorder level:");
//             product.ReorderLevel = Convert.ToShort(Console.ReadLine());
//             Console.WriteLine("Is this product discontinued:");
//             product.Discontinued = Convert.ToBoolean(Console.ReadLine());

//             ValidationContext context = new ValidationContext(product, null, null);
//             List<ValidationResult> results = new List<ValidationResult>();

//             var isValid = Validator.TryValidateObject(product, context, results, true);
//             if (isValid)
//             {
//                 // check for unique name
//                 if (db.Categories.Any(c => c.CategoryName == product.ProductName))
//                 {
//                     // generate validation error
//                     isValid = false;
//                     results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
//                 }
//                 else
//                 {
//                     logger.Info("Validation passed");
//                     // TODO: save category to db
//                 }
//             }
//             if (!isValid)
//             {
//                 foreach (var result in results)
//                 {
//                     logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
//                 }
//             }

//         }
//         Console.WriteLine();
//     } while (choice.ToLower() != "q");
// }
// catch (Exception ex)
// {
//     logger.Error(ex.Message);
// }

// logger.Info("Program ended");



// static Product GetProduct(ProductContext db, Logger logger)
// {
//     var products = db.Products.OrderBy(b => p.ProductID);
//     foreach (Product p in products)
//     {
//         Console.WriteLine($"{p.ProductId}: {p.ProductName}");
//     }
//     if (int.TryParse(Console.ReadLine(), out int ProductID))
//     {
//         Product product = db.Products.FirstOrDefault(b => p.ProductID == ProductID);
//         if (product != null)
//         {
//             return product;
//         }
//     }
//     logger.Error("Invalid Product ID");
//     return null;
// }
