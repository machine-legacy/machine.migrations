using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using CommandLine.Text;
using Machine.Migrations.Services;

namespace Machine.Migrations.ConsoleRunner
{
  public class Options
  {
    [Option("c", "connection-string", HelpText = "The connection string to the database to migrate", Required = true)] 
    public string ConnectionString = string.Empty;

    /*
    [Option("a", "apply", HelpText = "Applies only the specified migration", MutuallyExclusiveSet = "WhatToDo")] 
    public string ApplyMigration = string.Empty;
    */

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
    public string[] References = new string[] {};

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
  }
}