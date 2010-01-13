using System;
using System.Linq;
using log4net.Appender;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;
using Machine.Migrations.Services;
using Machine.Migrations.Services.Impl;

namespace Machine.Migrations.ConsoleRunner
{
  public class Program
  {
    public Program(IConsole console)
    {
    }

    public static void Main(string[] args)
    {
      var appender = new ConsoleAppender() { Layout = new log4net.Layout.PatternLayout("%-5p %x %m") };
      appender.ActivateOptions();

      log4net.Config.BasicConfigurator.Configure(appender);

      var program = new Program(new DefaultConsole());
      ExitCode exitCode = program.Run(args);

      Environment.Exit((int)exitCode);
    }

    public ExitCode Run(string[] args)
    {
      try
      {
        var options = new Options();
        options.ParseArguments(args);

        var task = new Migrator();
        task.Run(new Configuration(options));
      }
      catch (Exception err)
      {
        Console.Error.WriteLine(err.ToString());
        return ExitCode.Failure;
      }

      return ExitCode.Success;
    }
  }

  public enum ExitCode
  {
    Success = 0,
    Failure = 1
  }

  public interface IConsole
  {
  }

  public class DefaultConsole : IConsole
  {
  }

  public class Configuration : IConfiguration
  {
    public Configuration(Options options)
    {
      this.Scope = options.Scope;
      this.ConnectionProviderType = typeof(SqlServerConnectionProvider);
      this.TransactionProviderType = typeof(TransactionProvider);
      this.SchemaProviderType = typeof(SqlServerSchemaProvider);
      this.DatabaseProviderType = typeof(SqlServerDatabaseProvider);
      this.MigrationsDirectory = options.MigrationsDirectory;
      Console.WriteLine("Migrations dir: " + options.MigrationsDirectory);
      this.CompilerVersion = options.CompilerVersion;
      this.DesiredVersion = options.ToMigration;
      this.ShowDiagnostics = options.ShowDiagnostics;
      this.References = options.References;
      this.CommandTimeout = options.CommandTimeout;
      this.ConnectionString = options.ConnectionString;
    }

    public string Scope { get; set; }
    public Type ConnectionProviderType { get; set; }
    public Type TransactionProviderType { get; set; }
    public Type SchemaProviderType { get; set; }
    public Type DatabaseProviderType { get; set; }
    public string ConnectionString { get; set; }
    public string ActiveConfigurationKey { get; set; }
    public string ConnectionStringByKey(string key)
    {
      return ConnectionString;
    }

    public string MigrationsDirectory { get; set; }
    public string CompilerVersion { get; set; }
    public long DesiredVersion { get; set; }
    public bool ShowDiagnostics { get; set; }
    public string[] References { get; set; }
    public int CommandTimeout { get; set; }

    public void SetCommandTimeout(int commandTimeout)
    {
      this.CommandTimeout = commandTimeout;
    }
  }
}
