using System;
using Machine.Container;

using Machine.Core.Utility;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;
using Machine.Migrations.Services;
using Machine.Migrations.Services.Impl;
using Machine.MsBuildExtensions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Machine.Migrations
{
  public class Migrator
  {
    public virtual IMigratorContainerFactory CreateContainerFactory()
    {
      return new MigratorContainerFactory();
    }

    public void Run(IConfiguration configuration)
    {
      LoggingHelper.Disable("Machine.Container");
      IMigratorContainerFactory migratorContainerFactory = CreateContainerFactory();
      using (Machine.Core.LoggingUtilities.Log4NetNdc.Push(String.Empty))
      {
        IMachineContainer container = migratorContainerFactory.CreateAndPopulateContainer(configuration);
        container.Resolve.Object<IMigrator>().RunMigrator();
      }
      
    }
  }

  public class MigratorTask : Task, IConfiguration
  {
    string _migrationsDirectory;
    string _scope;
    string _connectionString;
    long _desiredVersion;
    int _commandTimeout = 30;
    bool _diagnostics;
    string[] _references;
    string _compilerVersion;

    public MigratorTask()
    {
      _migrationsDirectory = Environment.CurrentDirectory;
    }

    public override bool Execute()
    {
      log4net.Config.BasicConfigurator.Configure(new Log4NetMsBuildAppender(this.Log, new log4net.Layout.PatternLayout("%-5p %x %m")));
      var migrator = new Migrator();
      migrator.Run(this);

      return true;
    }

    #region IConfiguration Members
    [Required]
    public string ConnectionString
    {
      get { return _connectionString; }
      set { _connectionString = value; }
    }

    public string Scope
    {
      get { return _scope; }
      set { _scope = value; }
    }

    public string ConnectionStringByKey(string key)
    {
      return _connectionString;
    }

    public string ActiveConfigurationKey
    {
      get;
      set;
    }

    public string MigrationsDirectory
    {
      get { return _migrationsDirectory; }
      set { _migrationsDirectory = value; }
    }

    public long DesiredVersion
    {
      get { return _desiredVersion; }
      set { _desiredVersion = value; }
    }

    public string CompilerVersion
    {
      get { return _compilerVersion; }
      set { _compilerVersion = value; }
    }

    public bool ShowDiagnostics
    {
      get { return _diagnostics; }
      set { _diagnostics = value; }
    }

    public string[] References
    {
      get
      {
        if (_references == null)
        {
          return new string[0];
        }
        return _references;
      }
      set { _references = value; }
    }

    public int CommandTimeout
    {
      get { return _commandTimeout; }
      set { _commandTimeout = value; }
    }

    public void SetCommandTimeout(int commandTimeout)
    {
      this.CommandTimeout = commandTimeout;
    }

    public virtual Type ConnectionProviderType
    {
      get { return typeof(SqlServerConnectionProvider); }
    }

    public virtual Type TransactionProviderType
    {
      get { return typeof(TransactionProvider); }
    }

    public virtual Type SchemaProviderType
    {
      get { return typeof(SqlServerSchemaProvider); }
    }

    public virtual Type DatabaseProviderType
    {
      get { return typeof(AdoNetDatabaseProvider); }
    }
    #endregion
  }
}