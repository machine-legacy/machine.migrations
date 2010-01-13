using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using System.Linq;

namespace Machine.Migrations.ConsoleRunner
{
  public class Options
  {
    [OptionList("c", "connection-string", HelpText = "The connection string to the database to migrate", Required = true, Separator = ',')] 
    public List<string> ConnectionString = new List<string>();

    [Option("t", "to", HelpText = "Applies or unapplies migrations to get to the specified migration", MutuallyExclusiveSet = "WhatToDo")] 
    public long ToMigration = -1;

    [Option("u", "up", HelpText = "Applies migrations to the latest", MutuallyExclusiveSet = "WhatToDo")] 
    public bool Up = false;

    [Option("s", "scope", HelpText="The scope?")]
    public string Scope;

    [Option("d", "directory", HelpText = "Directory containing the migrations", Required = true)]
    public string MigrationsDirectory;

    [Option("v", "compiler-version", HelpText = "Version of the compiler to use")]
    public string CompilerVersion;

    [Option(null, "debug", HelpText="Show Diagnostics")]
    public bool ShowDiagnostics = false;

    [OptionList("r", "references", HelpText="Assemblies to reference while building migrations separated by commas", Separator = ',')]
    public List<string> References = new List<string>();

    [Option("t", "timeout", HelpText = "Default command timeout for migrations")]
    public int CommandTimeout = 60;

    [HelpOption("?", "help", HelpText = "Display this help screen")]
    public string GetUsage()
    {
      HelpText help = new HelpText(@"Machine.Migrations");
      help.Copyright = new CopyrightInfo("Machine Project", 2007, 2008, 2009);
      help.AddOptions(this);
      return help;
    }

    public virtual bool ParseArguments(string[] args)
    {
      var parser = new CommandLineParser();

      return parser.ParseArguments(args, this, Console.Error);
    }

    public IDictionary<string, string> ParseConnectionStrings()
    {
      return ConnectionString.Select(k => {
        var fields = k.Split('|');
        if (fields.Length == 1)
          return new[] { string.Empty, k };
        return fields;
      }).ToDictionary(k => k[0], v => v[1]);
    }
  }
}