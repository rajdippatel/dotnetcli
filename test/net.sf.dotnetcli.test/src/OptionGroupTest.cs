using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	/// Summary description for OptionGroupTest
	/// </summary>
	[TestFixture]
	public class OptionGroupTest
	{
		private readonly Options m_options;
		private readonly ICommandLineParser m_parser = new PosixParser();

		public OptionGroupTest()
		{
			Option file = new Option( "f", "file", false, "file to process" );
			Option dir = new Option( "d", "directory", false, "directory to process" );
			OptionGroup group = new OptionGroup();
			group.AddOption( file );
			group.AddOption( dir );
			m_options = new Options().AddOptionGroup( group );

			Option section = new Option( "s", "section", false, "section to process" );
			Option chapter = new Option( "c", "chapter", false, "chapter to process" );
			OptionGroup group2 = new OptionGroup();
			group2.AddOption( section );
			group2.AddOption( chapter );

			m_options.AddOptionGroup( group2 );

			Option importOpt = new Option( null, "import", false, "section to process" );
			Option exportOpt = new Option( null, "export", false, "chapter to process" );
			OptionGroup group3 = new OptionGroup();
			group3.AddOption( importOpt );
			group3.AddOption( exportOpt );
			m_options.AddOptionGroup( group3 );

			m_options.AddOption( "r", "revision", false, "revision number" );
		}

		[Test]
		public void NoOptionsExtraArgsTest()
		{
			string[] args = new[] { "arg1", "arg2" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );

				Assert.IsTrue( !cl.HasOption( "r" ), "Confirm -r is NOT set" );
				Assert.IsTrue( !cl.HasOption( "f" ), "Confirm -f is NOT set" );
				Assert.IsTrue( !cl.HasOption( "d" ), "Confirm -d is NOT set" );
				Assert.IsTrue( !cl.HasOption( "s" ), "Confirm -s is NOT set" );
				Assert.IsTrue( !cl.HasOption( "c" ), "Confirm -c is NOT set" );
				Assert.IsTrue( cl.ArgList.Count == 2, "Confirm TWO extra args" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void SingleLongOptionTest()
		{
			string[] args = new[] { "--file" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );

				Assert.IsTrue( !cl.HasOption( "r" ), "Confirm -r is NOT set" );
				Assert.IsTrue( cl.HasOption( "f" ), "Confirm -f is set" );
				Assert.IsTrue( !cl.HasOption( "d" ), "Confirm -d is NOT set" );
				Assert.IsTrue( !cl.HasOption( "s" ), "Confirm -s is NOT set" );
				Assert.IsTrue( !cl.HasOption( "c" ), "Confirm -c is NOT set" );
				Assert.IsTrue( cl.ArgList.Count == 0, "Confirm no extra args" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void SingleOptionFromGroupTest()
		{
			string[] args = new[] { "-f" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );

				Assert.IsTrue( !cl.HasOption( "r" ), "Confirm -r is NOT set" );
				Assert.IsTrue( cl.HasOption( "f" ), "Confirm -f is set" );
				Assert.IsTrue( !cl.HasOption( "d" ), "Confirm -d is NOT set" );
				Assert.IsTrue( !cl.HasOption( "s" ), "Confirm -s is NOT set" );
				Assert.IsTrue( !cl.HasOption( "c" ), "Confirm -c is NOT set" );
				Assert.IsTrue( cl.ArgList.Count == 0, "Confirm no extra args" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void SingleOptionTest()
		{
			string[] args = new[] { "-r" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );

				Assert.IsTrue( cl.HasOption( "r" ), "Confirm -r is set" );
				Assert.IsTrue( !cl.HasOption( "f" ), "Confirm -f is NOT set" );
				Assert.IsTrue( !cl.HasOption( "d" ), "Confirm -d is NOT set" );
				Assert.IsTrue( !cl.HasOption( "s" ), "Confirm -s is NOT set" );
				Assert.IsTrue( !cl.HasOption( "c" ), "Confirm -c is NOT set" );
				Assert.IsTrue( cl.ArgList.Count == 0, "Confirm no extra args" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void TwoLongOptionsFromGroupTest()
		{
			string[] args = new[] { "--file", "--directory" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );
				Assert.Fail( "two arguments from group not allowed" );
			}
			catch ( ParseException e )
			{
				if ( !( e is AlreadySelectedException ) )
				{
					Assert.Fail( "incorrect exception caught:" + e.Message );
				}
			}
		}

		[Test]
		public void TwoOptionsFromDifferentGroupTest()
		{
			string[] args = new[] { "-f", "-s" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );

				Assert.IsTrue( !cl.HasOption( "r" ), "Confirm -r is NOT set" );
				Assert.IsTrue( cl.HasOption( "f" ), "Confirm -f is set" );
				Assert.IsTrue( !cl.HasOption( "d" ), "Confirm -d is NOT set" );
				Assert.IsTrue( cl.HasOption( "s" ), "Confirm -s is set" );
				Assert.IsTrue( !cl.HasOption( "c" ), "Confirm -c is NOT set" );
				Assert.IsTrue( cl.ArgList.Count == 0, "Confirm no extra args" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void TwoOptionsFromGroupTest()
		{
			string[] args = new[] { "-f", "-d" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );
				Assert.Fail( "two arguments from group not allowed" );
			}
			catch ( ParseException e )
			{
				if ( !( e is AlreadySelectedException ) )
				{
					Assert.Fail( "incorrect exception caught:" + e.Message );
				}
			}
		}

		[Test]
		public void TwoValidLongOptionsTest()
		{
			string[] args = new[] { "--revision", "--file" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );

				Assert.IsTrue( cl.HasOption( "r" ), "Confirm -r is set" );
				Assert.IsTrue( cl.HasOption( "f" ), "Confirm -f is set" );
				Assert.IsTrue( !cl.HasOption( "d" ), "Confirm -d is NOT set" );
				Assert.IsTrue( !cl.HasOption( "s" ), "Confirm -s is NOT set" );
				Assert.IsTrue( !cl.HasOption( "c" ), "Confirm -c is NOT set" );
				Assert.IsTrue( cl.ArgList.Count == 0, "Confirm no extra args" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void TwoValidOptionsTest()
		{
			string[] args = new[] { "-r", "-f" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );

				Assert.IsTrue( cl.HasOption( "r" ), "Confirm -r is set" );
				Assert.IsTrue( cl.HasOption( "f" ), "Confirm -f is set" );
				Assert.IsTrue( !cl.HasOption( "d" ), "Confirm -d is NOT set" );
				Assert.IsTrue( !cl.HasOption( "s" ), "Confirm -s is NOT set" );
				Assert.IsTrue( !cl.HasOption( "c" ), "Confirm -c is NOT set" );
				Assert.IsTrue( cl.ArgList.Count == 0, "Confirm no extra args" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void ValidLongOnlyOptionsTest()
		{
			try
			{
				CommandLine cl = m_parser.Parse( m_options, new[] { "--export" } );
				Assert.IsTrue( cl.HasOption( "export" ), "Confirm --export is set" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}

			try
			{
				CommandLine cl = m_parser.Parse( m_options, new[] { "--import" } );
				Assert.IsTrue( cl.HasOption( "import" ), "Confirm --import is set" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}
	}
}