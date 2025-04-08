using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using UpDownForms.Models;

public class UpDownFormsContext : IdentityDbContext<User>
{
    public UpDownFormsContext(DbContextOptions<UpDownFormsContext> options) : base(options)
    {
    }
    public DbSet<Form> Forms { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionMultipleChoice> QuestionsMultipleChoice { get; set; }
    public DbSet<QuestionOpenEnded> QuestionsOpenEnded { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<AnswerMultipleChoice> AnswersMultipleChoice { get; set; }
    public DbSet<AnswerOpenEnded> AnswersOpenEnded { get; set; }
    public DbSet<AnsweredOption> AnsweredOptions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<User> AppUsers => Set<User>();

    public UpDownFormsContext() { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //setting some types because default is insanely large
        modelBuilder.Entity<Form>()
            .Property(f => f.Description)
            .HasColumnType("text");

        modelBuilder.Entity<Option>()
            .Property(o => o.Text)
            .HasColumnType("text");

        modelBuilder.Entity<Question>()
            .Property(q => q.Text)
            .HasColumnType("text");

        modelBuilder.Entity<QuestionMultipleChoice>()
            .Property(q => q.QuestionMCType)
            .HasColumnType("varchar(255)");

        modelBuilder.Entity<Response>()
            .Property(r => r.RespondentEmail)
            .HasColumnType("varchar(255)");
        
        modelBuilder.Entity<AnswerOpenEnded>()
            .Property(a => a.AnswerText)
            .HasColumnType("text");

        // Relationships for Form and User
        modelBuilder.Entity<Form>()
            .HasOne(f => f.User)
            .WithMany(u => u.Forms)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);  

        // inheritance between Question and differents types of questions using EF 
        modelBuilder.Entity<Question>()
            .HasDiscriminator<string>("Type")
            .HasValue<QuestionMultipleChoice>("MultipleChoice")
            .HasValue<QuestionOpenEnded>("OpenEnded");

        // Relationships for Question and Form
        modelBuilder.Entity<Question>()
            .HasOne(q => q.Form)
            .WithMany(f => f.Questions)
            .HasForeignKey(q => q.FormId)
            .OnDelete(DeleteBehavior.Cascade);

        // Store QuestionType as an integer in the db
        modelBuilder.Entity<QuestionMultipleChoice>()
            .Property(q => q.QuestionMCType)
            .HasConversion<string>();  

        modelBuilder.Entity<QuestionMultipleChoice>()
            .HasMany(q => q.Options)
            .WithOne(o => o.QuestionMultipleChoice)
            .HasForeignKey(o => o.QuestionId);

        // Relationships for Response and Form
        modelBuilder.Entity<Response>()
            .HasOne(r => r.Form)
            .WithMany(f => f.Responses)
            .HasForeignKey(r => r.FormId)
            .OnDelete(DeleteBehavior.Cascade);  

        // TPH Inheritance for Answer
        modelBuilder.Entity<Answer>()
            .HasDiscriminator<string>("Type")
            .HasValue<AnswerMultipleChoice>("MultipleChoice")
            .HasValue<AnswerOpenEnded>("OpenEnded");

        // Relationships for Answer, Response, Question, and Option
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Response)
            .WithMany(r => r.Answers)
            .HasForeignKey(a => a.ResponseId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);  

        modelBuilder.Entity<AnsweredOption>()
            .HasKey(ao => new { ao.AnswerMultipleChoiceId, ao.OptionId }); 

        modelBuilder.Entity<AnsweredOption>()
            .HasOne(ao => ao.AnswerMultipleChoice)
            .WithMany(amc => amc.SelectedOptions) 
            .HasForeignKey(ao => ao.AnswerMultipleChoiceId);

        modelBuilder.Entity<AnsweredOption>()
            .HasOne(ao => ao.Option)
            .WithMany(o => o.AnsweredOptions) 
            .HasForeignKey(ao => ao.OptionId);
    }

    public override Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        
        foreach (var entry in entries)
        {
            switch (entry.Entity)
            {
                case Form form:
                    form.UpdatedAt = DateTime.UtcNow;
                    break;
                case User user:
                    user.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
        return base.SaveChangesAsync(token);
    }
}