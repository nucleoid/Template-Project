
using System.Linq;
using MbUnit.Framework;
using NHibernate;
using Rhino.Mocks;
using TemplateProject.Infrastructure.NHibernateConfig;

namespace TemplateProject.Tests.Infrastructure.NHibernateConfig
{
    [TestFixture]
    public class ThreadAndWebSessionStorageTest
    {
        [Test]
        public void GetAllSessions_Returns_All_Sessions()
        {
            //Arrange
            var storage = new ThreadAndWebSessionStorage(null);
            storage.SetSessionForKey("blah", MockRepository.GenerateStub<ISession>());
            storage.SetSessionForKey("blahs", MockRepository.GenerateStub<ISession>());

            //Act
            var sessions = storage.GetAllSessions();

            //Assert
            Assert.AreEqual(2, sessions.Count());
        }

        [Test]
        public void GetSessionForKey_Returns_Correct_Session()
        {
            //Arrange
            var storage = new ThreadAndWebSessionStorage(null);
            storage.SetSessionForKey("blah", null);
            storage.SetSessionForKey("blahs", MockRepository.GenerateStub<ISession>());
            storage.SetSessionForKey("blahed", null);

            //Act
            var session = storage.GetSessionForKey("blahs");
            
            Assert.IsNotNull(session);
        }

        [Test]
        public void SetSessionForKey_Sets_Session()
        {
            //Arrange
            var storage = new ThreadAndWebSessionStorage(null);

            //Act
            storage.SetSessionForKey("blah", MockRepository.GenerateStub<ISession>());

            //Assert
            var session = storage.GetSessionForKey("blah");
            Assert.IsNotNull(session);
        }
    }
}
