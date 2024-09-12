using System.ComponentModel.DataAnnotations;

namespace FormSubmission.Models;

public class SaveModel
{
    [Required]
    public string Name { get; set; }

    public List<Block> Blocks { get; set; } = [];
}

public class Block
{
    [Required]
    public string Name { get; set; }

    public List<Field> Fields { get; set; } = [];
}
public enum Type
{
    Image = 1,
    Video
}
public class Field
{
    [Required]
    public string Name { get; set; }
    public Type Type { get; set; }
    public IFormFile? Image { get; set; }
    public string? VideoUrl { get; set; }
}