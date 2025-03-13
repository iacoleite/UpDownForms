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

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //// User Entity -> Not needed anymore (YAY!) because of Identity 

        //modelBuilder.Entity<User>()
        //    .HasKey(u => u.Id);  
        ////modelBuilder.Entity<User>()
        ////    .Property(u => u.Id)
        ////    .ValueGeneratedOnAdd(); 
        //modelBuilder.Entity<User>()
        //    .Property(u => u.Name)
        //    .HasMaxLength(100)
        //    .IsRequired();  
        //modelBuilder.Entity<User>()
        //    .Property(u => u.Email)
        //    .HasMaxLength(255)
        //    .IsRequired()
        //    .IsUnicode(false);  
        //modelBuilder.Entity<User>()
        //    .Property(u => u.PasswordHash)
        //    .HasMaxLength(255)
        //    .IsRequired(); 
        //modelBuilder.Entity<User>()
        //    .Property(u => u.CreatedAt);  
        //modelBuilder.Entity<User>()
        //    .Property(u => u.IsDeleted);  

        // Form Entity
        modelBuilder.Entity<Form>()
            .HasKey(f => f.Id);
        modelBuilder.Entity<Form>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Form>()
            .Property(f => f.Title)
            .HasMaxLength(255)
            .IsRequired();
        modelBuilder.Entity<Form>()
            .Property(f => f.CreatedAt);
        modelBuilder.Entity<Form>()
            .Property(f => f.UpdatedAt); 
        modelBuilder.Entity<Form>()
            .Property(f => f.IsPublished);
        modelBuilder.Entity<Form>()
            .Property(f => f.IsDeleted);

        // Relationships for Form and User
        modelBuilder.Entity<Form>()
            .HasOne(f => f.User)
            .WithMany(u => u.Forms)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);  

        // Question Entity
        modelBuilder.Entity<Question>()
            .HasKey(q => q.Id);  
        modelBuilder.Entity<Question>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();  
        modelBuilder.Entity<Question>()
            .Property(q => q.Text)
            .IsRequired();  
        modelBuilder.Entity<Question>()
            .Property(q => q.Order);
        modelBuilder.Entity<Question>()
            .Property(q => q.IsRequired);
        modelBuilder.Entity<Question>()
            .Property(q => q.IsDeleted);

        // inheritance between Question and differents types of questions using EF 
        modelBuilder.Entity<Question>()
            .HasDiscriminator<string>("BaseQuestionType")
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
            .Property(q => q.QuestionType)
            .HasConversion<string>();  

        modelBuilder.Entity<QuestionMultipleChoice>()
            .HasMany(q => q.Options)
            .WithOne(o => o.QuestionMultipleChoice)
            .HasForeignKey(o => o.QuestionId);

        // Option Entity
        modelBuilder.Entity<Option>()
            .HasKey(o => o.Id);  
        modelBuilder.Entity<Option>()
            .Property(o => o.Id)
            .ValueGeneratedOnAdd();  
        modelBuilder.Entity<Option>()
            .Property(o => o.Order);        

       
        // Response Entity
        modelBuilder.Entity<Response>()
            .HasKey(r => r.Id);  
        modelBuilder.Entity<Response>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();  
        modelBuilder.Entity<Response>()
            .Property(r => r.SubmittedAt);
        modelBuilder.Entity<Response>()
            .Property(r => r.IsDeleted);

        // Relationships for Response and Form
        modelBuilder.Entity<Response>()
            .HasOne(r => r.Form)
            .WithMany(f => f.Responses)
            .HasForeignKey(r => r.FormId)
            .OnDelete(DeleteBehavior.Cascade);  

        // Answer Entity
        modelBuilder.Entity<Answer>()
            .HasKey(a => a.Id);  
        modelBuilder.Entity<Answer>()
            .Property(a => a.Id)
            .ValueGeneratedOnAdd();  
        modelBuilder.Entity<Answer>()
            .Property(a => a.IsDeleted);

        // TPH Inheritance for Answer
        modelBuilder.Entity<Answer>()
            .HasDiscriminator<string>("AnswerType")
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

        modelBuilder.Entity<AnswerOpenEnded>()
           .Property(a => a.AnswerText)
           .IsRequired();
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
                //case Question question:
                //    question.UpdatedAt = DateTime.UtcNow;
                //    break;
                //case Response response:
                //    response.UpdatedAt = DateTime.UtcNow;
                //    break;
                //case Answer answer:
                //    answer.UpdatedAt = DateTime.UtcNow;
                //    break;
            }
        }
        return base.SaveChangesAsync(token);
    }
}