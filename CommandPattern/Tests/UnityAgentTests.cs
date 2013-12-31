using System;
using CommandPattern.Agents;
using CommandPattern.Commands;
using CommandPattern.Core;
using CommandPattern.Helpers;
using Microsoft.Practices.Unity;
using Xunit;

namespace CommandPattern.Tests
{
    public class UnityAgentTests
    {
        [Fact]
        public void GetUser()
        {
            var factory = GetCommandRunner();
            var command = new GetUserNameCommand {Id = 1};

            var result = factory.Execute(command);
            Assert.Equal("Demo McTester", result);
        }

        [Fact]
        public void GetUserAsync()
        {
            var factory = GetCommandRunner();
            var command = new GetUserNameCommand {Id = 1};

            var result = factory.ExecuteAsync(command);
            result.Wait();
            Assert.Equal("Demo McTester", result.Result);
        }

        [Fact]
        public void GetPet()
        {
            var runner = GetCommandRunner();
            var command = new GetPetCommand();

            var result = runner.Execute(command);
            Assert.Equal(PetType.Dog, result.Type);
        }

        [Fact]
        public void GetPetInvalidId()
        {
            var runner = GetCommandRunner();
            var command = new GetPetCommand {Id = 42};

            Assert.Throws<ArgumentException>(() =>
            {
                runner.Execute(command);
            });
        }

        [Fact]
        public void Fail()
        {
            var factory = GetCommandRunner();
            var command = new FailCommand();

            Assert.Throws<NotImplementedException>(() =>
            {
                factory.Execute(command);
            });
        }

        [Fact]
        public void FailAsync()
        {
            var factory = GetCommandRunner();
            var command = new FailCommand();

            var result = factory.ExecuteAsync(command);

            Assert.Throws<AggregateException>(() => result.Wait());
        }

        private static IAgent GetCommandRunner()
        {
            var container = new UnityContainer();
            var map = CommandReflectionHelper.GetOperationMap();

            foreach (var pair in map)
            {
                var manager = new ContainerControlledLifetimeManager();
                container.RegisterType(pair.Key, pair.Value, manager);
            }

            return new UnityAgent(container);
        }
    }
}