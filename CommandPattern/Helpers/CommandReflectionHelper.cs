using System;
using System.Collections.Generic;
using System.Linq;
using CommandPattern.Core;

namespace CommandPattern.Helpers
{
    public static class CommandReflectionHelper
    {
        public static IDictionary<Type, Type> GetOperationMap()
        {
            var map = new Dictionary<Type, Type>();
            var operations = GetOperations();

            foreach (var operation in operations)
            {
                var command = GetOperationInterface(operation);
                map.Add(command, operation);
            }

            return map;
        }

        private static readonly Type OperationType = typeof(IOperation<,>);
        private static readonly Type CommandType = typeof(ICommand<>);

        private static IEnumerable<Type> GetOperations()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a =>
                {
                    try
                    {
                        return a.GetTypes();
                    }
                    catch
                    {
                        return Enumerable.Empty<Type>();
                    }
                })
                .Where(t =>
                {
                    if (!t.IsClass || t.IsAbstract)
                        return false;

                    var i = GetOperationInterface(t);
                    return i != null;
                });
        }

        public static Type GetOperationInterface(Type type)
        {
            return GetInterface(type, OperationType);
        }

        public static Type GetCommandInterface(Type type)
        {
            return GetInterface(type, CommandType);
        }

        private static Type GetInterface(Type type, Type target)
        {
            var interfaces = type.GetInterfaces();

            foreach (var i in interfaces)
            {
                if (!i.IsGenericType)
                    continue;

                var generic = i.GetGenericTypeDefinition();
                if (generic == target)
                    return i;
            }

            return null;
        }
    }
}