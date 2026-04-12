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
        public async Task<int> CountByQuestionIdAsync(Guid questionId)
        {
            return await _dbSet.CountAsync(qo => qo.QuestionId == questionId);
        }

        public async Task<bool> HasCorrectOptionAsync(Guid questionId)
        {
            return await _dbSet.AnyAsync(qo => qo.QuestionId == questionId && qo.IsCorrect);
        }

        public async Task<bool> HasOtherCorrectOptionAsync(Guid questionId, Guid optionId)
        {
            return await _dbSet.AnyAsync(qo =>
                qo.QuestionId == questionId &&
                qo.Id != optionId &&
                qo.IsCorrect);
        }
    }
}
