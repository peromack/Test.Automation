﻿/*
The MIT License (MIT)

Copyright (c) 2015 Objectivity Bespoke Software Specialists

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace Objectivity.Test.Automation.Tests.PageObjects.PageObjects.TheInternet
{
    using System;
    using System.Globalization;
    using System.IO;

    using NLog;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Helpers;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Tests.PageObjects;

    public class DownloadPage : ProjectPageBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Locators for elements
        /// </summary>
        private readonly ElementLocator downloadPageHeader = new ElementLocator(Locator.XPath, "//h3[.='File Downloader']"),
                                        fileLink = new ElementLocator(Locator.CssSelector, "a[href='download/{0}']");

        public DownloadPage(DriverContext driverContext)
            : base(driverContext)
        {
            Logger.Info("Waiting for File Download page to open");
            this.Driver.IsElementPresent(this.downloadPageHeader, BaseConfiguration.ShortTimeout);
        }

        public DownloadPage SaveFile(string fileName, string newName)
        {
            if (BaseConfiguration.TestBrowser == DriverContext.BrowserType.Firefox
                || BaseConfiguration.TestBrowser == DriverContext.BrowserType.Chrome)
            {
                this.Driver.GetElement(this.fileLink.Evaluate(fileName), "Click on file").Click();
                FilesHelper.WaitForFileOfGivenName(fileName, this.DriverContext.DownloadFolder);
                FilesHelper.RenameFile(fileName, newName, this.DriverContext.DownloadFolder, FileType.Csv);
            }
            else
            {
               Logger.Info(CultureInfo.CurrentCulture, "Downloading files in browser {0} is not supported", BaseConfiguration.TestBrowser);
            }

            return this;
        }

        public DownloadPage SaveFile(string newName)
        {
           if (BaseConfiguration.TestBrowser == DriverContext.BrowserType.Firefox
                || BaseConfiguration.TestBrowser == DriverContext.BrowserType.Chrome)
            {
                var filesNumber = FilesHelper.CountFiles(this.DriverContext.DownloadFolder, FileType.Txt);
                this.Driver.GetElement(this.fileLink.Evaluate("some-file.txt")).Click();
                FilesHelper.WaitForFileOfGivenType(FileType.Txt, filesNumber, this.DriverContext.DownloadFolder);
                FileInfo file = FilesHelper.GetLastFile(this.DriverContext.DownloadFolder, FileType.Txt);
                FilesHelper.RenameFile(file.Name, newName, this.DriverContext.DownloadFolder, FileType.Txt);
            }
            else
            {  
                Logger.Info(CultureInfo.CurrentCulture, "Downloading files in browser {0} is not supported", BaseConfiguration.TestBrowser);
            }

            return this;
        }

        public DownloadPage SaveAnyFile()
        {
            if (BaseConfiguration.TestBrowser == DriverContext.BrowserType.Firefox
                 || BaseConfiguration.TestBrowser == DriverContext.BrowserType.Chrome)
            {
                var filesNumber = FilesHelper.CountFiles(this.DriverContext.DownloadFolder);
                this.Driver.GetElement(this.fileLink.Evaluate("some-file.txt")).Click();
                FilesHelper.WaitForFile(filesNumber, this.DriverContext.DownloadFolder);
                FileInfo file = FilesHelper.GetLastFile(this.DriverContext.DownloadFolder);
                FilesHelper.RenameFile(BaseConfiguration.ShortTimeout,file.Name, "name_of_file_branch.txt", this.DriverContext.DownloadFolder);
            }
            else
            {
                Logger.Info(CultureInfo.CurrentCulture, "Downloading files in browser {0} is not supported", BaseConfiguration.TestBrowser);
            }

            return this;
        }

        public string CheckIfScreenShotIsSaved(int screenShotNumber, string newNameOfFile)
        {
            FilesHelper.WaitForFileOfGivenType(FileType.Png, 5, screenShotNumber, this.DriverContext.ScreenShotFolder);
            var nameOfFile = FilesHelper.GetLastFile(this.DriverContext.ScreenShotFolder, FileType.Png).Name;
            FilesHelper.RenameFile(5, FilesHelper.GetLastFile(this.DriverContext.ScreenShotFolder, FileType.Png).Name,  newNameOfFile, this.DriverContext.ScreenShotFolder);
            return nameOfFile;
        }

        public void SaveWebDriverScreenShot()
        {
            this.DriverContext.SaveScreenshot(new ErrorDetail(this.DriverContext.TakeScreenshot(), DateTime.Now, null), this.DriverContext.ScreenShotFolder, this.DriverContext.TestTitle);
        }     
    }
}
