using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using CommandPattern.Core;
using CommandPattern.Helpers;
using CommandPattern.Models;
using CommandPattern.Runners;
using Microsoft.Practices.Unity;
using Xunit;

namespace CommandPattern.Tests
{
    public class UnityCommandRunnerTests
    {
        [Fact]
        public void GetUser()
        {
            var factory = GetCommandRunner();
            var model = new GetUserNameModel {Id = 1};

            var result = factory.Execute(model);
            Assert.Equal("Demo McTester", result);
        }

        [Fact]
        public void GetUserAsync()
        {
            var factory = GetCommandRunner();
            var model = new GetUserNameModel {Id = 1};

            var result = factory.ExecuteAsync(model);
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
            var model = new GetPetModel {Id = 42};

            Assert.Throws<ArgumentException>(() =>
            {
                runner.Execute(model);
            });
        }

        [Fact]
        public void Fail()
        {
            var factory = GetCommandRunner();
            var model = new FailModel();

            Assert.Throws<NotImplementedException>(() =>
            {
                factory.Execute(model);
            });
        }

        [Fact]
        public void FailAsync()
        {
            var factory = GetCommandRunner();
            var model = new FailModel();

            var result = factory.ExecuteAsync(model);

            Assert.Throws<AggregateException>(() => result.Wait());
        }

        private static ICommandRunner GetCommandRunner()
        {
            var container = new UnityContainer();
            var map = CommandReflectionHelper.GetCommandTypes();

            foreach (var pair in map)
            {
                var manager = new ContainerControlledLifetimeManager();
                container.RegisterType(pair.Key, pair.Value, manager);
            }

            return new UnityCommandRunner(container);
        }
    }
}