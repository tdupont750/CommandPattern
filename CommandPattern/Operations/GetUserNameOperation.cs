using System;
using CommandPattern.Commands;
using CommandPattern.Core;

namespace CommandPattern.Operations
{
    public class GetUserNameOperation : IOperation<GetUserNameCommand, string>
    {
        public void Validate(GetUserNameCommand command)
        {
            if (command.Id == default(int))
                throw new ArgumentException("Invalid Id");
        }

        public string Execute(GetUserNameCommand command)
        {
            return "Demo McTester";
        }
    }
}