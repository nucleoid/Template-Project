using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using NHibernate;
using SharpArch.NHibernate;

namespace TemplateProject.Infrastructure.NHibernateConfig
{
    public class ThreadAndWebSessionStorage : ISessionStorage
    {
        private const string HttpContextSessionStorageKey = "HttpContextSessionStorageKey";
        private const string ThreadContextSessionStorageKey = "ThreadContextSessionStorageKey";

        public ThreadAndWebSessionStorage(HttpApplication app)
        {
            if(app != null)
                app.EndRequest += Application_EndRequest;
        }

        public IEnumerable<ISession> GetAllSessions()
        {
            return GetSimpleSessionStorage().GetAllSessions();
        }

        public ISession GetSessionForKey(string factoryKey)
        {
            var storage = GetSimpleSessionStorage();
            return storage.GetSessionForKey(factoryKey);
        }

        public void SetSessionForKey(string factoryKey, ISession session)
        {
            var storage = GetSimpleSessionStorage();
            storage.SetSessionForKey(factoryKey, session);
        }

        private static void Application_EndRequest(object sender, EventArgs e)
        {
            if (NHibernateSession.Storage == null)
                return;
            var session = NHibernateSession.Storage.GetSessionForKey(NHibernateSession.DefaultFactoryKey);
            if (session != null && session.IsOpen)
                session.Close();

            HttpContext.Current.Items.Remove(HttpContextSessionStorageKey);
        }

        private static SimpleSessionStorage GetSimpleSessionStorage()
        {
            HttpContext current = HttpContext.Current;

            if(current != null)
            {
                var simpleSessionStorage = current.Items[HttpContextSessionStorageKey] as SimpleSessionStorage;
                if (simpleSessionStorage == null)
                {
                    simpleSessionStorage = new SimpleSessionStorage();
                    current.Items[HttpContextSessionStorageKey] = simpleSessionStorage;
                }
                return simpleSessionStorage;
            }
            return GetStorageForThread();
        }

        private static SimpleSessionStorage GetStorageForThread()
        {
            var simpleSessionStorage = Thread.CurrentContext.GetProperty(ThreadContextSessionStorageKey) as ThreadStorageProperty;
            if (simpleSessionStorage == null)
            {
                simpleSessionStorage = new ThreadStorageProperty(new SimpleSessionStorage());
                Thread.CurrentContext.SetProperty(simpleSessionStorage);
            }
            return simpleSessionStorage.SessionStorage;
        }
    }
}
