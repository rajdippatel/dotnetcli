using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	/// Summary description for ParseRequired
	/// </summary>
	[TestFixture]
	public class ParseRequired
	{
		private readonly Options m_options;
		private readonly ICommandLineParser m_parser = new PosixParser();

		public ParseRequired()
		{
			m_options =
				new Options().AddOption( "a", "enable-a", false, "turn [a] on or off" ).
					AddOption(
					OptionBuilder.Factory.WithLongOpt( "bfile" ).HasArg().IsRequired().
						WithDescription( "set the value of [b]" ).Create( 'b' ) );
		}

		[Test]
		public void MissingRequiredOptionTest()
		{
			string[] args = new[] { "-a" };
			try
			{
				m_parser.Parse( m_options, args );
				Assert.Fail( "did not throw MissingOptionException" );
			}
			catch ( ParseException e )
			{
				Assert.IsTrue( e is MissingOptionException );
			}
		}

		[Test]
		public void OptionAndRequiredOptionTest()
		{
			string[] args = new[] { "-a", "-b", "file" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );

				Assert.IsTrue( cl.HasOption( "a" ), "Confirm -a is set" );
				Assert.IsTrue( cl.HasOption( "b" ), "Confirm -b is set" );
				Assert.IsTrue(
					cl.GetOptionValue( "b" ).Equals( "file" ), "Confirm arg of -b" );
				Assert.IsTrue( cl.ArgList.Count == 0, "Confirm NO extra args" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void WithRequiredOptionTest()
		{
			string[] args = new[] { "-b", "file" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );

				Assert.IsTrue( !cl.HasOption( "a" ), "Confirm -a is NOT set" );
				Assert.IsTrue( cl.HasOption( "b" ), "Confirm -b is set" );
				Assert.IsTrue(
					cl.GetOptionValue( "b" ).Equals( "file" ), "Confirm arg of -b" );
				Assert.IsTrue( cl.ArgList.Count == 0, "Confirm NO extra args" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}
	}
}