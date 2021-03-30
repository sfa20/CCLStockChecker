using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using CCLStockChecker.Constants;
using CCLStockChecker.Models;
using CCLStockChecker.Services;

namespace CCLStockChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            //var isAccepted = false;
            IEnumerable<ProductModel> products = null;


                var userDetails = new BuyerModel()
                {
                    Title = "Mr",
                    Forename = "Scott",
                    Surname = "Farrell",
                    MobileNumber = "07949145866",
                    AddressLine1 = "10 Sandiland Drive",
                    City = "West Lothian",
                    Postcode = "EH530LD",
                    cardNumber = "0000111122223333",
                    ExpiryDate = "01/22",
                    SecNumber = "888"
                };

                // Create a driver instance for chromedriver
                IWebDriver driver = new ChromeDriver();
                ProductService.OpenChrome(driver);

               

                products = ProductService.GetAllProducts(driver);


                while (true)
                {
                    Thread.Sleep(5000);
                    driver.Navigate().Refresh();
                    products = ProductService.GetAllProducts(driver);
                    ProductService.CheckInStock(products, driver, userDetails);
                }
                driver.Quit();
            }
    }
}
