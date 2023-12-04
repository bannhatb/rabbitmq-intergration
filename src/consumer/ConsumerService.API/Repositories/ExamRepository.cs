using ConsumerService.API.Models.Entities;

namespace ConsumerService.API.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly DataContext _dataContext;

        public ExamRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<Exam> Exams => _dataContext.Exams;

        public void Add(Exam exam)
        {
            _dataContext.Exams.Add(exam);
        }

        public void Delete(Exam exam)
        {
            _dataContext.Exams.Remove(exam);
        }

        public Exam GetExamById(int id)
        {
            return _dataContext.Exams.FirstOrDefault(x => x.Id == id);
        }

        public List<Exam> GetListExam()
        {
            return _dataContext.Exams.ToList();
        }

        public bool IsSaveChanges()
        {
            return _dataContext.SaveChanges() > 0;
        }

        public void Update(Exam exam)
        {
            _dataContext.Exams.Update(exam);
        }
    }
}