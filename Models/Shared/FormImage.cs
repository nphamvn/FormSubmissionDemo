using System.Text.Json.Serialization;

namespace FormSubmissionDemo.Models.Shared;

public class FormImage
{
    public const string DefaultSrc = "img/default.jpeg";
    public const int DefaultImageFileId = 1;
    [JsonIgnore]
    public IFormFile? FormFile { get; set; }
    public int? AppFileId { get; set; }
    public int? TempFileId { get; set; }
    public string? Src { get; set; }
    public string? Alt { get; set; }
}
