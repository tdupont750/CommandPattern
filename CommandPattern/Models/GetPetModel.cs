using CommandPattern.Core;

namespace CommandPattern.Models
{
    public class GetPetModel : ICommandModel<GetPetResult>
    {
        public int Id { get; set; }
    }

    public enum PetType
    {
        Dog,
        Cat,
        Fish
    }

    public class GetPetResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PetType Type { get; set; }
    }
}