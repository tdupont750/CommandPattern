using System;
using System.Threading.Tasks;
using CommandPattern.Core;
using Microsoft.Practices.Unity;

namespace CommandPattern.Runners
{
    public class UnityCommandRunner : ICommandRunner
    {
        private static readonly Type CommandType = typeof(ICommand<,>);
        
        private readonly IUnityContainer _container;

        public UnityCommandRunner(IUnityContainer container)
        {
            _container = container;
        }

        public Task<TResult> ExecuteAsync<TResult>(ICommandModel<TResult> model)
        {
            return Task.Run(() => Execute(model));
        }
        
        public TResult Execute<TResult>(ICommandModel<TResult> model)
        {
            var inputType = model.GetType();
            var resultType = typeof (TResult);
            var genericType = CommandType.MakeGenericType(inputType, resultType);

            dynamic command = _container.Resolve(genericType);
            dynamic dynamicModel = model;

            if (command == null)
            {
                var message = "No Command found for " + inputType.Name;
                throw new InvalidOperationException(message);
            }

            command.Validate(dynamicModel);
            return command.Execute(dynamicModel);
        }

    }
}