using System.Threading.Tasks;

namespace CommandPattern.Core
{
    public interface ICommandRunner
    {
        Task<TResult> ExecuteAsync<TResult>(ICommandModel<TResult> model);

        TResult Execute<TResult>(ICommandModel<TResult> model);
    }
}