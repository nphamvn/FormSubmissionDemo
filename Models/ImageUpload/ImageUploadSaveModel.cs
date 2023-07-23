namespace FormSubmissionDemo.Models.ImageUpload;

public class ImageUploadSaveModel : BaseModel
{
    public IFormFile Image { get; set; }
}
