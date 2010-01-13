using System.Collections.Generic;
using System.Linq;

namespace Machine.Migrations.Services.Impl
{
  public class MigrationSelector : IMigrationSelector
  {
    #region Member Data
    readonly IMigrationFinder _migrationFinder;
    readonly IVersionStateFactory _versionStateFactory;
    #endregion

    #region MigrationSelector()
    public MigrationSelector(IMigrationFinder migrationFinder, IVersionStateFactory versionStateFactory)
    {
      _versionStateFactory = versionStateFactory;
      _migrationFinder = migrationFinder;
    }
    #endregion

    #region IMigrationSelector Members
    public IDictionary<string, List<MigrationStep>> SelectMigrations()
    {
      var all = _migrationFinder.FindMigrations();
      var selected = new Dictionary<string, List<MigrationStep>>();
      selected[string.Empty] = new List<MigrationStep>();
      if (all.Count == 0)
      {
        return selected;
      }
      var versionStates = _versionStateFactory.CreateVersionState(all);
      foreach (var state in versionStates)
      {
        selected[state.Key] = new List<MigrationStep>();
      }
      foreach (var migration in all)
      {
        var version = versionStates[migration.ConfigurationKey];
        if (version.IsApplicable(migration))
        {
          var step = new MigrationStep(migration, version.IsReverting);
          selected[migration.ConfigurationKey].Add(step);
        }
      }
      foreach (var pair in versionStates)
      {
        if (pair.Value.IsReverting)
        {
          selected[pair.Key].Reverse();
        }
      }
      return selected;
    }
    #endregion
  }
}