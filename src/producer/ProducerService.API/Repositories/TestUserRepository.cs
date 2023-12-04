using System.Threading;
using Microsoft.EntityFrameworkCore;
using ProducerService.API.Models.Entities;

namespace ProducerService.API.Repositories
{
    public class TestUserRepository : ITestUserRepository
    {
        private readonly DataContext _dataContext;

        public IQueryable<TestUser> TestUsers => _dataContext.TestUsers;

        public IQueryable<TestQuestionUserChoose> TestQuestionUserChooses => _dataContext.TestQuestionUserChooses;

        public TestUserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(TestUser testUser)
        {
            _dataContext.TestUsers.Add(testUser);
        }

        public void Update(TestUser testUser)
        {
            _dataContext.TestUsers.Update(testUser);
        }

        public void Delete(TestUser testUser)
        {
            _dataContext.TestUsers.Remove(testUser);
        }

        public void Add(TestQuestionUserChoose testQuestionUserChoose)
        {
            _dataContext.TestQuestionUserChooses.Add(testQuestionUserChoose);
        }

        public void Update(TestQuestionUserChoose testQuestionUserChoose)
        {
            _dataContext.TestQuestionUserChooses.Update(testQuestionUserChoose);
        }

        public void Delete(TestQuestionUserChoose testQuestionUserChoose)
        {
            _dataContext.TestQuestionUserChooses.Remove(testQuestionUserChoose);
        }

        public bool IsSaveChanges()
        {
            return _dataContext.SaveChanges() > 0;
        }

        public List<TestUser> GetListTestUser()
        {
            return _dataContext.TestUsers.ToList();
        }

        public TestUser GetTestUserById(int id)
        {
            return _dataContext.TestUsers.FirstOrDefault(x => x.Id == id);
        }

        public List<TestQuestionUserChoose> GetListTestQuestionUserChoose()
        {
            return _dataContext.TestQuestionUserChooses.ToList();
        }

        public TestQuestionUserChoose GetTestQuestionUserChooseById(int id)
        {
            return _dataContext.TestQuestionUserChooses.FirstOrDefault(x => x.Id == id);
        }

        public List<TestUser> GetRankUserByExamId(int id)
        {
            var top10Users = _dataContext.TestUsers
                .Where(x => x.ExamId == id)
                .OrderByDescending(x => x.Point)
                .Take(10)
                .ToList();
            return top10Users;
        }
    }
}