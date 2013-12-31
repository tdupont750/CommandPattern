using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using CommandPattern.Core;
using Newtonsoft.Json;

namespace CommandPattern.Agents
{
    public class HttpAgent : IAgent
    {
        private readonly string _baseAddress;
        
        private static string GetQueryString<T>(T command)
        {
            var properties = from p in command.GetType().GetProperties()
                             let v = p.GetValue(command, null)
                             where v != null
                             select p.Name + "=" + HttpUtility.UrlEncode(v.ToString());

            return String.Join("&", properties.ToArray());
        }

        public HttpAgent(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_baseAddress)
            };

            var type = command.GetType();
            var query = GetQueryString(command);
            var getResult = client.GetStringAsync(type.Name + "?" + query);

            await getResult;

            return JsonConvert.DeserializeObject<TResult>(getResult.Result);
        }

        public TResult Execute<TResult>(ICommand<TResult> command)
        {
            var result = ExecuteAsync(command);

            result.Wait();

            return result.Result;
        }
    }
}
