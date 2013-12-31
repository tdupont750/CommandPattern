using System;
using CommandPattern.Commands;
using CommandPattern.Core;

namespace CommandPattern.Operations
{
    public class FailOperation : IOperation<FailCommand, int>
    {
        public void Validate(FailCommand command)
        {
            throw new NotImplementedException();
        }

        public int Execute(FailCommand command)
        {
            throw new NotImplementedException();
        }
    }
}