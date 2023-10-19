namespace GrubHubClone.Restaurant.Models.Domain;

public class Menu
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Product> Products { get; set; } = new List<Product>();
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
