using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using TemplateProject.Tasks.Commands;

namespace TemplateProject.Tests.Tasks.Commands
{
    [TestFixture]
    public class MassCategoryChangeCommandTest
    {
        [Test]
        public void Constructor_Sets_Properties()
        {
            //Arrange
            const int catId = 4;
            var productIds = new List<int> {2, 3, 4};

            //Act
            var command = new MassCategoryChangeCommand(catId, productIds);

            //Assert
            Assert.AreEqual(4, command.CategoryId);
            Assert.AreEqual(3, command.ProductIds.Count());
        }
    }
}
