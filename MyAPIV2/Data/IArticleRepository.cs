using MyAPIV2.Models;

namespace MyAPIV2.Data
{
  public interface IArticleRepository
  {
    public bool SaveChanges();
    public void AddEntity<T>(T entity);
    public void RemoveEntity<T>(T entity);
    public IEnumerable<Article> GetAll(string? search, int? userId);
    public IEnumerable<Article> GetByUserId(int userId);
    public Article GetById(int articleId);
  }
}
