using Dapper;
using MyAPIV2.Models;
using System.Linq;

namespace MyAPIV2.Data
{
  public class ArticleRepository : IArticleRepository
  {
    private DataContextEF _connectionEF;

    public ArticleRepository(IConfiguration config)
    {
      _connectionEF = new DataContextEF(config);
    }

    public bool SaveChanges()
    {
      return _connectionEF.SaveChanges() >0;
    }

    public void AddEntity<T>(T entity)
    {
      if(entity != null)
      {
        _connectionEF.Add(entity);
      }
    }
    
    public void RemoveEntity<T>(T entity)
    {
      if(entity != null)
      {
        _connectionEF.Remove(entity);
      }
    }
    public IEnumerable<Article> GetByUserId(int userId)
    {
      return _connectionEF.Articles
        .Where(a => a.UserId == userId)
        .ToList<Article>();
    }
    public IEnumerable<Article> GetAll(string? search, int? userId)
    {
      return _connectionEF.Articles
        .Where(x =>
        search != null && userId != null ?
          (x.Title.Contains(search) || x.Content.Contains(search)) && 
           x.UserId == userId :
           search != null && userId == null ?
          (x.Title.Contains(search) || x.Content.Contains(search)) :
          search == null && userId != null ?
            x.UserId == userId :
            true
        );
    }

    public Article GetById(int articleId)
    {
      Article? articleDb = _connectionEF.Articles
        .Where(a => a.ArticleId == articleId)
        .FirstOrDefault<Article>();

      if (articleDb != null)
      {
        return articleDb;
      }
      throw new Exception("Fail to get article");
    }

  }
}