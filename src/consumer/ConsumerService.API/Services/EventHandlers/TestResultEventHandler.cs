
using ConsumerService.API.Models.Events;
using ConsumerService.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConsumerService.API.Services.EventHandlers
{
    public class TestResultEventHandler : IIntegrationEventHandler<TestResultEvent>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IEventBusService _eventBus;

        public TestResultEventHandler(IQuestionRepository questionRepository, IEventBusService eventBus)
        {
            _questionRepository = questionRepository;
            _eventBus = eventBus;
        }

        public async Task Handle(TestResultEvent @event)
        {
            double rightAnswer = 0;
            double wrongAnswer = 0;
            double countQuestionOfExam = _questionRepository.CountQuestionOfExam(@event.ExamId);
            var testUser = new ScoreUserTestEvent();
            testUser.UserId = @event.UserId;
            testUser.ExamId = @event.ExamId;
            foreach (var item in @event.ResultUserChooses)
            {
                if (item.Choose != null)
                {
                    var check = await _questionRepository.Answers.FirstOrDefaultAsync(x => x.QuestionId == item.QuestionId && x.Id == item.Choose && x.RightAnswer == true);
                    // var check = _questionRepository.Answers.Where(x => x.QuestionId == item.QuestionId && x.Id == item.Choose && x.RightAnswer == true).FirstOrDefault();
                    if (check == null)
                    {
                        wrongAnswer += 1;
                    }
                    else
                    {
                        rightAnswer += 1;
                    }
                }
                else
                {
                    wrongAnswer += 1;
                }
            }

            double score = (rightAnswer / countQuestionOfExam) * 10;
            score = Math.Round(score, 1);
            testUser.Point = score;
            // Gui data qua test user
            _eventBus.Publish(testUser);
        }
    }
}