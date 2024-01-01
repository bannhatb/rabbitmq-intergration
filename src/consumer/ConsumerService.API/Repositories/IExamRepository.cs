using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerService.API.Models.Entities;

namespace ConsumerService.API.Repositories
{
    public interface IExamRepository
    {
        IQueryable<Exam> Exams { get; }

        void Add(Exam exam);
        void Update(Exam exam);
        void Delete(Exam exam);
        bool IsSaveChanges();
        Exam GetExamById(int id);
        List<Exam> GetListExam();
        void Add(TestUser testUser);
        void Update(TestUser testUser);
    }
}