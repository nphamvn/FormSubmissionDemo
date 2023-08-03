using System.ComponentModel.DataAnnotations;

namespace FormSubmissionDemo.Entities;

public class TempFileEntity : BaseEntity
{
    [Key]
    public int FileId { get; set; }
    public byte[] Content { get; set; }
}
