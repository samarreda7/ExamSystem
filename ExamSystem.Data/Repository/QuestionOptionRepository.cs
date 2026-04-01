using ExamSystem.Data.DBContext;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Data.Repository
{
    public class QuestionOptionRepository : BaseRepository<QuestionOption>, IQuestionOptionRepository
    {
        public QuestionOptionRepository(AppDBContext context) : base(context) { }




    }
}
