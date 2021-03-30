using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using CCLStockChecker.Models;

namespace CCLStockChecker.Services
{
    public class BuyingService
    {
        public static void BuyProduct(ProductModel product, IWebDriver driver, BuyerModel buyer)
        {
            NavigateToLink(product, driver);
            ClickAddToBasket(driver);
            ClickContinueToCheckout(driver);
            GoToCheckout(driver);
            WhereToDeliver(driver);
            SelectDeliveryDate(driver);
            EnterEmailAndCheckoutAsGuest(driver);
            EnterDetails(driver, buyer);
            MakePayment(driver, buyer);
        }

        public static void NavigateToLink(ProductModel product, IWebDriver driver)
        {
            try
            {
                driver.Navigate().GoToUrl(product.Link);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Couldn't open navigate to link: {e.Message}", e);
            }

        }

        private static void ClickAddToBasket(IWebDriver driver)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);
                var addToBasket = driver.FindElement(By.CssSelector(".dJJJCD"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", addToBasket);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Couldn't click add to basket: {e.Message}", e);
            }

        }

        private static void ClickContinueToCheckout(IWebDriver driver)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                var continueToBasket = driver.FindElement(By.XPath("//*[@data-interaction='Continue to basket']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", continueToBasket);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Couldn't continue to checkout: {e.Message}", e);
            }

        }

        private static void GoToCheckout(IWebDriver driver)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                var goToCheckout = driver.FindElement(By.XPath("//*[@data-component='Button']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", goToCheckout);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Couldn't go to checkout: {e.Message}", e);
            }
        }

        private static void WhereToDeliver(IWebDriver driver)
        {
            try
            {
                Thread.Sleep(4000);
                var enterDeliveryLocation = driver.FindElement(By.XPath("//*[@placeholder='Enter town or postcode']"));
                enterDeliveryLocation.SendKeys("Eh545bg");
                Thread.Sleep(1000);
                enterDeliveryLocation.SendKeys(Keys.Tab);
                Thread.Sleep(500);
                enterDeliveryLocation.SendKeys(Keys.Tab);
                Thread.Sleep(500);
                enterDeliveryLocation.SendKeys(Keys.Enter);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Couldn't complete where to deliver form: {e.Message}", e);
            }
        }

        private static void SelectDeliveryDate(IWebDriver driver)
        {
            try
            {
                //selects earliest all day delivery slot, cheapest time to cost delivery slot!
                Thread.Sleep(4000);
                var deliveryDiv = driver.FindElements(By.ClassName("sc-sdtwF"));
                deliveryDiv[0].Click();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Couldn't find delivery date: {e.Message}", e);
            }

        }

        private static void EnterEmailAndCheckoutAsGuest(IWebDriver driver)
        {
            try
            {
                Thread.Sleep(4000);
                driver.FindElement(By.XPath("//*[@name='email']")).SendKeys("c.cheney@hotmail.co.uk");
                driver.FindElement(By.XPath("//button[.='Continue']")).Click();
                driver.FindElement(By.XPath("//a[.='Continue as a guest']")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Couldn't click checkout as guess: {e.Message}", e);
                throw;
            }

        }

        private static void EnterDetails(IWebDriver driver, BuyerModel buyer)
        {
            try
            {
                var title = driver.FindElement(By.XPath("//*[@name='d-title']"));
                Thread.Sleep(TimeSpan.FromMilliseconds(200));
                title.SendKeys(Keys.Tab);
                Thread.Sleep(TimeSpan.FromMilliseconds(200));
                title.SendKeys(Keys.Tab);
                Thread.Sleep(TimeSpan.FromMilliseconds(200));
                title.SendKeys(Keys.ArrowDown);
                Thread.Sleep(TimeSpan.FromMilliseconds(200));
                title.SendKeys(Keys.ArrowDown);
                title.SendKeys(Keys.Enter);
                title.SendKeys(Keys.Tab);

                var fname = driver.FindElement(By.XPath("//*[@name='fname']"));
                fname.SendKeys(buyer.Forename);

                var lname = driver.FindElement(By.XPath("//*[@name='lname']"));
                lname.SendKeys(buyer.Surname);

                var mobile = driver.FindElement(By.XPath("//*[@name='country-code phone']"));
                mobile.SendKeys(buyer.MobileNumber);

                var address = driver.FindElement(By.XPath("//*[@name='address']"));
                address.SendKeys(buyer.AddressLine1);

                var city = driver.FindElement(By.XPath("//*[@name='city']"));
                city.SendKeys(buyer.City);

                var postcode = driver.FindElement(By.XPath("//*[@name='zip']"));
                postcode.SendKeys(Keys.Backspace);
                postcode.SendKeys(Keys.Backspace);
                postcode.SendKeys(Keys.Backspace);
                postcode.SendKeys(Keys.Backspace);
                postcode.SendKeys(Keys.Backspace);
                postcode.SendKeys(Keys.Backspace);
                postcode.SendKeys(Keys.Backspace);
                postcode.SendKeys(Keys.Backspace);
                postcode.SendKeys(buyer.Postcode);

                var act = new Actions(driver);
                act.SendKeys(Keys.Tab).Build().Perform();
                act.SendKeys(Keys.Tab).Build().Perform();
                act.SendKeys(Keys.Tab).Build().Perform();
                act.SendKeys(Keys.Tab).Build().Perform();
                act.SendKeys(Keys.Tab).Build().Perform();
                act.SendKeys(Keys.Tab).Build().Perform();
                act.SendKeys(Keys.Enter).Build().Perform();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Couldn't fill out delivery details: {e.Message}", e);
                throw;
            }

        }

        private static void MakePayment(IWebDriver driver, BuyerModel buyer)
        {
            PlaySound();
            Thread.Sleep(9000);
            var postcode = driver.FindElement(By.XPath("//*[@data-element='ButtonText']"));
            //postcode.Click();
            //Console.WriteLine("");
        }

        private static void PlaySound()
        {
            using (var audioFile = new AudioFileReader("C:\\Users\\cchen\\Downloads\\Siren.mp3"))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
