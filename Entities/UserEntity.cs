using System.ComponentModel.DataAnnotations;

namespace FormSubmissionDemo.Entities;

public class UserEntity : BaseEntity
{
    [Key]
    public int UserId { get; set; }
    public int ProfileFileId { get; set; }
    public string Username { get; set; }
}