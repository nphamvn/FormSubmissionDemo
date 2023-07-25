namespace FormSubmissionDemo.Entities;

public class TempFileEntity : BaseEntity
{
    public int FileId { get; set; }
    public byte[] Content { get; set; }
}
