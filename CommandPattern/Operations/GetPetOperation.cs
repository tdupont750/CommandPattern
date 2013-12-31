using System;
using System.Collections.Generic;
using System.Linq;
using CommandPattern.Commands;
using CommandPattern.Core;

namespace CommandPattern.Operations
{
    public class GetPetOperation : IOperation<GetPetCommand, GetPetResult>
    {
        public static readonly IList<GetPetResult> Pets = new[]
        {
            new GetPetResult {Id = 0, Name = "Taboo", Type = PetType.Dog},
            new GetPetResult {Id = 1, Name = "Linq", Type = PetType.Cat},
            new GetPetResult {Id = 2, Name = "Sql", Type = PetType.Cat},
            new GetPetResult {Id = 3, Name = "Kevin", Type = PetType.Fish}
        };

        public void Validate(GetPetCommand command)
        {
            if (Pets.Any(p => p.Id == command.Id) == false)
                throw new ArgumentException("Invalid Id");
        }

        public GetPetResult Execute(GetPetCommand command)
        {
            return Pets.First(p => p.Id == command.Id);
        }
    }
}