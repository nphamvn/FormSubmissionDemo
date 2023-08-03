using System.ComponentModel.DataAnnotations;

namespace FormSubmissionDemo.Models;
public class IgnoreValidation {
    [Required]
    public string Field1 { get; set; }
    [Required]
    public string Field2 { get; set; }
}