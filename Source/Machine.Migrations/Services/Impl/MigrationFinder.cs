using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

using Machine.Core.Services;

namespace Machine.Migrations.Services.Impl
{
  public class MigrationFinder : IMigrationFinder
  {
    #region Member Data
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(MigrationFinder));
    readonly Regex _regex = new Regex(@"^(\d+)_([\w_]+)\.(cs|boo|sql)$");
    readonly IConfiguration _configuration;
    readonly IFileSystem _fileSystem;
    readonly INamer _namer;
    #endregion

    #region MigrationFinder()
    public MigrationFinder(IFileSystem fileSystem, INamer namer, IConfiguration configuration)
    {
      _fileSystem = fileSystem;
      _namer = namer;
      _configuration = configuration;
    }
    #endregion

    #region IMigrationFinder Members
    public ICollection<MigrationReference> FindMigrations()
    {
      var migrations = new Dictionary<string, MigrationReference>();
      foreach (var match in FindFiles(_configuration.MigrationsDirectory))
      {
        var m = match.Match;
        var file = Path.GetFileName(match.Path);
        var migration = new MigrationReference(Int64.Parse(m.Groups[1].Value), _namer.ToCamelCase(m.Groups[2].Value), file, match.ConfigurationKey);

        if (migrations.ContainsKey(migration.ConfigurationKey + migration.Version))
        {
          throw new DuplicateMigrationVersionException("Duplicate Version " + migration.Version);
        }

        migrations.Add(migration.ConfigurationKey + migration.Version, migration);
      }
      var sortedMigrations = new List<MigrationReference>(migrations.Values);
      sortedMigrations.Sort((mr1, mr2) => mr1.Version.CompareTo(mr2.Version));

      if (sortedMigrations.Count == 0)
      {
        _log.InfoFormat("Found {0} migrations in '{1}'!", migrations.Count, _configuration.MigrationsDirectory);
      }
      return sortedMigrations;
    }

    IEnumerable<MigrationFile> FindFiles(string directory)
    {
      var files = FindFiles(directory, string.Empty);
      foreach (var subDirectory in _fileSystem.GetDirectories(directory))
      {
        files = files.Union(FindFiles(subDirectory, Path.GetFileName(subDirectory)));
      }
      return files;
    }

    IEnumerable<MigrationFile> FindFiles(string directory, string key)
    {
      foreach (var path in _fileSystem.GetFiles(directory))
      {
        var match = _regex.Match(Path.GetFileName(path));
        if (match.Success)
        {
          yield return new MigrationFile { Match = match, Path = path, ConfigurationKey = key };
        }
      }
    }
    #endregion
  }

  public class MigrationFile
  {
    public string Path { get; set; }
    public string ConfigurationKey { get; set; }
    public Match Match { get; set; }
  }
}