namespace CommandPattern.Core
{
    public interface ICommand<in TInput, out TResult>
        where TInput : ICommandModel<TResult>
    {
        void Validate(TInput nameModel);
        TResult Execute(TInput nameModel);
    }
}