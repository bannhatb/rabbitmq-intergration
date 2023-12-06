
using ProducerService.API.Models.Entities;

namespace ProducerService.API.Repositories
{
    public interface ITestUserRepository
    {
        IQueryable<TestUser> TestUsers { get; }
        void Add(TestUser testUser);
        void Update(TestUser testUser);
        void Delete(TestUser testUser);
        List<TestUser> GetListTestUser();
        List<TestUser> GetRankUserByExamId(int id);
        TestUser GetTestUserById(int id);

        IQueryable<TestQuestionUserChoose> TestQuestionUserChooses { get; }
        void Add(TestQuestionUserChoose testQuestionUserChoose);
        void Update(TestQuestionUserChoose testQuestionUserChoose);
        void Delete(TestQuestionUserChoose testQuestionUserChoose);
        List<TestQuestionUserChoose> GetListTestQuestionUserChoose();
        TestQuestionUserChoose GetTestQuestionUserChooseById(int id);

        bool IsSaveChanges();
    }
}