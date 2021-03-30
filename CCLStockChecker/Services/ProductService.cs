using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using CCLStockChecker.Constants;
using CCLStockChecker.Constants.Enums;
using CCLStockChecker.Models;

namespace CCLStockChecker.Services
{
    public static class ProductService
    {
        public static void OpenChrome(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(SearchStrings.CCLTestPage);
            driver.Manage().Window.FullScreen();
        }

        
        public static IList<ProductModel> GetAllProducts(IWebDriver driver)
        {
            IList<ProductModel> products = new List<ProductModel>();
            try
            {
                var elements = driver.FindElements(By.CssSelector("[class=\"productList\"]"));
                foreach (var element in elements)
                {
                    var name = GetName(element);
                    var inStock = GetStockStatus(element);
                    var link = GetLink(element);
                    var price = "0";

                    Console.WriteLine(inStock);

                    if (inStock == StockEnum.OutofStock)
                    {
                        price = ("OutofStock");
                    }
                    else
                    {
                        price = GetPrice(element);
                    }


                    var productModel = new ProductModel()
                    {
                        Name = name,
                        Price = price,
                        InStock = inStock,
                        Link = link
                    };

                    PrintProductToConsole(productModel);
                    products.Add(productModel);
                }

                Console.WriteLine($"Products found: {products.Count}");
                return products;
            }
            catch (Exception e)
            {
                Console.WriteLine($"No products found {e.Message}");
                return null;
            }
        }

        public static void CheckInStock(IEnumerable<ProductModel> products, IWebDriver driver, BuyerModel buyer)
        {
            foreach (var product in products)
            {
                if (product.InStock == StockEnum.Instock)
                {
                    BuyingService.BuyProduct(product, driver, buyer);
                }
            }
        }

        private static StockEnum GetStockStatus(IWebElement element)
        {
            try
            {
                var rawStockStatus = element.FindElement(By.ClassName("availability-box"));
                Console.WriteLine(rawStockStatus);
                var rawText = rawStockStatus.Text;
                var formattedText = rawText.Split(",");

                if (formattedText.First() != "FREE")
                {
                    return StockEnum.OutofStock;
                }

                return StockEnum.Instock;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StockEnum.OutofStock;
            }

        }

        private static string GetName(IWebElement element)
        {
            try
            {
                var rawName = element.FindElement(By.ClassName("productList_Middle_Left"));
                return rawName.Text;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        private static string GetLink(IWebElement element)
        {
            try
            {
                var linkTag = element.FindElement(By.TagName("a"));
                return linkTag.GetAttribute("href");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private static string GetPrice(IWebElement element)
        {
            try
            {
                var rawPriceString = element.FindElement(By.ClassName("price"));
                //var rawPrice = rawPriceString.Text.Split("\r", StringSplitOptions.None);
                Console.WriteLine("hey " + rawPriceString);
                return rawPriceString.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private static void PrintProductToConsole(ProductModel productModel)
        {
            Console.WriteLine(productModel.Name);
            Console.WriteLine(productModel.Price);
            Console.WriteLine(productModel.InStock);
            Console.WriteLine(productModel.Link);
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------\n");
        }
    }
}