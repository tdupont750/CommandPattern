using System;
using CommandPattern.Core;
using CommandPattern.Models;

namespace CommandPattern.Commands
{
    public class FailCommand : ICommand<FailModel, int>
    {
        public void Validate(FailModel nameModel)
        {
            throw new NotImplementedException();
        }

        public int Execute(FailModel nameModel)
        {
            throw new NotImplementedException();
        }
    }
}