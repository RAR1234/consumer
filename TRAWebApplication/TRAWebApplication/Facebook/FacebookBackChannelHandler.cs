using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace TRAWebApplication.Facebook
{
    public class FacebookBackChannelHandler:HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                if (!request.RequestUri.AbsolutePath.Contains("/oauth"))
                {
                    request.RequestUri = new Uri(request.RequestUri.AbsoluteUri.Replace("?access_token", "&access_token"));
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}