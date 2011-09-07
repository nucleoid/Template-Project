using NLog;
using Quartz;
using TemplateProject.Domain.Contracts.Tasks;

namespace TemplateProject.Infrastructure.Quartz.Jobs
{
    public class OddJob : IJob
    {
        public IProductTasks ProductTasks { get; set; }
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void Execute(JobExecutionContext context)
        {
            var allProducts = ProductTasks.GetAll();
            logger.Debug(string.Format("There are {0} products in the database", allProducts.Count));
        }
    }
}
