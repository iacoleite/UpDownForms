namespace UpDownForms.Models
{
    public interface IVerifyOwnership
    {
        string UserId { get; }

        string GetUserId();
    }
}
