using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using NHibernate;
using SharpArch.NHibernate;

namespace TemplateProject.Infrastructure.NHibernateConfig
{
    /// <summary>
    /// Storage solution to allow request based storage for regular web requests, 
    /// and thread storage for things like quartz jobs that don't have an httpcontext
    /// </summary>
    public class ThreadAndWebSessionStorage : ISessionStorage
    {
        private const string HttpContextSessionStorageKey = "HttpContextSessionStorageKey";
        public static LocalDataStoreSlot Slot = Thread.AllocateNamedDataSlot("ThreadContextSessionStorageKey");

        public ThreadAndWebSessionStorage(HttpApplication app)
        {
            if(app != null)
                app.EndRequest += Application_EndRequest;
        }

        public IEnumerable<ISession> GetAllSessions()
        {
            return GetSessionStorage().GetAllSessions();
        }

        public ISession GetSessionForKey(string factoryKey)
        {
            var storage = GetSessionStorage();
            return storage.GetSessionForKey(factoryKey);
        }

        public void SetSessionForKey(string factoryKey, ISession session)
        {
            var storage = GetSessionStorage();
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

        private static ISessionStorage GetSessionStorage()
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

        private static ISessionStorage GetStorageForThread()
        {
            var simpleSessionStorage = Thread.GetData(Slot) as SimpleSessionStorage;
            if (simpleSessionStorage == null)
            {
                simpleSessionStorage = new SimpleSessionStorage();
                Thread.SetData(Slot, simpleSessionStorage);
            }
            return simpleSessionStorage;
        }
    }
}
