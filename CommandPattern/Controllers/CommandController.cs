using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using CommandPattern.Core;
using CommandPattern.Helpers;
using CommandPattern.Runners;
using Microsoft.Practices.Unity;

namespace CommandPattern.Controllers
{
    public class CommandController : Controller
    {
        private static readonly IDictionary<string, Type> Map = CommandReflectionHelper
            .GetCommandTypes()
            .Select(p => p.Key.GetGenericArguments().First())
            .ToDictionary(
                k => k.Name,
                v => v,
                StringComparer.InvariantCultureIgnoreCase);

        private readonly ICommandRunner _commandRunner;
        
        public CommandController(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner;
        }
        
        public ActionResult Execute(string commandModelName)
        {
            Type type;

            if (Map.ContainsKey(commandModelName))
                type = Map[commandModelName];
            else if (Map.ContainsKey(commandModelName + "Model"))
                type = Map[commandModelName + "Model"];
            else
                throw new ArgumentException("Invalid Command Name: " + commandModelName, "commandModelName");

            dynamic model = Activator.CreateInstance(type);
            UpdateModel(model);

            var result = _commandRunner.Execute(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }

}
