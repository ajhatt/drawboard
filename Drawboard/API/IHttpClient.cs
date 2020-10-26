using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace Drawboard.API
{
    /// <summary>
    /// Http client interface.
    /// 
    /// The interface exists to make clients of this interface testable.
    /// </summary>
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(System.Uri uri);

        Task<IBuffer> GetBufferAsync(Uri uri);
    }
}
