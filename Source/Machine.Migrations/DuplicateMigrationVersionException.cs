using System;
using System.Runtime.Serialization;

namespace Machine.Migrations
{
  [Serializable]
  public class DuplicateMigrationVersionException : Exception
  {
    //
    // For guidelines regarding the creation of new exception types, see
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
    // and
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
    //

    public DuplicateMigrationVersionException()
    {
    }

    public DuplicateMigrationVersionException(string message) : base(message)
    {
    }

    public DuplicateMigrationVersionException(string message, Exception inner) : base(message, inner)
    {
    }

    protected DuplicateMigrationVersionException(
      SerializationInfo info,
      StreamingContext context) : base(info, context)
    {
    }
  }
}