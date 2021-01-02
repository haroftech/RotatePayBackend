using Microsoft.AspNetCore.Http;

namespace Backend.Helpers
{
    public static class IPAddress
    {
        public static string GetIpAddress(HttpRequest httpRequest)
        {
            string ipAddressString = string.Empty;
            if (httpRequest == null)
            {
                return ipAddressString;
            }
            if (httpRequest.Headers != null && httpRequest.Headers.Count > 0)
            {
                if (httpRequest.Headers.ContainsKey("X-Forwarded-For") == true)
                {
                    string headerXForwardedFor = httpRequest.Headers["X-Forwarded-For"];
                    if (string.IsNullOrEmpty(headerXForwardedFor) == false)
                    {
                        string xForwardedForIpAddress = headerXForwardedFor.Split(':')[0];
                        if (string.IsNullOrEmpty(xForwardedForIpAddress) == false)
                        {
                            ipAddressString = xForwardedForIpAddress;
                        }
                    }
                } else if (httpRequest.HttpContext.Connection.RemoteIpAddress != null) {
                    ipAddressString = httpRequest.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                }
            }
            else if (httpRequest.HttpContext.Connection.RemoteIpAddress != null)
            {
                ipAddressString = httpRequest.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            return ipAddressString;
        }
    }
}
