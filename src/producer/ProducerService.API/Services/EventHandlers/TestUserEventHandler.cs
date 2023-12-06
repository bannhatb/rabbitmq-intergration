
using ProducerService.API.Models.Entities;
using ProducerService.API.Models.Events;
using ProducerService.API.Repositories;

namespace ProducerService.API.Services.EventHandlers
{
    public class TestUserHandler : IIntegrationEventHandler<ScoreUserTestEvent>
    {
        private readonly ITestUserRepository _testUserRepository;
        public TestUserHandler(ITestUserRepository testUserRepository)
        {
            _testUserRepository = testUserRepository;
        }

        public async Task Handle(ScoreUserTestEvent @event)
        {
            var newUserTest = new TestUser()
            {
                ExamId = @event.ExamId,
                UserId = @event.UserId,
                Point = @event.Point
            };
            _testUserRepository.Add(newUserTest);
            _testUserRepository.IsSaveChanges();
        }
    }
}