using ConsumerService.API.DTOs;
using ConsumerService.API.Models.Entities;

namespace ConsumerService.API.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DataContext _dataContext;

        public QuestionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<Question> Question => _dataContext.Questions;

        public IQueryable<QuestionExam> QuestionExams => _dataContext.QuestionExams;

        public IQueryable<Answer> Answers => _dataContext.Answers;

        public void Add(Question question)
        {
            _dataContext.Questions.Add(question);
        }

        public void Add(QuestionExam questionExam)
        {
            _dataContext.QuestionExams.Add(questionExam);
        }

        public void Add(Answer answer)
        {
            _dataContext.Answers.Add(answer);
        }

        public double CountQuestionOfExam(int examId)
        {
            return _dataContext.QuestionExams.Count(x => x.ExamId == examId);
        }

        public void Delete(Question question)
        {
            _dataContext.Questions.Remove(question);
        }

        public void Delete(QuestionExam questionExam)
        {
            _dataContext.QuestionExams.Remove(questionExam);
        }

        public void Delete(Answer answer)
        {
            _dataContext.Answers.Remove(answer);
        }

        public Answer GetAnswerById(int id)
        {
            return _dataContext.Answers.FirstOrDefault(x => x.Id == id);
        }

        public List<Answer> GetListAnswer()
        {
            return _dataContext.Answers.ToList();
        }

        public List<Answer> GetListAnswerByQuestionId(int questionId)
        {
            return _dataContext.Answers.Where(x => x.QuestionId == questionId).ToList();
        }

        public List<AnswerDto> GetListAnswerDtoByQuestionId(int questionId)
        {
            return _dataContext.Answers
                .Where(x => x.QuestionId == questionId)
                .Select(x => new AnswerDto
                {
                    AnswerId = x.Id,
                    AnswerContent = x.AnswerContent,
                    RightAnswer = x.RightAnswer
                })
                .ToList();
        }

        public List<Question> GetListQuestion()
        {
            return _dataContext.Questions.ToList();
        }


        public List<QuestionDto> GetListQuestionDtoByExamId(int examId)
        {
            List<QuestionDto> data = _dataContext.Questions
                .Join(_dataContext.QuestionExams,
                    question => question.Id,
                    questionExam => questionExam.QuestionId,
                    (question, questionExam) => new { question, questionExam })
                .Join(_dataContext.Exams,
                    questionQuestionExam => questionQuestionExam.questionExam.ExamId,
                    exam => exam.Id,
                    (questionQuestionExam, exam) => new { questionQuestionExam, exam })
                .Where(x => x.exam.Id == examId)
                .Select(x => new QuestionDto
                {
                    QuestionId = x.questionQuestionExam.question.Id,
                    QuestionContent = x.questionQuestionExam.question.QuestionContent,
                    Explaint = x.questionQuestionExam.question.Explaint
                })
                .ToList();
            foreach (var item in data)
            {
                item.AnswerDtos = GetListAnswerDtoByQuestionId(item.QuestionId);
            }
            return data;
        }

        public Question GetQuestionById(int id)
        {
            return _dataContext.Questions.FirstOrDefault(x => x.Id == id);
        }

        public bool IsSaveChanges()
        {
            return _dataContext.SaveChanges() > 0;
        }

        public void Update(Question question)
        {
            _dataContext.Questions.Update(question);
        }

        public void Update(QuestionExam questionExam)
        {
            _dataContext.QuestionExams.Update(questionExam);
        }

        public void Update(Answer answer)
        {
            _dataContext.Answers.Update(answer);
        }
    }
}