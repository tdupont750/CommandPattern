using System;
using System.Collections.Generic;
using System.Linq;
using CommandPattern.Core;
using CommandPattern.Models;

namespace CommandPattern.Commands
{
    public class GetPetCommand : ICommand<GetPetModel, GetPetResult>
    {
        public static readonly IList<GetPetResult> Pets = new[]
        {
            new GetPetResult {Id = 0, Name = "Taboo", Type = PetType.Dog},
            new GetPetResult {Id = 1, Name = "Linq", Type = PetType.Cat},
            new GetPetResult {Id = 2, Name = "Sql", Type = PetType.Cat},
            new GetPetResult {Id = 3, Name = "Kevin", Type = PetType.Fish}
        };

        public void Validate(GetPetModel model)
        {
            if (Pets.Any(p => p.Id == model.Id) == false)
                throw new ArgumentException("Invalid Id");
        }

        public GetPetResult Execute(GetPetModel model)
        {
            return Pets.First(p => p.Id == model.Id);
        }
    }
}