using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UpDownForms.Models;

public interface IUpDownFormsContext
{
    DbSet<AnsweredOption> AnsweredOptions { get; set; }
    DbSet<Answer> Answers { get; set; }
    DbSet<AnswerMultipleChoice> AnswersMultipleChoice { get; set; }
    DbSet<AnswerOpenEnded> AnswersOpenEnded { get; set; }
    DbSet<User> Users { get; }
    DbSet<Form> Forms { get; set; }
    DbSet<Option> Options { get; set; }
    DbSet<Question> Questions { get; set; }
    DbSet<QuestionMultipleChoice> QuestionsMultipleChoice { get; set; }
    DbSet<QuestionOpenEnded> QuestionsOpenEnded { get; set; }
    DbSet<Response> Responses { get; set; }
    Task<int> SaveChangesAsync(CancellationToken token = default);
    IDbContextTransaction BeginTransaction();

}