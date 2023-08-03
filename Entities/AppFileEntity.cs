using System.ComponentModel.DataAnnotations;

namespace FormSubmissionDemo.Entities;

public class AppFileEntity : BaseEntity
{
    [Key]
    public int FileId { get; set; }
    public byte[] Content { get; set; }
}