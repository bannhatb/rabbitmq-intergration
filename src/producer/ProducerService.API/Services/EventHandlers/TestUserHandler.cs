
using ProducerService.API.DTOs;
using ProducerService.API.Models.Entities;
using ProducerService.API.Repositories;

namespace ProducerService.API.Services.EventHandlers
{
    public class TestUserHandler
    {
        private readonly ITestUserRepository _testUserRepository;
        public TestUserHandler()
        {
        }
        public TestUserHandler(ITestUserRepository testUserRepository)
        {
            _testUserRepository = testUserRepository;
        }

        public void ProcessScoreUserTest(ScoreUserTest scoreUserTest)
        {
            var newUserTest = new TestUser()
            {
                ExamId = scoreUserTest.ExamId,
                UserId = scoreUserTest.UserId,
                Point = scoreUserTest.Point
            };
            _testUserRepository.Add(newUserTest);
            _testUserRepository.IsSaveChanges();
        }
    }
}