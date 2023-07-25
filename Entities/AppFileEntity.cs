namespace FormSubmissionDemo.Entities;

public class AppFileEntity : BaseEntity
{
    public int FileId { get; set; }
    public byte[] Content { get; set; }
}