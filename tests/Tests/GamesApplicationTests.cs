using Archse.Application;
using Archse.Publisher;
using Archse.Service;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace Tests
{
    [TestFixture]
    public class GamesApplicationTests
    {
        private MockRepository mockRepository;

        private Mock<IGamesService> mockGamesService;
        private Mock<IMapper> mockMapper;
        private Mock<ILogger<GamesApplication>> mockLogger;
        private Mock<IPublisher> mockPublisher;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockGamesService = this.mockRepository.Create<IGamesService>();
            this.mockMapper = this.mockRepository.Create<IMapper>();
            this.mockLogger = this.mockRepository.Create<ILogger<GamesApplication>>();
            this.mockPublisher = this.mockRepository.Create<IPublisher>();
        }

        private GamesApplication CreateGamesApplication()
        {
            return new GamesApplication(
                this.mockGamesService.Object,
                this.mockMapper.Object,
                this.mockLogger.Object,
                this.mockPublisher.Object);
        }

        [Test]
        public void Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamesApplication = this.CreateGamesApplication();
            GameRequest game = null;

            // Act
            var result = gamesApplication.Create(
                game);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamesApplication = this.CreateGamesApplication();
            GameRequest game = null;
            string chave = null;

            // Act
            gamesApplication.Create(
                game,
                chave);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void Publish_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamesApplication = this.CreateGamesApplication();
            GameRequest game = null;

            // Act
            var result = gamesApplication.Publish(
                game);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamesApplication = this.CreateGamesApplication();
            string identificador = null;

            // Act
            gamesApplication.Delete(
                identificador);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamesApplication = this.CreateGamesApplication();
            string identificador = null;

            // Act
            var result = gamesApplication.Get(
                identificador);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void Update_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var gamesApplication = this.CreateGamesApplication();
            string identificador = null;
            GameRequest gameIn = null;

            // Act
            gamesApplication.Update(
                identificador,
                gameIn);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
