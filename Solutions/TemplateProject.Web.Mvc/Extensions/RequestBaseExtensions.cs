
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using MvpRestApiLib;

namespace TemplateProject.Web.Mvc.Extensions
{
    public static class RequestBaseExtensions
    {
        private static string[] _restfulAcceptedTypes;
        public static string[] RestfulAcceptedTypes
        {
            get
            {
                if(_restfulAcceptedTypes == null)
                {
                    var restfulAttributes = typeof(EnableRestAttribute).Assembly.GetTypes().Where(type =>
                        typeof(EnableRestAttribute).IsAssignableFrom(type) && !type.IsAbstract).ToList();
                    var types = new List<string>();
                    foreach (Type attr in restfulAttributes)
                    {
                        ConstructorInfo info = attr.GetConstructor(new Type[0]);
                        var attribute = info.Invoke(null) as EnableRestAttribute;
                        types.AddRange(attribute.AcceptedTypes);
                    }
                    _restfulAcceptedTypes = types.ToArray();
                }
                return _restfulAcceptedTypes;
            }
        }

        public static bool IsRestful(this HttpRequestBase requestBase)
        {
            var acceptTypes = requestBase.AcceptTypes ?? new[] { "text/html" };
            return RestfulAcceptedTypes.Any(acceptTypes.Contains);
        }
    }
}