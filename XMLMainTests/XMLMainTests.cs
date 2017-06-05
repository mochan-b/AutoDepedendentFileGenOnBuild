using Microsoft.VisualStudio.TestTools.UnitTesting;
using XMLMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLMain;

namespace XMLMain.Tests
{
    [TestClass()]
    public class XMLMainTests
    {
        [TestMethod()]
        public void loadDataTest()
        {
            // Load the data
            var data = XMLMain.loadData();

            // Check that the values are read in correctly
            Assert.AreEqual("Alice", data[0].FirstName);
            Assert.AreEqual("Adams", data[0].LastName);
            Assert.AreEqual(100, data[0].Salary);

            Assert.AreEqual("Emma", data[4].FirstName);
            Assert.AreEqual("Ester", data[4].LastName);
            Assert.AreEqual(75, data[4].Salary);
        }
    }
}