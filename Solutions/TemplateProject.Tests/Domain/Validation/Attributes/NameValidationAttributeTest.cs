using System.Linq;
using MbUnit.Framework;
using TemplateProject.Domain.Validation;
using TemplateProject.Domain.Validation.Attributes;

namespace TemplateProject.Tests.Domain.Validation.Attributes
{
    [TestFixture]
    public class NameValidationAttributeTest
    {
        [Test]
        [Row(null, true)]
        [Row("", true)]
        [Row("High", true)]
        [Row("High School", true)]
        [Row("4323", true)]
        [Row("h", false)]
        [Row("high School", false)]
        [Row("High school", false)]
        [Row("high school", false)]
        public void IsValid_Checks_For_Capitalization(string toValidate, bool validated)
        {
            //Arrange
            var validator = new NameValidationAttribute();

            //Act
            var valid = validator.IsValid(toValidate);

            //Assert
            Assert.AreEqual(validated, valid);
        }

        [Test]
        public void Validation_Message_Is_Correct()
        {
            //Act
            var message = new NameValidationAttribute().FormatErrorMessage("PuppyDog");

            //Assert
            Assert.AreEqual("The first letter of each word must be capitalized for field PuppyDog", message);
        }

        [Test]
        public void GetClientValidationRules_Returns_Correct_Entry()
        {
            //Act
            var rules = new NameValidationAttribute().GetClientValidationRules(null, null);

            //Assert
            Assert.AreEqual(1, rules.Count());
            var rule = rules.SingleOrDefault();
            Assert.IsInstanceOfType<ModelClientNameValidationRule>(rule);
            Assert.AreEqual("The first letter of each word must be capitalized for this field", rule.ErrorMessage);
            Assert.AreEqual(1, rule.ValidationParameters.Count);
            Assert.AreEqual(NameValidationAttribute.CapsRegex, rule.ValidationParameters.SingleOrDefault().Value);
        }
    }
}
