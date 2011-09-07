
using System.Linq;
using MbUnit.Framework;
using TemplateProject.Domain.Validation;

namespace TemplateProject.Tests.Domain.Validation
{
    [TestFixture]
    public class ModelClientNameValidationRuleTest
    {
        [Test]
        public void Constructor_Sets_Properties()
        {
            //Arrange
            const string errorMessage = "blah";
            const string regex = "asdf";

            //Act
            var rule = new ModelClientNameValidationRule(errorMessage, regex);

            //Assert
            Assert.AreEqual(errorMessage, rule.ErrorMessage);
            Assert.AreEqual("namefirstlettercaps", rule.ValidationType);
            Assert.AreEqual(1, rule.ValidationParameters.Count);
            Assert.AreEqual("letterregex", rule.ValidationParameters.SingleOrDefault().Key);
            Assert.AreEqual(regex, rule.ValidationParameters.SingleOrDefault().Value);
        }
    }
}
