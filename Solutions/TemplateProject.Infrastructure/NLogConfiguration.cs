
using NLog;
using NLog.Config;
using NLog.Targets;

namespace TemplateProject.Infrastructure
{
    public static class NLogConfiguration
    {
        public static LoggingConfiguration CreateConfig()
        {
            var config = new LoggingConfiguration();

            ConfigureFileLogger(config);
            ConfigureEmailLogger(config);

            return config;
        }

        private static void ConfigureEmailLogger(LoggingConfiguration config)
        {
            var emailTarget = new MailTarget();
            config.AddTarget("mail", emailTarget);

            emailTarget.Body = "${longdate} ${message} ${newline} ${onexception:EXCEPTION OCCURRED\\:${exception:format=tostring}}";
            emailTarget.From = "TemplateProject@gosquids.com";
            emailTarget.Html = false;
            emailTarget.Subject = "TemplateProject .Net Log Error ${date:format=yyyyMMddHHmmss}";
            emailTarget.To = "mitch@gosquids.com";

            emailTarget.SmtpServer = "smtp.gmail.com";
            emailTarget.SmtpPort = 587;
            emailTarget.SmtpAuthentication = SmtpAuthenticationMode.Basic;
            emailTarget.SmtpUserName = "squidlogger@gmail.com";
            emailTarget.SmtpPassword = "squidpassword";
            emailTarget.EnableSsl = true;

            var emailRule = new LoggingRule("*", LogLevel.Error, emailTarget);
            config.LoggingRules.Add(emailRule);
        }

        private static void ConfigureFileLogger(LoggingConfiguration config)
        {
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            fileTarget.FileName = "${basedir}/filelog.log";
            fileTarget.Layout = "${longdate} ${message} ${onexception:EXCEPTION OCCURRED\\:${exception:format=tostring}}";

            var fileRule = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(fileRule);
        }
    }
}
