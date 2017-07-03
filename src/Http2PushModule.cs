using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using ZDV.HTTP2PushModule.Configuration;

namespace ZDV.HTTP2PushModule
{
    public class Http2PushModule : IHttpModule
    {
        private PushGroupSection _section;

        private static readonly Dictionary<string, string[]> PushCache = new Dictionary<string, string[]>();
        
        public void Init(HttpApplication context)
        {
            _section = WebConfigurationManager.GetSection("Http2PushGroups") as PushGroupSection;
            if (_section == null)
                throw new ConfigurationErrorsException($"Section not found. Define <section name=\"Http2PushGroups\" type=\"{typeof(PushGroupSection).FullName}\" /> and provide <Http2PushGroups /> to use the Module.");

            context.PreSendRequestHeaders += Http2PushHandler;
        }

        
        private void Http2PushHandler(Object source, EventArgs e)
        {
            var context = source as HttpApplication;

            var requestUrl = context?.Request.AppRelativeCurrentExecutionFilePath;
            if (string.IsNullOrWhiteSpace(requestUrl))
                return;

            string[] pushUrls;
            if (PushCache.TryGetValue(requestUrl, out pushUrls))
            {
                PushFiles(context.Response, pushUrls);
                return;
            }

            var pushedElements = _section.PushGroup
                .OfType<PushGroupElement>()
                .Where(pge =>
                    pge.PushElements
                        .Cast<PushElement>()
                        .Any(pe => pe.Triggers && requestUrl.Equals(pe.Url, StringComparison.OrdinalIgnoreCase)))
                .SelectMany(pge => pge.PushElements.Cast<PushElement>().Select(pe => pe.Url))
                .ToList();

            pushUrls = pushedElements
                .Where(pe => pe != requestUrl)
                .ToArray();
            
            lock (PushCache)
            {
                if (PushCache.ContainsKey(requestUrl))
                    return;
                
                PushCache.Add(requestUrl, pushUrls);
            }
        }

        private void PushFiles(HttpResponse httpResponse, string[] pushUrls)
        {
            foreach(var pushUrl in pushUrls)
                httpResponse.PushPromise(pushUrl);
        }

        public void Dispose()
        {
        }
    }
}
