using System;
using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	/// Summary description for ValuesTest
	/// </summary>
	[TestFixture]
	public class ValuesTest
	{
		/** CommandLine instance */
		private readonly CommandLine m_cmdline;
		private readonly Option m_option;

		public ValuesTest()
		{
			var opts = new Options();

			opts.AddOption( "a", false, "toggle -a" );
			opts.AddOption( "b", true, "set -b" );
			opts.AddOption( "c", "c", false, "toggle -c" );

			opts.AddOption( "d", "d", true, "set -d" );

			opts.AddOption(
				OptionBuilder.Factory.WithLongOpt(
					"e" ).HasArgs().WithDescription(
					"set -e " ).Create( 'e' ) );

			opts.AddOption( "f", "f", false, "jk" );

			opts.AddOption(
				OptionBuilder.Factory.WithLongOpt( "g" ).HasArgs( 2 ).WithDescription(
					"set -g" ).Create( 'g' ) );

			opts.AddOption(
				OptionBuilder.Factory.WithLongOpt( "h" ).HasArgs( 2 ).WithDescription(
					"set -h" ).Create( 'h' ) );

			opts.AddOption(
				OptionBuilder.Factory.WithLongOpt( "i" ).WithDescription( "set -i" ).Create(
					'i' ) );

			opts.AddOption(
				OptionBuilder.Factory.WithLongOpt( "j" ).HasArgs().WithDescription(
					"set -j" ).WithValueSeparator( '=' ).Create( 'j' ) );

			opts.AddOption(
				OptionBuilder.Factory.WithLongOpt( "k" ).HasArgs().WithDescription(
					"set -k" ).WithValueSeparator( '=' ).Create( 'k' ) );

			m_option =
				OptionBuilder.Factory.WithLongOpt( "m" ).HasArgs().WithDescription(
					"set -m" ).WithValueSeparator().Create( 'm' );

			opts.AddOption( m_option );

			var args = new[] {
			                 	"-a",
			                 	"-b", "foo",
			                 	"--c",
			                 	"--d", "bar",
			                 	"-e", "one", "two",
			                 	"-f", "arg1",
			                 	"arg2", "-g", "val1", "val2", "arg3", "-h", "val1", "-i",
			                 	"-h", "val2",
			                 	"-jkey=value", "-j", "key=value", "-kkey1=value1",
			                 	"-kkey2=value2",
			                 	"-mkey=value"
			                 };

			ICommandLineParser parser = new PosixParser();

			try
			{
				m_cmdline = parser.Parse( opts, args );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setup CommandLine: " + e.Message );
			}
		}

		private bool CompareArrays<T>( T[] arrayA, T[] arrayB )
		{
			if ( arrayA.Length != arrayB.Length ) return false;

			Array.Sort( arrayA );
			Array.Sort( arrayB );

			for ( int x = 0; x < arrayA.Length; ++x ) if ( !arrayA[ x ].Equals( arrayB[ x ] ) ) return false;

			return true;
		}

		[Test]
		public void CharSeparatorTest()
		{
			// tests the char methods of CommandLine that delegate to
			// the string methods
			var values = new[] { "key", "value", "key", "value" };
			Assert.IsTrue( m_cmdline.HasOption( "j" ) );
			Assert.IsTrue( m_cmdline.HasOption( 'j' ) );
			Assert.AreEqual( 4, m_cmdline.GetOptionValues( "j" ).Length );
			Assert.AreEqual( 4, m_cmdline.GetOptionValues( 'j' ).Length );
			Assert.IsTrue( CompareArrays( values, m_cmdline.GetOptionValues( "j" ) ) );
			Assert.IsTrue( CompareArrays( values, m_cmdline.GetOptionValues( 'j' ) ) );

			values = new[] { "key1", "value1", "key2", "value2" };
			Assert.IsTrue( m_cmdline.HasOption( "k" ) );
			Assert.IsTrue( m_cmdline.HasOption( 'k' ) );
			Assert.IsTrue( m_cmdline.GetOptionValues( "k" ).Length == 4 );
			Assert.IsTrue( m_cmdline.GetOptionValues( 'k' ).Length == 4 );
			Assert.IsTrue( CompareArrays( values, m_cmdline.GetOptionValues( "k" ) ) );
			Assert.IsTrue( CompareArrays( values, m_cmdline.GetOptionValues( 'k' ) ) );

			values = new[] { "key", "value" };
			Assert.IsTrue( m_cmdline.HasOption( "m" ) );
			Assert.IsTrue( m_cmdline.HasOption( 'm' ) );
			Assert.IsTrue( m_cmdline.GetOptionValues( "m" ).Length == 2 );
			Assert.IsTrue( m_cmdline.GetOptionValues( 'm' ).Length == 2 );
			Assert.IsTrue( CompareArrays( values, m_cmdline.GetOptionValues( "m" ) ) );
			Assert.IsTrue( CompareArrays( values, m_cmdline.GetOptionValues( 'm' ) ) );
		}

		[Test]
		public void ComplexValuesTest()
		{
			string[] result = m_cmdline.GetOptionValues( "h" );
			var values = new[] { "val1", "val2" };
			Assert.IsTrue( m_cmdline.HasOption( "i" ) );
			Assert.IsTrue( m_cmdline.HasOption( "h" ) );
			Assert.IsTrue( m_cmdline.GetOptionValues( "h" ).Length == 2 );
			Assert.IsTrue( CompareArrays( values, m_cmdline.GetOptionValues( "h" ) ) );
		}

		[Test]
		public void ExtraArgsTest()
		{
			var args = new[] { "arg1", "arg2", "arg3" };
			Assert.IsTrue( m_cmdline.Args.Length == 3 );
			Assert.IsTrue( CompareArrays( args, m_cmdline.Args ) );
		}

		[Test]
		public void MultipleArgValuesTest()
		{
			string[] result = m_cmdline.GetOptionValues( "e" );
			var values = new[] { "one", "two" };
			Assert.IsTrue( m_cmdline.HasOption( "e" ) );
			Assert.IsTrue( m_cmdline.GetOptionValues( "e" ).Length == 2 );
			Assert.IsTrue( CompareArrays( values, m_cmdline.GetOptionValues( "e" ) ) );
		}

		[Test]
		public void ShortArgsTest()
		{
			Assert.IsTrue( m_cmdline.HasOption( "a" ) );
			Assert.IsTrue( m_cmdline.HasOption( "c" ) );

			Assert.IsNull( m_cmdline.GetOptionValues( "a" ) );
			Assert.IsNull( m_cmdline.GetOptionValues( "c" ) );
		}

		[Test]
		public void ShortArgsWithValueTest()
		{
			Assert.IsTrue( m_cmdline.HasOption( "b" ) );
			Assert.IsTrue( m_cmdline.GetOptionValue( "b" ).Equals( "foo" ) );
			Assert.IsTrue( m_cmdline.GetOptionValues( "b" ).Length == 1 );

			Assert.IsTrue( m_cmdline.HasOption( "d" ) );
			Assert.IsTrue( m_cmdline.GetOptionValue( "d" ).Equals( "bar" ) );
			Assert.IsTrue( m_cmdline.GetOptionValues( "d" ).Length == 1 );
		}

		[Test]
		public void TwoArgValuesTest()
		{
			string[] result = m_cmdline.GetOptionValues( "g" );
			var values = new[] { "val1", "val2" };
			Assert.IsTrue( m_cmdline.HasOption( "g" ) );
			Assert.IsTrue( m_cmdline.GetOptionValues( "g" ).Length == 2 );
			Assert.IsTrue( CompareArrays( values, m_cmdline.GetOptionValues( "g" ) ) );
		}
	}
}