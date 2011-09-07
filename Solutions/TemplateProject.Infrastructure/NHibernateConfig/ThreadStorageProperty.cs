using System.Runtime.Remoting.Contexts;
using SharpArch.NHibernate;

namespace TemplateProject.Infrastructure.NHibernateConfig
{
    public class ThreadStorageProperty : IContextProperty
    {
        public ThreadStorageProperty(SimpleSessionStorage sessionStorage)
        {
            SessionStorage = sessionStorage;
        }

        public bool IsNewContextOK(Context newCtx)
        {
            return true;
        }

        public void Freeze(Context newContext)
        {
        }

        public string Name
        {
            get { return "ThreadStorageProperty"; }
        }

        public SimpleSessionStorage SessionStorage { get; set; }
    }
}
