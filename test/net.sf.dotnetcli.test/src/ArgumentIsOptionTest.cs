using System;
using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	/// Summary description for ArgumentIsOptionTest
	/// </summary>
	[TestFixture]
	public class ArgumentIsOptionTest
	{
		private readonly Options m_options;
		private readonly ICommandLineParser m_parser;

		public ArgumentIsOptionTest()
		{
			m_options =
				new Options().AddOption( "p", false, "Option p" ).AddOption(
					"attr", true, "Option accepts argument" );
			m_parser = new PosixParser();
		}

		[Test]
		public void OptionAndOptionWithArgumentTest()
		{
			string[] args = new[] { "-p", "-attr", "p" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );
				Assert.IsTrue( cl.HasOption( "p" ), "Confirm -p is set" );
				Assert.IsTrue( cl.HasOption( "attr" ), "Confirm -attr is set" );
				Assert.IsTrue(
					cl.GetOptionValue( "attr" ).Equals( "p" ), "Confirm arg of -attr" );
				Assert.IsTrue( cl.Args.Length == 0, "Confirm all arguments recognized" );
			}
			catch ( Exception e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void OptionTest()
		{
			string[] args = new[] { "-p" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );
				Assert.IsTrue( cl.HasOption( "p" ), "Confirm -p is set" );
				Assert.IsFalse( cl.HasOption( "attr" ), "Confirm -attr is not set" );
				Assert.IsTrue( cl.Args.Length == 0, "Confirm all arguments recognized" );
			}
			catch ( Exception e )
			{
				Assert.Fail( e.Message );
			}
		}

		[Test]
		public void OptionWithArgumentTest()
		{
			string[] args = new[] { "-attr", "p" };

			try
			{
				CommandLine cl = m_parser.Parse( m_options, args );
				Assert.IsFalse( cl.HasOption( "p" ), "Confirm -p is not set" );
				Assert.IsTrue( cl.HasOption( "attr" ), "Confirm -attr is set" );
				Assert.IsTrue(
					cl.GetOptionValue( "attr" ).Equals( "p" ), "Confirm arg of -attr" );
				Assert.IsTrue( cl.Args.Length == 0, "Confirm all arguments recognized" );
			}
			catch ( Exception e )
			{
				Assert.Fail( e.Message );
			}
		}
	}
}