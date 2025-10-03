namespace ShowcaseGallery.Api.Models.Entities;

public enum ShowcaseType
{
    ImageRendering,
    TextGeneration
}

public enum SourceFileType
{
    Pdf,
    Docx,
    Text
}

public class Showcase
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string MediaMetadata { get; set; } = "{\"files\": []}"; // JSONB stored as string
    public int ViewCount { get; set; } = 0;
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Showcase type fields
    public ShowcaseType ShowcaseType { get; set; }
    public string Prompt { get; set; } = string.Empty;

    // Image Rendering fields
    public string? SourceImageUrl { get; set; }
    public string? ResultImageUrl { get; set; }

    // Text Generation fields
    public string? GeneratedText { get; set; }
    public string? SourceText { get; set; }
    public string? SourceFileUrl { get; set; }
    public SourceFileType? SourceFileType { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}