using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CommandPattern.Core;
using CommandPattern.Helpers;

namespace CommandPattern.Controllers
{
    public class CommandController : Controller
    {
        private static readonly IDictionary<string, Type> Map = CommandReflectionHelper
            .GetOperationMap()
            .Select(p => p.Key.GetGenericArguments().First())
            .ToDictionary(
                k => k.Name,
                v => v,
                StringComparer.InvariantCultureIgnoreCase);

        private readonly IAgent _agent;

        public CommandController(IAgent agent)
        {
            _agent = agent;
        }
        
        public ActionResult Execute(string commandName)
        {
            Type type;

            if (Map.ContainsKey(commandName))
                type = Map[commandName];
            else if (Map.ContainsKey(commandName + "Command"))
                type = Map[commandName + "Command"];
            else
                throw new ArgumentException("Invalid Command Name: " + commandName, "commandName");

            dynamic command = Activator.CreateInstance(type);
            UpdateModel(command);

            var result = _agent.Execute(command);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }

}
