using System;
using CommandPattern.Core;
using CommandPattern.Models;
using CommandPattern.Runners;
using Xunit;

namespace CommandPattern.Tests
{
    public class HttpCommandRunnerTests
    {
        private const string BaseAddress = "http://localhost/CommandPattern/";

        [Fact]
        public void GetUser()
        {
            var runner = GetCommandRunner();
            var model = new GetUserNameModel { Id = 1 };

            var result = runner.Execute(model);
            Assert.Equal("Demo McTester", result);
        }

        [Fact]
        public void GetUserAsync()
        {
            var runner = GetCommandRunner();
            var model = new GetUserNameModel { Id = 1 };

            var result = runner.ExecuteAsync(model);
            result.Wait();
            Assert.Equal("Demo McTester", result.Result);
        }

        [Fact]
        public void GetPet()
        {
            var runner = GetCommandRunner();
            var model = new GetPetModel();

            var result = runner.Execute(model);
            Assert.Equal(PetType.Dog, result.Type);
        }

        [Fact]
        public void GetPetInvalidId()
        {
            var runner = GetCommandRunner();
            var model = new GetPetModel { Id = 42 };

            Assert.Throws<AggregateException>(() =>
            {
                runner.Execute(model);
            });
        }

        [Fact]
        public void Fail()
        {
            var runner = GetCommandRunner();
            var model = new FailModel();

            Assert.Throws<AggregateException>(() =>
            {
                runner.Execute(model);
            });
        }

        [Fact]
        public void FailAsync()
        {
            var runner = GetCommandRunner();
            var model = new FailModel();

            var result = runner.ExecuteAsync(model);

            Assert.Throws<AggregateException>(() => result.Wait());
        }

        private static ICommandRunner GetCommandRunner()
        {
            return new HttpCommandRunner(BaseAddress);
        }
    }
}