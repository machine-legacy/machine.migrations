using System.Data;

namespace Machine.Migrations.Services
{
  public interface IConnectionProvider
  {
    void OpenConnection();
    IDbConnection CurrentConnection
    {
      get;
    }
  }
}