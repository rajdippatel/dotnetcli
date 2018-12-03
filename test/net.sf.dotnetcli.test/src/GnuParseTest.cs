using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	/// Summary description for GnuParseTest
	/// </summary>
	[TestFixture]
	public class GnuParseTest
	{
		private readonly Options m_options;
		private readonly Parser m_parser;

		public GnuParseTest()
		{
			m_options =
				new Options().AddOption( "a", "enable-a", false, "turn [a] on or off" ).
					AddOption( "b", "bfile", true, "set the value of [b]" ).AddOption(
					"c", "copt", false, "turn [c] on or off" );

			m_parser = new GnuParser();
		}

		[Test]
		public void DoubleDashTest()
		{
			string[] args = new[] { "--copt", "--", "-b", "toast" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );

				Assert.IsTrue( cl.HasOption( "c" ), "Confirm -c is set" );
				Assert.IsTrue( !cl.HasOption( "b" ), "Confirm -b is not set" );
				Assert.IsTrue(
					cl.ArgList.Count == 2,
					string.Format( "Confirm 2 extra args: {0}", cl.ArgList.Count ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void ExtraOptionTest()
		{
			string[] args = new[] { "-a", "-d", "-b", "toast", "foo", "bar" };

			bool caught = false;

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );
				Assert.IsTrue( cl.HasOption( "a" ), "Confirm -a is set" );
				Assert.IsTrue( cl.HasOption( "b" ), "Confirm -b is set" );
				Assert.IsTrue(
					cl.GetOptionValue( "b" ).Equals( "toast" ), "Confirm arg of -b" );
				Assert.IsTrue( cl.ArgList.Count == 3, "Confirm size of extra args" );
			}
			catch ( UnrecognizedOptionException )
			{
				caught = true;
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
			Assert.IsTrue( caught, "Confirm UnrecognizedOptionException caught" );
		}

		[Test]
		public void MissingArgTest()
		{
			string[] args = new[] { "-b" };

			bool caught = false;

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );
			}
			catch ( MissingArgumentException )
			{
				caught = true;
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}

			Assert.IsTrue( caught, "Confirm MissingArgumentException caught" );
		}

		[Test]
		public void MultipleTest()
		{
			string[] args = new[] { "-c", "foobar", "-b", "toast" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args, true );
				Assert.IsTrue( cl.HasOption( "c" ), "Confirm -c is set" );
				Assert.IsTrue(
					cl.ArgList.Count == 3,
					string.Format( "Confirm 3 extra args: {0}", cl.ArgList.Count ) );

				cl = m_parser.Parse( m_options, cl.Args );

				Assert.IsTrue( !cl.HasOption( "c" ), "Confirm -c is not set" );
				Assert.IsTrue( cl.HasOption( "b" ), "Confirm -b is set" );
				Assert.IsTrue(
					cl.GetOptionValue( "b" ).Equals( "toast" ), "Confirm arg of -b" );
				Assert.IsTrue(
					cl.ArgList.Count == 1,
					string.Format( "Confirm 1 extra arg: {0}", cl.ArgList.Count ) );
				Assert.IsTrue(
					cl.ArgList[ 0 ].Equals( "foobar" ),
					"Confirm value of exra arg: " + cl.ArgList[ 0 ] );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void MultipleWithLongTest()
		{
			string[] args = new[] { "--copt", "foobar", "--bfile", "toast" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args, true );
				Assert.IsTrue( cl.HasOption( "c" ), "Confirm -c is set" );
				Assert.IsTrue(
					cl.ArgList.Count == 3,
					string.Format( "Confirm 3 extra args: {0}", cl.ArgList.Count ) );

				cl = m_parser.Parse( m_options, cl.Args );

				Assert.IsTrue( !cl.HasOption( "c" ), "Confirm -c is not set" );
				Assert.IsTrue( cl.HasOption( "b" ), "Confirm -b is set" );
				Assert.IsTrue(
					cl.GetOptionValue( "b" ).Equals( "toast" ), "Confirm arg of -b" );
				Assert.IsTrue(
					cl.ArgList.Count == 1,
					string.Format( "Confirm 1 extra arg: {0}", cl.ArgList.Count ) );
				Assert.IsTrue(
					cl.ArgList[ 0 ].Equals( "foobar" ),
					"Confirm value of exra arg: " + cl.ArgList[ 0 ] );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void SimpleLongTest()
		{
			string[] args = new[] { "--enable-a", "--bfile", "toast", "foo", "bar" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );
				Assert.IsTrue( cl.HasOption( "a" ), "Confirm -a is set" );
				Assert.IsTrue( cl.HasOption( "b" ), "Confirm -b is set" );
				Assert.IsTrue(
					cl.GetOptionValue( "b" ).Equals( "toast" ), "Confirm arg of -b" );
				Assert.IsTrue( cl.ArgList.Count == 2, "Confirm size of extra args" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void SimpleShortTest()
		{
			string[] args = new[] { "-a", "-b", "toast", "foo", "bar" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );
				Assert.IsTrue( cl.HasOption( "a" ), "Confirm -a is set" );
				Assert.IsTrue( cl.HasOption( "b" ), "Confirm -b is set" );
				Assert.IsTrue(
					cl.GetOptionValue( "b" ).Equals( "toast" ), "Confirm arg of -b" );
				Assert.IsTrue( cl.ArgList.Count == 2, "Confirm size of extra args" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void SingleDashTest()
		{
			string[] args = new[] { "--copt", "-b", "-", "-a", "-" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );

				Assert.IsTrue( cl.HasOption( "a" ), "Confirm -a is set" );
				Assert.IsTrue( cl.HasOption( "b" ), "Confirm -b is set" );
				Assert.IsTrue( cl.GetOptionValue( "b" ).Equals( "-" ), "Confirm arg of -b" );
				Assert.IsTrue(
					cl.ArgList.Count == 1,
					string.Format( "Confirm 1 extra arg: {0}", cl.ArgList.Count ) );
				Assert.IsTrue(
					cl.ArgList[ 0 ].Equals( "-" ),
					"Confirm value of extra arg: " + cl.ArgList[ 0 ] );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void StopTest()
		{
			string[] args = new[] { "-c", "foober", "-b", "toast" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args, true );
				Assert.IsTrue( cl.HasOption( "c" ), "Confirm -c is set" );
				Assert.IsTrue(
					cl.ArgList.Count == 3,
					string.Format( "Confirm 3 extra args: {0}", cl.ArgList.Count ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
		}
	}
}