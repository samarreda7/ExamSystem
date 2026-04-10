using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;


namespace ExamSystem.Application.Service
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitofwork;
        public QuestionService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
    }
}
