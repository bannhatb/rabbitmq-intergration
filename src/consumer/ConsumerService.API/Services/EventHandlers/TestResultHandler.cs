
using ConsumerService.API.DTOs;
using ConsumerService.API.Models.Entities;
using ConsumerService.API.Repositories;

namespace ConsumerService.API.Services.EventHandlers
{
    public class TestResultHandler
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IEventBusService _eventBus;

        public TestResultHandler()
        {
        }

        public TestResultHandler(IQuestionRepository questionRepository, IEventBusService eventBus)
        {
            _questionRepository = questionRepository;
            _eventBus = eventBus;
        }

        public void ProcessTestResult(TestResultDto testResultDto)
        {
            int rightAnswer = 0;
            int wrongAnswer = 0;
            var testUser = new ScoreUserTest();
            testUser.UserId = testResultDto.UserId;
            testUser.ExamId = testResultDto.ExamId;
            foreach (var item in testResultDto.ResultUserChooses)
            {
                if (item.Choose != null)
                {
                    var check = _questionRepository.Answers.FirstOrDefault(x => x.QuestionId == item.QuestionId && x.Id == item.Choose && x.RightAnswer == true);
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
            double score = rightAnswer / (rightAnswer + wrongAnswer) * 10;
            score = Math.Round(score, 1);
            testUser.Point = score;
            // Gui data qua test user
            _eventBus.Publish(testUser);

        }
    }
}