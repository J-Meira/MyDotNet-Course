namespace MyAPIV2.Models
{
  public partial class Article
  {
    public int ArticleId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAT {get; set;} = DateTime.Now;
    public DateTime UpdatedAT {get; set;} = DateTime.Now;

  }
}
