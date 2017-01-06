using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nunitphantom
{
    [TestFixture]
    public class Page
    {
        private static int WaitTime = 5;
        protected PhantomJSDriver driver;

        public string Url { get; set; }

        [OneTimeSetUp]
        public void ClassSetup()
        {
            driver = new PhantomJSDriver();
            driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 40));
        }

        [SetUp]
        public void Setup()
        {
            if (string.IsNullOrEmpty(Url))
                throw new Exception("URL Cannot be empty");

            driver.Navigate().GoToUrl(Url);
            driver.ExecuteScript("$.fx.off = true");
        }

        [OneTimeTearDown]
        public void ClassTearDown()
        {
            driver.Quit();
        }

        protected string CombineCssSelector(string _base, string _selector)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0} {1}", _base, _selector);
            return builder.ToString();
        }

        protected void ScreenShot()
        {
            Screenshot sh = driver.GetScreenshot();
            sh.SaveAsFile(@"C:\Temp.jpg", ImageFormat.Png);
        }

        public void ClickById(string elemId)
        {
            var domElement = FindElementById(elemId);
            domElement.Click();
        }

        public void ClickByCss(string elemCss)
        {
            var domElement = getWait().Until(driver => driver.FindElement(By.CssSelector(elemCss)));
            domElement.Click();
        }

        public void ClickByXpath(string elemXPath)
        {
            var domElement = this.FindElementByXpath(elemXPath);
            domElement.Click();
        }
        
        public IWebElement FindElementByXpath(string XPath)
        {
            return getWait().Until(driver => driver.FindElement(By.XPath(XPath)));
        }

        public IWebElement FindElementById(string elemId)
        {
            return getWait().Until(driver => driver.FindElement(By.Id(elemId)));
        }

        public IWebElement FindElementByCssSelector(string selector)
        {
            return getWait().Until(driver => driver.FindElement(By.CssSelector(selector)));
        }

        public void ChooseValueInSelectById(string elemId, string value)
        {
            var select = new SelectElement(FindElementById(elemId));
            select.SelectByValue(value);
        }

        private WebDriverWait getWait()
        {
            return new WebDriverWait(new SystemClock(), 
                driver, 
                TimeSpan.FromSeconds(WaitTime), 
                TimeSpan.FromMilliseconds(500) );
        }
    }
}
