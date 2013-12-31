using CommandPattern.Core;

namespace CommandPattern.Commands
{
    public class GetUserNameCommand : ICommand<string>
    {
        public int Id { get; set; }
    }
}