namespace CommandPattern.Core
{
    public interface IOperation<in TInput, out TResult>
        where TInput : ICommand<TResult>
    {
        void Validate(TInput command);
        TResult Execute(TInput command);
    }
}