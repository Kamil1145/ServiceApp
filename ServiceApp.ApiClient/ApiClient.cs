using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.ApiClient
{
    public partial class ApiClient : IApiClientBase
    {

        public HttpClient httpClient 
        {
            get
            {
                return this._httpClient;
            }
            
            set
            { 
                _httpClient = httpClient;
            }
        }
    }
}
