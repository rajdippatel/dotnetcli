using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	///		This is a collection of tests that test real world applications
	///		command lines focusing on options with long and short names.
	/// </summary>
	[TestFixture]
	public class LongOptionWithShortTest
	{
		[Test]
		public void LongOptWithShortTest()
		{
			Option help = new Option( "h", "help", false, "print this message" );
			Option version = new Option(
				"v", "version", false, "print version information" );
			Option newRun = new Option(
				"n", "new", false, "Create NLT cache entries only for new items" );
			Option trackerRun = new Option(
				"t", "tracker", false, "Create NLT cache entries only for tracker items" );

			Option timeLimit =
				OptionBuilder.Factory.WithLongOpt( "limit" ).HasArg().WithValueSeparator().
					WithDescription( "Set time limit for execution, in minutes" ).Create( "l" );

			Option age =
				OptionBuilder.Factory.WithLongOpt( "age" ).HasArg().WithValueSeparator().
					WithDescription( "Age (in days) of cache item before being recomputed" ).
					Create( "a" );

			Option server =
				OptionBuilder.Factory.WithLongOpt( "server" ).HasArg().WithValueSeparator().
					WithDescription( "The NLT server address" ).Create( "s" );

			Option numResults =
				OptionBuilder.Factory.WithLongOpt( "results" ).HasArg().WithValueSeparator()
					.WithDescription( "Number of results per item" ).Create( "r" );

			Option configFile =
				OptionBuilder.Factory.WithLongOpt( "file" ).HasArg().WithValueSeparator().
					WithDescription( "Use the specified configuration file" ).Create();

			Options options = new Options();
			options.AddOption( help );
			options.AddOption( version );
			options.AddOption( newRun );
			options.AddOption( trackerRun );
			options.AddOption( timeLimit );
			options.AddOption( age );
			options.AddOption( server );
			options.AddOption( numResults );
			options.AddOption( configFile );

			// create the command line parser
			ICommandLineParser parser = new PosixParser();

			string[] args = new[] { "-v", "-l", "10", "-age", "5", "-file", "filename" };

			try
			{
				CommandLine line = parser.Parse( options, args );
				Assert.IsTrue( line.HasOption( "v" ) );
				Assert.AreEqual( "10", line.GetOptionValue( "l" ) );
				Assert.AreEqual( "10", line.GetOptionValue( "limit" ) );
				Assert.AreEqual( "5", line.GetOptionValue( "a" ) );
				Assert.AreEqual( "5", line.GetOptionValue( "age" ) );
				Assert.AreEqual( "filename", line.GetOptionValue( "file" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Unexpected exception: " + e.Message );
			}
		}
	}
}