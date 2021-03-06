﻿namespace nunitphantom
{
    #region Usings

    using NUnit.Framework;
    using System.Threading;

    #endregion

    [TestFixture]
    [Category("PostCodeValidation")]
    public class PostCodeTests : Page
    {
        #region Private
        private string postCodeFieldId = "postcode-field";
        private string compareButton = "compare-button";
        private string postCodeError = "//*[@id=\"splash\"]/div/div/div/div/div/div[2]/div/div/div[2]/ul/li";
        private string multipleMpansAlert = ".q-mpan .alert.alert-info";

        private string multipleMpansAddressValue = "2041496013d57b4d0828f0603dd6b9919670e8c3";
        private string firstAddressInMultipleMpansXpath = "//*[@id=\"multiple_meters\"]/div[1]/label/input";
        private string formContainerId = "form-container";
        private string addressFieldId = "address-field";

        private string wrongPostCode = "123";
        private string correctPostCodeUpperCase = "OX169BX";
        private string multipleMpansPostCodeLowerCase = "ky7 6lq";
        #endregion

        #region Constructor
        public PostCodeTests()
        {
            this.Url = "http://business.ukpower.co.uk/";
        }

        #endregion

        #region Tests
        [Test]
        public void Entering_PostCode_Lower_Should_Replace_To_Upper()
        {
            EnterPostcode(multipleMpansPostCodeLowerCase);
            var upperCasePostCode = FindElementById(postCodeFieldId).GetAttribute("value");

            Assert.AreEqual(multipleMpansPostCodeLowerCase.ToUpper(), upperCasePostCode);
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
            EnterPostcode(correctPostCodeUpperCase);

            Assert.AreEqual(false, FindElementByXpath(postCodeError).Displayed);
        }

        [Test]
        public void Correct_PostCode_Energy_Banner_Visible()
        {
            EnterPostcode(correctPostCodeUpperCase);

            Assert.AreEqual(true, FindElementById(formContainerId).Displayed);
        }

        [Test]
        public void Selecting_Multiple_Mpans_Noticed_Visible()
        {
            EnterPostcode(multipleMpansPostCodeLowerCase);
            ChooseValueInSelectById(addressFieldId, multipleMpansAddressValue);

            Assert.AreEqual(true, FindElementByCssSelector(multipleMpansAlert).Displayed);
        }

        [Test]
        public void Selecting_First_Mpan_In_Multiple_Mpans_Noticed_InVisible()
        {
            EnterPostcode(multipleMpansPostCodeLowerCase);
            ChooseValueInSelectById(addressFieldId, multipleMpansAddressValue);
            ClickByXpath(firstAddressInMultipleMpansXpath);

            Assert.AreEqual(false, FindElementByCssSelector(multipleMpansAlert).Displayed);
        }
        #endregion

        #region Private Methods

        private void EnterPostcode(string _postCode, int sleep=2000)
        {
            var postcode = FindElementById(postCodeFieldId);
            postcode.SendKeys(_postCode);

            ClickById(compareButton);
            Thread.Sleep(sleep);
        }
        #endregion
    }
}
