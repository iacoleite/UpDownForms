using Microsoft.EntityFrameworkCore;
using UpDownForms.Models;

public class UpDownFormsContext : DbContext
{
    public UpDownFormsContext(DbContextOptions<UpDownFormsContext> options) : base(options)
    {
    }

    public DbSet<Form> Forms { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Response> Responses { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User Entity

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);  // Primary Key
        //modelBuilder.Entity<User>()
        //    .Property(u => u.Id)
        //    .ValueGeneratedOnAdd();  // Auto-increment
        modelBuilder.Entity<User>()
            .Property(u => u.Name)
            .HasMaxLength(100)
            .IsRequired();  // Not Null
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired()
            .IsUnicode(false);  // Email field, set as required and unique
        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();  // PasswordHash field, set as required
        modelBuilder.Entity<User>()
            .Property(u => u.CreatedAt);  // Default value
        modelBuilder.Entity<User>()
            .Property(u => u.IsDeleted);  // Default value for IsDeleted

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

        // Configure enum mapping (store as integer or string)
        modelBuilder.Entity<Question>()
            .Property(q => q.Type)
            .HasConversion<int>();  // Store as an integer in the database

        // Relationships for Question and Form
        modelBuilder.Entity<Question>()
            .HasOne(q => q.Form)
            .WithMany(f => f.Questions)
            .HasForeignKey(q => q.FormId)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

        // Option Entity
        modelBuilder.Entity<Option>()
            .HasKey(o => o.Id);  // Primary Key
        modelBuilder.Entity<Option>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();  // Auto-increment
        modelBuilder.Entity<Option>()
            .Property(o => o.Order);        

        // Relationships for Option and Question
        modelBuilder.Entity<Option>()
            .HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

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
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();  // Auto-increment
        modelBuilder.Entity<Answer>()
            .Property(a => a.IsDeleted);

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

        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Option)
            .WithMany()
            .HasForeignKey(a => a.OptionId)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete

        // Answer text and Option validation (CHECK constraint logic)
        // This should be ok with MySql (?)
        //modelBuilder.Entity<Answer>()
        //    .ToTable(t => t.HasCheckConstraint("CHK_Answer",
        //        "(AnswerText IS NOT NULL AND OptionId IS NULL) OR (AnswerText IS NULL AND OptionId IS NOT NULL)"));

    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        
        foreach (var entry in entries)
        {
            if (entry.Entity is Form form)
            {
                form.UpdatedAt = DateTime.Now;
            }
        }
        return base.SaveChanges();
    }
}