using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeopleManager.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeopleManager.Tests.UnitTests
{
    [TestClass]
    public class PersonModelTests
    {
        private IList<ValidationResult> Validate(object model)
        {
            var ctx = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, ctx, results, validateAllProperties: true);
            return results;
        }

        [TestMethod]
        public void Person_RequiresFullName()
        {
            var p = new Person { FullName = null, Email = "a@b.com" };
            var results = Validate(p);
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(System.Linq.Enumerable.Any(results, r => r.MemberNames != null && System.Linq.Enumerable.Contains(r.MemberNames, "FullName")));
        }

        [TestMethod]
        public void Person_EmailMustBeValid()
        {
            var p = new Person { FullName = "Test", Email = "not-an-email" };
            var results = Validate(p);
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(System.Linq.Enumerable.Any(results, r => r.MemberNames != null && System.Linq.Enumerable.Contains(r.MemberNames, "Email")));
        }

        [TestMethod]
        public void Person_ValidModel_PassesValidation()
        {
            var p = new Person { FullName = "Ronen Feinstein", Email = "ronen@example.com" };
            var results = Validate(p);
            Assert.AreEqual(0, results.Count);
        }
    }
}
