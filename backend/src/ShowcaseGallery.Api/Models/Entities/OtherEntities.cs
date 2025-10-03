namespace ShowcaseGallery.Api.Models.Entities;

public class Rating
{
    public Guid Id { get; set; }
    public Guid ShowcaseId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; } // 1-5
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Showcase Showcase { get; set; } = null!;
    public User User { get; set; } = null!;
}

public class Comment
{
    public Guid Id { get; set; }
    public Guid ShowcaseId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsEdited { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Showcase Showcase { get; set; } = null!;
    public User User { get; set; } = null!;
    public Comment? Parent { get; set; }
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string Payload { get; set; } = "{}"; // JSONB stored as string
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
}

public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
}