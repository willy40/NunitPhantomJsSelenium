using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace nunitphantom
{
    [Category("PostCodeValidation")]
    public class PostCodeTests : Page
    {
        #region Private
        private string postCodeGroup = ".your-postcode";
        private string postCodeField = "postcode-field";
        private string compareButton = "compare-button";
        private string gasButton = "//*[@id=\"splash\"]/div/div/div/div/div/div[1]/div[2]/div/div[2]/label";
        private string postCodeError = "//*[@id=\"splash\"]/div/div/div/div/div/div[2]/div/div/div[2]/ul/li";
        private string mpanAlertSelector = ".q-mpan .alert.alert-info";

        private string multipleMpansAddressValue = "2041496013d57b4d0828f0603dd6b9919670e8c3";
        private string firstAddressInMultipleMpansXpath = "//*[@id=\"multiple_meters\"]/div[1]/label/input";
        private string formContainerId = "form-container";
        private string addressFieldId = "address-field";

        private string wrongPostCode = "123";
        private string correctPostCode = "oX169BX";
        private string multipleMpansPostCode = "ky7 6lq";
        #endregion

        public PostCodeTests()
        {
            this.Url = "http://business.ukpower.co.uk/";
        }

        [Test]
        public void Entering_PostCode_Lower_Should_Replace_To_Upper()
        {
            EnterPostcode(multipleMpansPostCode);
            var elem = FindElementById(postCodeField);
            var upperCasePostCode = elem.GetAttribute("value");

            Assert.AreEqual("KY7 6LQ", upperCasePostCode);
        }

        [Test]
        public void InCorrect_PostCode_Error_Visible()
        {
            EnterPostcode(wrongPostCode, 50);

            Assert.AreEqual(true, FindElementByXpath(postCodeError).Displayed);
        }

        [Test]
        public void Correct_PostCode_Error_Invisible()
        {
            EnterPostcode(correctPostCode);

            Assert.AreEqual(false, FindElementByXpath(postCodeError).Displayed);
        }

        [Test]
        public void Correct_PostCode_Energy_Banner_Visible()
        {
            EnterPostcode(correctPostCode);

            Assert.AreEqual(true, FindElementById(formContainerId).Displayed);
        }

        [Test]
        public void Selecting_Multiple_Mpans_Noticed_Visible()
        {
            EnterPostcode(multipleMpansPostCode);
            ChooseValueInSelectById(addressFieldId, multipleMpansAddressValue);

            Assert.AreEqual(true, FindElementByCssSelector(mpanAlertSelector).Displayed);
        }

        [Test]
        public void Selecting_First_Mpan_In_Multiple_Mpans_Noticed_InVisible()
        {
            EnterPostcode(multipleMpansPostCode);
            ChooseValueInSelectById(addressFieldId, multipleMpansAddressValue);
            ClickByXpath(firstAddressInMultipleMpansXpath);

            Assert.AreEqual(false, FindElementByCssSelector(mpanAlertSelector).Displayed);
        }

        private void EnterPostcode(string _postCode, int sleep=2000)
        {
            var postcode = FindElementById(postCodeField);
            postcode.SendKeys(_postCode);

            ClickById(compareButton);
            Thread.Sleep(sleep);

        }
    }
}
