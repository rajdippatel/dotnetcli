using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	///		<para>This is a collection of tests that test real world 
	///		applications command lines.</para>
	///		<para>The following are the applications that are tested: Ant</para>
	/// </summary>
	[TestFixture]
	public class ApplicationTest
	{
		[Test]
		public void AntTest()
		{
			// use the GNU parser
			ICommandLineParser parser = new GnuParser();
			var options = new Options();
			options.AddOption( "help", false, "print this message" );
			options.AddOption( "projecthelp", false, "print project help information" );
			options.AddOption(
				"version", false, "print the version information and exit" );
			options.AddOption( "quiet", false, "be extra quiet" );
			options.AddOption( "verbose", false, "be extra verbose" );
			options.AddOption( "debug", false, "print debug information" );
			//options.AddOption( "version", false, 
			//	"produce logging information without adornments" );
			options.AddOption( "logfile", true, "use given file for log" );
			options.AddOption(
				"logger", true, "the class which is to perform the logging" );
			options.AddOption(
				"listener", true, "add an instance of a class as a project listener" );
			options.AddOption( "buildfile", true, "use given buildfile" );
			options.AddOption(
				OptionBuilder.Factory.WithDescription( "use value for given property" ).
					HasArgs().WithValueSeparator().Create( 'D' ) );

			//, null, true, , false, true );
			options.AddOption(
				"find",
				true,
				"search for buildfile towards the root " + "of the filesystem and use it" );

			var args = new[] {
			                 	"-buildfile", "mybuild.xml", "-Dproperty=value",
			                 	"-Dproperty1=value1",
			                 	"-projecthelp"
			                 };

			try
			{
				CommandLine line = parser.Parse( options, args );

				// check multiple values
				string[] opts = line.GetOptionValues( "D" );
				Assert.IsTrue( opts != null, "opts is not null" );
				Assert.IsTrue( opts.Length == 4, "opts has two values" );
				Assert.AreEqual( "property", opts[ 0 ] );
				Assert.AreEqual( "value", opts[ 1 ] );
				Assert.AreEqual( "property1", opts[ 2 ] );
				Assert.AreEqual( "value1", opts[ 3 ] );

				// check single value
				Assert.AreEqual( "mybuild.xml", line.GetOptionValue( "buildfile" ) );

				// check option
				Assert.IsTrue( line.HasOption( "projecthelp" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Unexpected exception: " + e.Message );
			}
		}

		[Test]
		public void LsTest()
		{
			// create the command line parser
			ICommandLineParser parser = new PosixParser();
			var options = new Options();
			options.AddOption( "a", "all", false, "do not hide entries starting with ." );
			options.AddOption( "A", "almost-all", false, "do not list implied . and .." );
			options.AddOption(
				"b", "escape", false, "print octal escapes for nongraphic characters" );
			options.AddOption(
				OptionBuilder.Factory.WithLongOpt( "block-size" ).WithDescription(
					"use SIZE-byte blocks" ).WithValueSeparator( '=' ).HasArg().Create() );
			options.AddOption(
				"B", "ignore-backups", false, "do not list implied entried ending with ~" );
			options.AddOption(
				"c",
				false,
				"with -lt: sort by, and show, ctime " +
				"(time of last modification of file status information) " +
				"with -l:show ctime and sort by name otherwise: sort by ctime" );
			options.AddOption( "C", false, "list entries by columns" );

			var args = new[] { "--block-size=10" };

			try
			{
				CommandLine line = parser.Parse( options, args );
				Assert.IsTrue( line.HasOption( "block-size" ) );
				Assert.AreEqual( "10", line.GetOptionValue( "block-size" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Unexpected exception: " + e.Message );
			}
		}
	}
}