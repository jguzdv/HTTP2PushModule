using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace HTTP2PushModule
{
    public class Http2PushModule : IHttpModule
    {
        private PushGroupSection _section;
        public void Init(HttpApplication context)
        {
            _section = WebConfigurationManager.GetSection("PushGroupSection") as PushGroupSection;

            if (_section == null)
                return;

            context.PreSendRequestHeaders += Http2PushHandler;
        }

        private void Http2PushHandler(Object source, EventArgs e)
        {
            var context = source as HttpApplication;
            if (context == null)
                return;

            var requestUrl = context.Request.AppRelativeCurrentExecutionFilePath;
            if (string.IsNullOrWhiteSpace(requestUrl))
                return;

            foreach (PushGroupElement pge in _section.PushGroup)
            {
                foreach (PushElement pe in pge.PushElements)
                {
                    if (!(pe.Triggers && requestUrl.Equals(pe.Url)))
                        continue;

                    foreach (PushElement innerPe in pge.PushElements)
                    {
                        if (innerPe.Url.Equals(pe.Url))
                            continue;

                        context.Response.PushPromise(innerPe.Url);
                    }
                    break;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
