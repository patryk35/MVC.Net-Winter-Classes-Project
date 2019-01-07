using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
namespace Selenium
{
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.Support.UI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Selenium
    {
        class Demo
        {


            IWebDriver driver;
            
            [SetUp]
            public void startBrowser()
            {
               // driver = new FirefoxDriver("C:\\tmp");
                driver = new ChromeDriver("C:\\tmp");
                driver.Navigate().GoToUrl("https://localhost:44333/");//"https://jobofferspmdevelop.azurewebsites.net/"); //navigate to page
                var query = driver.FindElement(By.Id("accept_cookie"));
                query.Click();

            }


            [Test]
            public void authorization()
            {


                var query = driver.FindElement(By.Id("loggInButton"));
                query.Click();

                query = Utils.FindElement(driver, By.Id("logonIdentifier"), 10);
                query.SendKeys("pat35@op.pl");

                query = driver.FindElement(By.Id("password"));
                query.SendKeys("!Qazxsw2");

                query = driver.FindElement(By.Id("next"));
                query.Click();

                //var cookies = driver.Manage().Cookies.AllCookies.ToList();
               // System.Diagnostics.Debug.WriteLine(cookies.ToString());
               // Utils.FindElement(driver, By.Id("loggOutButton"), 10);

                query = driver.FindElement(By.Id("loggOutButton"));
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                query.Click();
            }
            [Test]
            public void testSearch()
            {
                var query = driver.FindElement(By.Id("offers"));
                query.Click();

                query = driver.FindElement(By.Id("search"));
                query.SendKeys("Programista");

                query = driver.FindElement(By.Id("go"));
                query.Click();

                query = Utils.FindElement(driver, By.Id("offer_link"), 10);
                query.Click();
            }

            [Test]
            public void testAddCompany()
            {
                var query = driver.FindElement(By.Id("loggInButton"));
                query.Click();

                query = Utils.FindElement(driver, By.Id("logonIdentifier"), 10);
                query.SendKeys("pat35@op.pl");

                query = driver.FindElement(By.Id("password"));
                query.SendKeys("!Qazxsw2");

                query = driver.FindElement(By.Id("next"));
                query.Click();
         

                query = Utils.FindElement(driver, By.Id("m_companies"), 10);
                query.Click();

                query = Utils.FindElement(driver, By.Id("add"), 10);
                query.Click();

                query = Utils.FindElement(driver, By.Id("Name"), 10);
                query.SendKeys("Alpha2");

                query = driver.FindElement(By.ClassName("btn-primary"));
                query.Click();


            }


            [TearDown]
            public void closeBrowser()
            {
                
                driver.Close();
                Console.WriteLine("UI tests ended");
            }




        }
        static class Utils
        {
            public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
            {
                if (timeoutInSeconds > 0)
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                    return wait.Until(drv => drv.FindElement(by));
                }
                return driver.FindElement(by);
            }

            public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
            {
                if (timeoutInSeconds > 0)
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                    return wait.Until(drv => (drv.FindElements(by).Count > 0) ? drv.FindElements(by) : null);
                }
                return driver.FindElements(by);
            }
        }
    }
}
