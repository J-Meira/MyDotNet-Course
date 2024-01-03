using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAPIV2.Data;
using MyAPIV2.Dtos;
using MyAPIV2.Models;
using System.Security.Claims;

namespace MyAPIV2.Controllers
{
  [Authorize]
  [ApiController]
  [Route("[controller]")]
  public class ArticleController : ControllerBase
  {

    private IArticleRepository _articleRepository;

    public ArticleController(IConfiguration config, IArticleRepository articleRepository)
    {
      _articleRepository = articleRepository;
    }

    [HttpPost("Add")]
    public IActionResult Add(ArticleDto article)

    {
      if (int.TryParse(this.User
        .FindFirstValue("userId"), out int userId))
      {
        Article articleDb = new Article()
        {
          UserId = userId,
          Title = article.Title,
          Content = article.Content
        };
        _articleRepository.AddEntity<Article>(articleDb);
        if (_articleRepository.SaveChanges())
        {
          return Ok(articleDb);
        }
        throw new Exception("Fail to add Article");
      }
      throw new Exception("Fail add Article");
    }

    [HttpGet("GetAll")]
    public IEnumerable<Article> GetAll(string? search, int? userId)
    {
      return _articleRepository.GetAll(search, userId);
    }

    [HttpGet("GetMyArticles")]
    public IEnumerable<Article> GetMyArticles()
    {
      if(int.TryParse(this.User
        .FindFirstValue("userId"), out int userId))
      {
        return _articleRepository.GetByUserId(userId);
      }
      throw new Exception("Fail to get Articles");
    }

    [HttpGet("GetById/{id}")]
    public Article GetById(int id)
    {
      return _articleRepository.GetById(id);
    }

    [HttpPut("UpdateById/{id}")]
    public IActionResult UpdateById(ArticleDto article, int id)
    {
      if (int.TryParse(this.User
        .FindFirstValue("userId"), out int userId))
      {
        Article? articleDb = _articleRepository.GetById(id);

        if (articleDb == null)
        {
          throw new Exception("Fail to update Article");
        }
        if (articleDb.UserId != userId)
        {
          return BadRequest("This Article is bound to another user");
        }
       
          articleDb.Content = article.Content;
          articleDb.Title = article.Title;
          articleDb.UpdatedAT = DateTime.Now;
        if (_articleRepository.SaveChanges())
        {
          return Ok();
        }
        throw new Exception("Fail to update Article");
      }
      throw new Exception("Fail to update Article");
      
    }

    [HttpDelete("DeleteById/{id}")]
    public IActionResult DeleteById(ArticleDto article, int id)
    {
      if (int.TryParse(this.User
        .FindFirstValue("userId"), out int userId))
      {
        Article? articleDb = _articleRepository.GetById(id);

        if (articleDb == null)
        {
          throw new Exception("Fail to delete Article");
        }
        if (articleDb.UserId != userId)
        {
          return BadRequest("This Article is bound to another user");
        }

        _articleRepository.RemoveEntity<Article>(articleDb);
        if (_articleRepository.SaveChanges())
        {
          return Ok();
        }
        throw new Exception("Fail to delete Article");
      }
      throw new Exception("Fail to delete Article");
      
    }

  }
}
