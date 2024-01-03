using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MyAPIV2.Data
{
  public class DataContextDapper
  {
    private string _connectionString;
    private IConfiguration _config;

    public DataContextDapper(IConfiguration config){
      _connectionString = config.GetConnectionString("DefaultConnection")??"";
      _config = config;
    }
    public IEnumerable<T> LoadData<T>(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_connectionString);
      return dbConnection.Query<T>(sql);
    }
    public T LoadSingleData<T>(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_connectionString);
      return dbConnection.QuerySingle<T>(sql);

    }

    public bool ExecuteSql(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_connectionString);
      return (dbConnection.Execute(sql) > 0);
    }

    public int ExecuteSqlWithRowCount(string sql)
    {
      IDbConnection dbConnection = new SqlConnection(_connectionString);
      return dbConnection.Execute(sql);
    }

    public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
    {
      SqlCommand commandWithParams = new SqlCommand(sql);

      foreach (SqlParameter parameter in parameters)
      {
        commandWithParams.Parameters.Add(parameter);
      }

      SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
      dbConnection.Open();

      commandWithParams.Connection = dbConnection;

      int rowsAffected = commandWithParams.ExecuteNonQuery();

      dbConnection.Close();

      return rowsAffected > 0;
    }

  }
}