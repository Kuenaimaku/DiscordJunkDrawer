using DiscordJunkDrawer.App.Interfaces;
using DiscordJunkDrawer.App.Models;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordJunkDrawer.Test
{

    [TestFixture]
    public class RepositoryTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAllAsyncReturnsValue()
        {
            var response = new List<DiscordGuildModel>()
            {
                new DiscordGuildModel
                {
                    Id = 1,
                    Name = "Test"
                }
            };

            var _repository = Substitute.For<IRepository<DiscordGuildModel>>();
            _repository.GetAllAsync().Returns(Task.FromResult(response));

            var r = await _repository.GetAllAsync();
            Assert.IsTrue(r.Count == 1);
        }
    }
}