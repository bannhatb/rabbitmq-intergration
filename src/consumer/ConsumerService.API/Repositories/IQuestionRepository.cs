using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerService.API.DTOs;
using ConsumerService.API.Models.Entities;

namespace ConsumerService.API.Repositories
{
    public interface IQuestionRepository
    {
        IQueryable<Question> Question { get; }

        void Add(Question question);
        void Update(Question question);
        void Delete(Question question);
        double CountQuestionOfExam(int examId);
        Question GetQuestionById(int id);
        List<Question> GetListQuestion();
        List<QuestionDto> GetListQuestionDtoByExamId(int examId);

        IQueryable<QuestionExam> QuestionExams { get; }
        void Add(QuestionExam questionExam);
        void Update(QuestionExam questionExam);
        void Delete(QuestionExam questionExam);

        IQueryable<Answer> Answers { get; }
        void Add(Answer answer);
        void Update(Answer answer);
        void Delete(Answer answer);
        Answer GetAnswerById(int id);
        List<Answer> GetListAnswer();
        List<Answer> GetListAnswerByQuestionId(int questionId);
        List<AnswerDto> GetListAnswerDtoByQuestionId(int questionId);

        bool IsSaveChanges();

    }
}