using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroservicesNET.Platform
{
    public interface IHttpClientFactory: IDisposable
    {
        Task<HttpClient> Create(Uri uri, string scope);
    }
}
