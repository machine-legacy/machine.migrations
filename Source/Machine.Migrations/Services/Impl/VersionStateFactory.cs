using System;
using System.Collections.Generic;
using System.Linq;

using Machine.Migrations.Core;

namespace Machine.Migrations.Services.Impl
{
  public class VersionStateFactory : IVersionStateFactory
  {
    #region Member Data
    readonly IConfiguration _configuration;
    readonly ISchemaStateManager _schemaStateManager;
    #endregion

    #region VersionStateFactory()
    public VersionStateFactory(IConfiguration configuration, ISchemaStateManager schemaStateManager)
    {
      _configuration = configuration;
      _schemaStateManager = schemaStateManager;
    }
    #endregion

    #region IVersionStateFactory Members
    public IDictionary<string, VersionState> CreateVersionState(ICollection<MigrationReference> migrations)
    {
      return migrations.Select(m => m.ConfigurationKey).Distinct().ToDictionary(k => k, v => {
        return GetVersionState(migrations);
      });
    }

    VersionState GetVersionState(ICollection<MigrationReference> migrations)
    {
      var applied = _schemaStateManager.GetAppliedMigrationVersions(_configuration.Scope);
      long desired = _configuration.DesiredVersion;
      long last = 0;
      if (migrations.Count > 0)
      {
        List<MigrationReference> all = new List<MigrationReference>(migrations);
        last = all[all.Count - 1].Version;
      }
      if (desired > last)
      {
        throw new ArgumentException("DesiredVersion is greater than maximum migration!");
      }
      if (desired < 0)
      {
        desired = last;
      }
      return new VersionState(last, desired, applied);
    }
    #endregion
  }
}