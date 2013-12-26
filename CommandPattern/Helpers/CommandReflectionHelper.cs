using System;
using System.Collections.Generic;
using System.Linq;
using CommandPattern.Core;

namespace CommandPattern.Helpers
{
    public static class CommandReflectionHelper
    {
        public static IDictionary<Type, Type> GetCommandTypes()
        {
            var map = new Dictionary<Type, Type>();
            var commands = GetCommands();

            foreach (var command in commands)
            {
                var from = GetCommandInterface(command);
                map.Add(from, command);
            }

            return map;
        }

        private static readonly Type CommandType = typeof(ICommand<,>);
        private static readonly Type ModelType = typeof(ICommandModel<>);

        private static IEnumerable<Type> GetCommands()
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

                    var i = GetCommandInterface(t);
                    return i != null;
                });
        }

        public static Type GetCommandInterface(Type type)
        {
            return GetInterfaceInterface(type, CommandType);
        }

        public static Type GetModelInterface(Type type)
        {
            return GetInterfaceInterface(type, ModelType);
        }

        private static Type GetInterfaceInterface(Type type, Type target)
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