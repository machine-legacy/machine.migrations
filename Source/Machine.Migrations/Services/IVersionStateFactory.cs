using System.Collections.Generic;

using Machine.Migrations.Core;

namespace Machine.Migrations.Services
{
  public interface IVersionStateFactory
  {
    IDictionary<string, VersionState> CreateVersionState(ICollection<MigrationReference> migrations);
  }
}