using System.Data;

namespace Machine.Migrations.Services
{
  public interface IConnectionProvider
  {
    void OpenConnection();
    void UseConfiguration(string key);
    IDbConnection CurrentConnection
    {
      get;
    }
  }
}