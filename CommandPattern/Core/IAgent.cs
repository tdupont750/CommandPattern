using System.Threading.Tasks;

namespace CommandPattern.Core
{
    public interface IAgent
    {
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command);

        TResult Execute<TResult>(ICommand<TResult> command);
    }
}