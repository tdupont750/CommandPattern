using System;
using CommandPattern.Agents;
using CommandPattern.Commands;
using CommandPattern.Core;
using Xunit;

namespace CommandPattern.Tests
{
    public class HttpAgentTests
    {
        private const string BaseAddress = "http://localhost/CommandPattern/";

        [Fact]
        public void GetUser()
        {
            var runner = GetCommandRunner();
            var command = new GetUserNameCommand { Id = 1 };

            var result = runner.Execute(command);
            Assert.Equal("Demo McTester", result);
        }

        [Fact]
        public void GetUserAsync()
        {
            var runner = GetCommandRunner();
            var command = new GetUserNameCommand { Id = 1 };

            var result = runner.ExecuteAsync(command);
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
            var command = new GetPetCommand { Id = 42 };

            Assert.Throws<AggregateException>(() =>
            {
                runner.Execute(command);
            });
        }

        [Fact]
        public void Fail()
        {
            var runner = GetCommandRunner();
            var command = new FailCommand();

            Assert.Throws<AggregateException>(() =>
            {
                runner.Execute(command);
            });
        }

        [Fact]
        public void FailAsync()
        {
            var runner = GetCommandRunner();
            var command = new FailCommand();

            var result = runner.ExecuteAsync(command);

            Assert.Throws<AggregateException>(() => result.Wait());
        }

        private static IAgent GetCommandRunner()
        {
            return new HttpAgent(BaseAddress);
        }
    }
}