﻿using NLog;
using System.Linq;
using NWConsole.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

try
{
    var db = new NWContext();
    string choice;
    do
    {
        Console.WriteLine("1) Display Categories");
        Console.WriteLine("2) Add Category");
        Console.WriteLine("3) Display Category and related products");
        Console.WriteLine("4) Display all Categories and their related products");
        Console.WriteLine("\"q\" to quit");
        choice = Console.ReadLine();
        Console.Clear();
        logger.Info($"Option {choice} selected");
        if (choice == "1")
        {
            var query = db.Categories.OrderBy(p => p.CategoryName);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{query.Count()} records returned");
            Console.ForegroundColor = ConsoleColor.Magenta;
            foreach (var item in query)
            {
                Console.WriteLine($"{item.CategoryName} - {item.Description}");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        else if (choice == "2")
        {
            Category category = new Category();
            Console.WriteLine("Enter Category Name:");
            category.CategoryName = Console.ReadLine();
            Console.WriteLine("Enter the Category Description:");
            category.Description = Console.ReadLine();
            ValidationContext context = new ValidationContext(category, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(category, context, results, true);
            if (isValid)
            {
                // check for unique name
                if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db
                }
            }
            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }
        else if (choice == "3")
        {
            var query = db.Categories.OrderBy(p => p.CategoryId);

            Console.WriteLine("Select the category whose products you want to display:");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            foreach (var item in query)
            {
                Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
            }
            Console.ForegroundColor = ConsoleColor.White;
            int id = int.Parse(Console.ReadLine());
            Console.Clear();
            logger.Info($"CategoryId {id} selected");
            Category category = db.Categories.Include("Products").FirstOrDefault(c => c.CategoryId == id);            
            Console.WriteLine($"{category.CategoryName} - {category.Description}");
            foreach (Product p in category.Products)
            {
                Console.WriteLine($"\t{p.ProductName}");
            }
        }
        else if (choice == "4")
        {
            var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
            foreach (var item in query)
            {
                Console.WriteLine($"{item.CategoryName}");
                foreach (Product p in item.Products)
                {
                    Console.WriteLine($"\t{p.ProductName}");
                }
            }
        }
        else if (choice == "")
        {
           
            Console.WriteLine("1) View All Products");
            Console.WriteLine("2) View All Dicontinued Products");
            Console.WriteLine("3) View All Active Products");
            choice = Console.ReadLine();

            if (choice == "1")
            {
                var query = db.Products.OrderBy(p => p.ProductId);
                foreach (var item in query)
                {
                    Console.WriteLine($"{item.ProductName}");
                }
            }
            else if (choice == "2")
            {
                var query = db.Products.OrderBy(p => p.ProductId).Where(p.discontinued != true);
                foreach (var item in query)
                {
                    Console.WriteLine($"{item.ProductName}");
                }
            }
            else if (choice == "3")
            {
                var query = db.Products.OrderBy(p => p.ProductId).Where(p.discontinued != false);
                foreach (var item in query)
                {
                    Console.WriteLine($"{item.ProductName}");
                }
            }
        }
        else if (choice == "5")
        {
            Product product = new Product();
            Console.WriteLine("Enter Product Name:");
            product.ProductName = Console.ReadLine();
            Console.WriteLine("Enter a Supplier ID:");
            product.SupplierId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter a Cateregory ID:");
            product.CategoryId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter Quantity per unit:");
            product.QuantityPerUnit = Console.ReadLine();
            Console.WriteLine("Enter unit price:");
            product.UnitPrice = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("enter units in stock:");
            product.UnitsInStock = Convert.ToShort(Console.ReadLine());
            Console.WriteLine("Enter units on order:");
            product.UnitsOnOrder = Convert.toshort(Console.ReadLine());
            Console.WriteLine("Enter reorder level:");
            product.ReorderLevel = Convert.ToShort(Console.ReadLine());
            Console.WriteLine("Is this product discontinued:");
            product.Discontinued = Convert.ToBoolean(Console.ReadLine());

            ValidationContext context = new ValidationContext(product, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(product, context, results, true);
            if (isValid)
            {
                // check for unique name
                if (db.Categories.Any(c => c.CategoryName == product.ProductName))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db
                }
            }
            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }

        }
        Console.WriteLine();
    } while (choice.ToLower() != "q");
}
catch (Exception ex)
{
    logger.Error(ex.Message);
}

logger.Info("Program ended");



static Product GetProduct(ProductContext db, Logger logger)
{
    var products = db.Products.OrderBy(b => p.ProductID);
    foreach (Product p in products)
    {
        Console.WriteLine($"{p.ProductId}: {p.ProductName}");
    }
    if (int.TryParse(Console.ReadLine(), out int ProductID))
    {
        Product product = db.Products.FirstOrDefault(b => p.ProductID == ProductID);
        if (product != null)
        {
            return product;
        }
    }
    logger.Error("Invalid Product ID");
    return null;
}
