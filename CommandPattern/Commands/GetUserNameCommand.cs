using System;
using CommandPattern.Core;
using CommandPattern.Models;

namespace CommandPattern.Commands
{
    public class GetUserNameCommand : ICommand<GetUserNameModel, string>
    {
        public void Validate(GetUserNameModel model)
        {
            if (model.Id == default(int))
                throw new ArgumentException("Invalid Id");
        }

        public string Execute(GetUserNameModel model)
        {
            return "Demo McTester";
        }
    }
}