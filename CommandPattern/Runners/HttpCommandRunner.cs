using System;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using CommandPattern.Core;
using Newtonsoft.Json;

namespace CommandPattern.Runners
{
    public class HttpCommandRunner : ICommandRunner
    {
        private readonly string _baseAddress;
        
        private static string GetQueryString<T>(T model)
        {
            var properties = from p in model.GetType().GetProperties()
                             let v = p.GetValue(model, null)
                             where v != null
                             select p.Name + "=" + HttpUtility.UrlEncode(v.ToString());

            return String.Join("&", properties.ToArray());
        }

        public HttpCommandRunner(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public async Task<TResult> ExecuteAsync<TResult>(ICommandModel<TResult> model)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_baseAddress)
            };

            var type = model.GetType();
            var query = GetQueryString(model);
            var getResult = client.GetStringAsync(type.Name + "?" + query);

            await getResult;

            return JsonConvert.DeserializeObject<TResult>(getResult.Result);
        }

        public TResult Execute<TResult>(ICommandModel<TResult> model)
        {
            var result = ExecuteAsync(model);

            result.Wait();

            return result.Result;
        }
    }
}
