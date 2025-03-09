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
    public DbSet<Option> Options { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<User> AppUsers => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //// User Entity

        //modelBuilder.Entity<User>()
        //    .HasKey(u => u.Id);  // Primary Key
        ////modelBuilder.Entity<User>()
        ////    .Property(u => u.Id)
        ////    .ValueGeneratedOnAdd();  // Auto-increment
        //modelBuilder.Entity<User>()
        //    .Property(u => u.Name)
        //    .HasMaxLength(100)
        //    .IsRequired();  // Not Null
        //modelBuilder.Entity<User>()
        //    .Property(u => u.Email)
        //    .HasMaxLength(255)
        //    .IsRequired()
        //    .IsUnicode(false);  // Email field, set as required and unique
        //modelBuilder.Entity<User>()
        //    .Property(u => u.PasswordHash)
        //    .HasMaxLength(255)
        //    .IsRequired();  // PasswordHash field, set as required
        //modelBuilder.Entity<User>()
        //    .Property(u => u.CreatedAt);  // Default value
        //modelBuilder.Entity<User>()
        //    .Property(u => u.IsDeleted);  // Default value for IsDeleted

        // Form Entity
        modelBuilder.Entity<Form>()
            .HasKey(f => f.Id);  // Primary Key
        modelBuilder.Entity<Form>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();  // Auto-increment
        modelBuilder.Entity<Form>()
            .Property(f => f.Title)
            .HasMaxLength(255)
            .IsRequired();  // Title field, set as required
        modelBuilder.Entity<Form>()
            .Property(f => f.CreatedAt);

        modelBuilder.Entity<Form>()
            //FOR MY SQL!!! REMEMBER TO CHANGE AFTER CHANGE DBMS!!!!!!!!!
            .Property(f => f.UpdatedAt); 
            

        // USE WHILE USING SQLITE!!!! 
        //.Property(f => f.UpdatedAt)
        //.HasDefaultValueSql("CURRENT_TIMESTAMP")
        //.ValueGeneratedOnAddOrUpdate();  // Default value with update
        modelBuilder.Entity<Form>()
            .Property(f => f.IsPublished);
            
        modelBuilder.Entity<Form>()
            .Property(f => f.IsDeleted);

        // Relationships for Form and User
        modelBuilder.Entity<Form>()
            .HasOne(f => f.User)
            .WithMany(u => u.Forms)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

        // Question Entity
        modelBuilder.Entity<Question>()
            .HasKey(q => q.Id);  // Primary Key
        modelBuilder.Entity<Question>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();  // Auto-increment
        modelBuilder.Entity<Question>()
            .Property(q => q.Text)
            .IsRequired();  // Text field, set as required
        modelBuilder.Entity<Question>()
            .Property(q => q.Order);
        modelBuilder.Entity<Question>()
            .Property(q => q.IsRequired);
        modelBuilder.Entity<Question>()
            .Property(q => q.IsDeleted);

        // Trying to implement inheritance between Question and differents types of questions using EF 
        modelBuilder.Entity<Question>()
            .HasDiscriminator<string>("QuestionType")
            .HasValue<QuestionMultipleChoice>("MultipleChoice")
            .HasValue<QuestionOpenEnded>("OpenEnded");

        // Relationships for Question and Form
        modelBuilder.Entity<Question>()
            .HasOne(q => q.Form)
            .WithMany(f => f.Questions)
            .HasForeignKey(q => q.FormId)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete
        
        modelBuilder.Entity<QuestionMultipleChoice>()
            .Property(q => q.Type)
            .HasConversion<string>();  // Store as an integer in the database

        modelBuilder.Entity<QuestionMultipleChoice>()
            .HasMany(q => q.Options)
            .WithOne(o => o.QuestionMultipleChoice)
            .HasForeignKey(o => o.QuestionId);

        // Configure enum mapping (store as integer or string)
 


        // Option Entity
        modelBuilder.Entity<Option>()
            .HasKey(o => o.Id);  // Primary Key
        modelBuilder.Entity<Option>()
            .Property(o => o.Id)
            .ValueGeneratedOnAdd();  // Auto-increment
        modelBuilder.Entity<Option>()
            .Property(o => o.Order);        

        //// Relationships for Option and Question
        //modelBuilder.Entity<Option>()
        //    .HasOne(o => o.QuestionMultipleChoice)
        //    .WithMany(q => q.Options)
        //    .HasForeignKey(o => o.QuestionId)
        //    .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

        // Response Entity
        modelBuilder.Entity<Response>()
            .HasKey(r => r.Id);  // Primary Key
        modelBuilder.Entity<Response>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();  // Auto-increment
        modelBuilder.Entity<Response>()
            .Property(r => r.SubmittedAt);
        modelBuilder.Entity<Response>()
            .Property(r => r.IsDeleted);

        // Relationships for Response and Form
        modelBuilder.Entity<Response>()
            .HasOne(r => r.Form)
            .WithMany(f => f.Responses)
            .HasForeignKey(r => r.FormId)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

        // Answer Entity
        modelBuilder.Entity<Answer>()
            .HasKey(a => a.Id);  // Primary Key
        modelBuilder.Entity<Answer>()
            .Property(a => a.Id)
            .ValueGeneratedOnAdd();  // Auto-increment
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
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

        modelBuilder.Entity<AnswerMultipleChoice>()
            .HasOne(a => a.Options)
            .WithMany()
            .HasForeignKey(a => a.OptionId)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

        modelBuilder.Entity<AnswerOpenEnded>()
           .Property(a => a.AnswerText)
           .IsRequired();

        // Answer text and Option validation (CHECK constraint logic)
        // This should be ok with MySql (?)
        //modelBuilder.Entity<Answer>()
        //    .ToTable(t => t.HasCheckConstraint("CHK_Answer",
        //        "(AnswerText IS NOT NULL AND OptionId IS NULL) OR (AnswerText IS NULL AND OptionId IS NOT NULL)"));

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