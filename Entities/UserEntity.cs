namespace FormSubmissionDemo.Entities;

public class UserEntity : BaseEntity
{
    public int Id { get; set; }
    public int ProfileFileId { get; set; }
    public string Username { get; set; }
}