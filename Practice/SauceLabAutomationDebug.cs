using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SauceLabsAutomation._Android.Tests;
using SauceLabsAutomation._IOS.Tests;

namespace TestUtilities.Practice
{

    //Refernce link: https://wiki.saucelabs.com/display/DOCS/Platform+Configurator#/
    class SauceLabAutomationAndroidDebug : AndroidTestBase
    {
        public void verifyAndroidWebTest()
        {
            androidDriver = createDriver("1.5.3", "Android Emulator",
                "phone", "portrait", "4.4", "Android", "Browser", "", "verifyAndroidLottoHomepageTest");
            androidDriver.Navigate().GoToUrl("https://m.mylotto.co.nz/");
        }


        //Sure that the apk is uploaded
        //curl  -u lottotestnz:4441f476-39c5-419f-9b4b-437e8323d461 -X POST -H "Content-Type: application/octet-stream" https://saucelabs.com/rest/v1/storage/lottotestnz/mylotto-cat1.apk?overwrite=true --data-binary @C:/apks/mylotto-cat1.apk
        public void verifyAndroidAppTest()
        {
            createDriver("1.5.3", "Android Emulator",
                "phone", "portrait", "4.4", "Android", "", "sauce-storage:mylotto-cat1.apk", "verifyAndroidLottoHomepageTest");
            //androidDriver.Navigate().GoToUrl("https://m.mylotto.co.nz/");
        }
    }

    class SauceLabAutomationIOSDebug : IOSTestBase
    {
        public void verifyIOSWebTest()
        {
            IOSDriver = createDriver("1.5.3", "iPhone 6 Plus", null,
                "portrait", "9.3", "iOS", "Safari", null, "verifyIOSWebTest");
            IOSDriver.Navigate().GoToUrl("https://m.mylotto.co.nz/");
        }

        //Sure that the apk is uploaded
        //curl  -u lottotestnz:4441f476-39c5-419f-9b4b-437e8323d461 -X POST -H "Content-Type: application/octet-stream" https://saucelabs.com/rest/v1/storage/lottotestnz/SampleApp.zip?overwrite=true --data-binary @C:/apks/SampleApp.zip
        //LottoNZ.zip
        //SampleApp.zip
        public void verifyIOSAppTest()
        {
            IOSDriver = createDriver("1.5.3", "iPhone Simulator", null,
                "portrait", "9.3", "iOS", "", "sauce-storage:LottoNZ.zip", "verifyIOSAppTest");
        }

    }

}
