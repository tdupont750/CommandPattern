using System;
using System.Threading.Tasks;
using CommandPattern.Core;
using Microsoft.Practices.Unity;

namespace CommandPattern.Agents
{
    public class UnityAgent : IAgent
    {
        private static readonly Type OperationType = typeof(IOperation<,>);
        
        private readonly IUnityContainer _container;

        public UnityAgent(IUnityContainer container)
        {
            _container = container;
        }

        public Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            return Task.Run(() => Execute(command));
        }
        
        public TResult Execute<TResult>(ICommand<TResult> command)
        {
            var commandType = command.GetType();
            var resultType = typeof (TResult);
            var genericType = OperationType.MakeGenericType(commandType, resultType);

            dynamic operation = _container.Resolve(genericType);
            dynamic dynamicCommand = command;

            if (operation == null)
            {
                var message = "No operation found for command: " + commandType.Name;
                throw new InvalidOperationException(message);
            }

            operation.Validate(dynamicCommand);
            return operation.Execute(dynamicCommand);
        }

    }
}