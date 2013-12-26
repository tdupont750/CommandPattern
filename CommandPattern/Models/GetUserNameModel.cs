using CommandPattern.Core;

namespace CommandPattern.Models
{
    public class GetUserNameModel : ICommandModel<string>
    {
        public int Id { get; set; }
    }
}