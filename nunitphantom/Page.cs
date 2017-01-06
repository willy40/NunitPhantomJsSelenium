namespace nunitphantom
{
    #region Usings

    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.PhantomJS;
    using OpenQA.Selenium.Support.UI;
    using System;
    using System.Drawing.Imaging;
    using System.Text;

    #endregion

    public class Page
    {
        #region Protected Properties
        protected string Url { get; set; }

        #endregion

        #region Private Properties

        private static int WaitTime = 5;
        private PhantomJSDriver driver;
        #endregion

        #region Public Methods
        [OneTimeSetUp]
        public void ClassSetup()
        {
            driver = new PhantomJSDriver();
            driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 40));
        }

        [SetUp]
        public void SetUp()
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

        #endregion

        #region Protected Methods
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

        protected void ClickById(string elemId)
        {
            var domElement = FindElementById(elemId);
            domElement.Click();
        }

        protected void ClickByCss(string elemCss)
        {
            var domElement = getWait().Until(driver => driver.FindElement(By.CssSelector(elemCss)));
            domElement.Click();
        }

        protected void ClickByXpath(string elemXPath)
        {
            var domElement = this.FindElementByXpath(elemXPath);
            domElement.Click();
        }

        protected IWebElement FindElementByXpath(string XPath)
        {
            return getWait().Until(driver => driver.FindElement(By.XPath(XPath)));
        }

        protected IWebElement FindElementById(string elemId)
        {
            return getWait().Until(driver => driver.FindElement(By.Id(elemId)));
        }

        protected IWebElement FindElementByCssSelector(string selector)
        {
            return getWait().Until(driver => driver.FindElement(By.CssSelector(selector)));
        }

        protected void ChooseValueInSelectById(string elemId, string value)
        {
            var select = new SelectElement(FindElementById(elemId));
            select.SelectByValue(value);
        }

        protected WebDriverWait getWait()
        {
            return new WebDriverWait(new SystemClock(), 
                driver, 
                TimeSpan.FromSeconds(WaitTime), 
                TimeSpan.FromMilliseconds(500) );
        }
        #endregion
    }
}
