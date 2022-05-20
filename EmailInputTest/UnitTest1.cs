using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using demoTest.Classes;

namespace EmailInputTest
{
    [TestClass]
    public class EmailTest
    {
        [TestMethod]
        public void TestCorrectEmail()
        {
            var email = "nicnamezicromain@gmail.com";

            bool actual = ConstraintsClass.EmailIsValid(email);

            Assert.IsTrue(actual); 
        }

        [TestMethod]
        public void TestIncorrectEmail()
        {
            var email = "notmailgmail com";

            bool actual = ConstraintsClass.EmailIsValid(email);

            Assert.IsFalse(actual);
        }


    }
}
