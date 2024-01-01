namespace GrubHubClone.Common.Models;

public class MenuModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ProductModel> Products { get; set; } = new List<ProductModel>();
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
