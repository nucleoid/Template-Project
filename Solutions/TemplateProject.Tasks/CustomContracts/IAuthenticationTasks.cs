
namespace TemplateProject.Tasks.CustomContracts
{
    public interface IAuthenticationTasks
    {
        void SetAuthCookie(string userName, bool createPersistentCookie);
        void SignOut();
    }
}
