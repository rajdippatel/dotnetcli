using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	/// Summary description for ValueTest
	/// </summary>
	[TestFixture]
	public class ValueTest
	{
		#region Setup/Teardown

		[SetUp]
		public void SetUp()
		{
			m_opts = new Options();
			m_opts.AddOption( "a", false, "toggle -a" );
			m_opts.AddOption( "b", true, "set -b" );
			m_opts.AddOption( "c", "c", false, "toggle -c" );
			m_opts.AddOption( "d", "d", true, "set -d" );
			m_opts.AddOption( OptionBuilder.Factory.HasOptionalArg().Create( 'e' ) );
			m_opts.AddOption(
				OptionBuilder.Factory.HasOptionalArg().WithLongOpt( "fish" ).Create() );
			m_opts.AddOption(
				OptionBuilder.Factory.HasOptionalArgs().WithLongOpt( "gravy" ).Create() );
			m_opts.AddOption(
				OptionBuilder.Factory.HasOptionalArgs( 2 ).WithLongOpt( "hide" ).Create() );
			m_opts.AddOption( OptionBuilder.Factory.HasOptionalArgs( 2 ).Create( 'i' ) );
			m_opts.AddOption( OptionBuilder.Factory.HasOptionalArgs().Create( 'j' ) );
			m_opts.AddOption(
				OptionBuilder.Factory.HasArgs().WithValueSeparator( ',' ).Create( 'k' ) );

			var args = new[] { "-a", "-b", "foo", "--c", "--d", "bar" };

			try
			{
				Parser parser = new PosixParser();
				m_cl = parser.Parse( m_opts, args );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}

		#endregion

		private CommandLine m_cl;
		private Options m_opts;

		private bool CompareArrays<T>( T[] arrayA, T[] arrayB )
		{
			if ( arrayA.Length != arrayB.Length ) return false;

			Array.Sort( arrayA );
			Array.Sort( arrayB );

			for ( int x = 0; x < arrayA.Length; ++x )
			{
				if ( !arrayA[ x ].Equals( arrayB[ x ] ) ) return false;
			}

			return true;
		}

		[Test]
		public void LongNoArgTest()
		{
			Assert.IsTrue( m_cl.HasOption( "c" ) );
			Assert.IsNull( m_cl.GetOptionValue( "c" ) );
		}

		[Test]
		public void LongOptionalArgValuesTest()
		{
			var args = new[] { "--gravy", "gold", "garden" };
			try
			{
				Parser parser = new PosixParser();
				CommandLine cmd = parser.Parse( m_opts, args );
				Assert.IsTrue( cmd.HasOption( "gravy" ) );
				Assert.AreEqual( "gold", cmd.GetOptionValue( "gravy" ) );
				Assert.AreEqual( "gold", cmd.GetOptionValues( "gravy" )[ 0 ] );
				Assert.AreEqual( "garden", cmd.GetOptionValues( "gravy" )[ 1 ] );
				Assert.AreEqual( cmd.Args.Length, 0 );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}

		[Test]
		public void LongOptionalArgValueTest()
		{
			var args = new[] { "--fish", "face" };
			try
			{
				Parser parser = new PosixParser();
				CommandLine cmd = parser.Parse( m_opts, args );
				Assert.IsTrue( cmd.HasOption( "fish" ) );
				Assert.AreEqual( "face", cmd.GetOptionValue( "fish" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}

		[Test]
		public void LongOptionalNArgValuesTest()
		{
			var args = new[] { "--hide", "house", "hair", "head" };

			Parser parser = new PosixParser();

			try
			{
				CommandLine cmd = parser.Parse( m_opts, args );
				Assert.IsTrue( cmd.HasOption( "hide" ) );
				Assert.AreEqual( "house", cmd.GetOptionValue( "hide" ) );
				Assert.AreEqual( "house", cmd.GetOptionValues( "hide" )[ 0 ] );
				Assert.AreEqual( "hair", cmd.GetOptionValues( "hide" )[ 1 ] );
				Assert.AreEqual( cmd.Args.Length, 1 );
				Assert.AreEqual( "head", cmd.Args[ 0 ] );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}

		[Test]
		public void LongOptionalNoValueTest()
		{
			var args = new[] { "--fish" };
			try
			{
				Parser parser = new PosixParser();
				CommandLine cmd = parser.Parse( m_opts, args );
				Assert.IsTrue( cmd.HasOption( "fish" ) );
				Assert.IsNull( cmd.GetOptionValue( "fish" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}

		[Test]
		public void LongWithArgTest()
		{
			Assert.IsTrue( m_cl.HasOption( "d" ) );
			Assert.IsNotNull( m_cl.GetOptionValue( "d" ) );
			Assert.AreEqual( "bar", m_cl.GetOptionValue( "d" ) );
		}

		[Test]
		[Category( "NotWorking" )]
		[Ignore]
		public void PropertyOptionFlagsTest()
		{
			var properties = new Dictionary<string, string>
				{ { "a", "true" }, { "c", "yes" }, { "e", "1" } };

			Parser parser = new PosixParser();

			try
			{
				CommandLine cmd = parser.Parse( m_opts, null, properties );
				Assert.IsTrue( cmd.HasOption( "a" ) );
				Assert.IsTrue( cmd.HasOption( "c" ) );
				Assert.IsTrue( cmd.HasOption( "e" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}

			properties = new Dictionary<string, string>();
			properties.Add( "a", "false" );
			properties.Add( "c", "no" );
			properties.Add( "e", "0" );
			try
			{
				CommandLine cmd = parser.Parse( m_opts, null, properties );
				Assert.IsTrue( !cmd.HasOption( "a" ) );
				Assert.IsTrue( !cmd.HasOption( "c" ) );
				Assert.IsTrue( !cmd.HasOption( "e" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}

			properties = new Dictionary<string, string>();
			properties.Add( "a", "TRUE" );
			properties.Add( "c", "nO" );
			properties.Add( "e", "TrUe" );
			try
			{
				CommandLine cmd = parser.Parse( m_opts, null, properties );
				Assert.IsTrue( cmd.HasOption( "a" ) );
				Assert.IsTrue( !cmd.HasOption( "c" ) );
				Assert.IsTrue( cmd.HasOption( "e" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}

			properties = new Dictionary<string, string>();
			properties.Add( "a", "just a string" );
			properties.Add( "e", "" );
			try
			{
				CommandLine cmd = parser.Parse( m_opts, null, properties );
				Assert.IsTrue( !cmd.HasOption( "a" ) );
				Assert.IsTrue( !cmd.HasOption( "c" ) );
				Assert.IsTrue( !cmd.HasOption( "e" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}

		[Test]
		public void PropertyOptionMultipleValuesTest()
		{
			var properties = new Dictionary<string, string>();
			properties.Add( "k", "one,two" );

			Parser parser = new PosixParser();

			var values = new[] { "one", "two" };
			try
			{
				CommandLine cmd = parser.Parse( m_opts, null, properties );
				Assert.IsTrue( cmd.HasOption( "k" ) );
				Assert.IsTrue( CompareArrays( values, cmd.GetOptionValues( 'k' ) ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}

		[Test]
		public void PropertyOptionSingularValueTest()
		{
			var properties = new Dictionary<string, string> { { "hide", "seek" } };

			Parser parser = new PosixParser();

			try
			{
				CommandLine cmd = parser.Parse( m_opts, null, properties );
				Assert.IsTrue( cmd.HasOption( "hide" ) );
				Assert.AreEqual( "seek", cmd.GetOptionValue( "hide" ) );
				Assert.IsTrue( !cmd.HasOption( "fake" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}

		[Test]
		public void PropertyOverrideValuesTest()
		{
			var args = new[] { "-j", "found", "-i", "ink" };

			var properties = new Dictionary<string, string> { { "j", "seek" } };
			try
			{
				Parser parser = new PosixParser();
				CommandLine cmd = parser.Parse( m_opts, args, properties );
				Assert.IsTrue( cmd.HasOption( "j" ) );
				Assert.AreEqual( "found", cmd.GetOptionValue( "j" ) );
				Assert.IsTrue( cmd.HasOption( "i" ) );
				Assert.AreEqual( "ink", cmd.GetOptionValue( "i" ) );
				Assert.IsTrue( !cmd.HasOption( "fake" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}

		[Test]
		public void ShortNoArgTest()
		{
			Assert.IsTrue( m_cl.HasOption( "a" ) );
			Assert.IsNull( m_cl.GetOptionValue( "a" ) );
		}

		[Test]
		public void ShortOptionalArgNoValueTest()
		{
			var args = new[] { "-e" };
			try
			{
				Parser parser = new PosixParser();
				CommandLine cmd = parser.Parse( m_opts, args );
				Assert.IsTrue( cmd.HasOption( "e" ) );
				Assert.IsNull( cmd.GetOptionValue( "e" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}

		[Test]
		public void ShortOptionalArgValuesTest()
		{
			var args = new[] { "-j", "ink", "idea" };
			try
			{
				Parser parser = new PosixParser();
				CommandLine cmd = parser.Parse( m_opts, args );
				Assert.IsTrue( cmd.HasOption( "j" ) );
				Assert.AreEqual( "ink", cmd.GetOptionValue( "j" ) );
				Assert.AreEqual( "ink", cmd.GetOptionValues( "j" )[ 0 ] );
				Assert.AreEqual( "idea", cmd.GetOptionValues( "j" )[ 1 ] );
				Assert.AreEqual( cmd.Args.Length, 0 );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}

		[Test]
		public void ShortOptionalArgValueTest()
		{
			var args = new[] { "-e", "everything" };
			try
			{
				Parser parser = new PosixParser();
				CommandLine cmd = parser.Parse( m_opts, args );
				Assert.IsTrue( cmd.HasOption( "e" ) );
				Assert.AreEqual( "everything", cmd.GetOptionValue( "e" ) );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}


		[Test]
		public void ShortOptionalNArgValuesTest()
		{
			var args = new[] { "-i", "ink", "idea", "isotope", "ice" };
			try
			{
				Parser parser = new PosixParser();
				CommandLine cmd = parser.Parse( m_opts, args );
				Assert.IsTrue( cmd.HasOption( "i" ) );
				Assert.AreEqual( "ink", cmd.GetOptionValue( "i" ) );
				Assert.AreEqual( "ink", cmd.GetOptionValues( "i" )[ 0 ] );
				Assert.AreEqual( "idea", cmd.GetOptionValues( "i" )[ 1 ] );
				Assert.AreEqual( cmd.Args.Length, 2 );
				Assert.AreEqual( "isotope", cmd.Args[ 0 ] );
				Assert.AreEqual( "ice", cmd.Args[ 1 ] );
			}
			catch ( ParseException e )
			{
				Assert.Fail( "Cannot setUp() CommandLine: " + e.Message );
			}
		}

		[Test]
		public void ShortWithArgTest()
		{
			Assert.IsTrue( m_cl.HasOption( "b" ) );
			Assert.IsNotNull( m_cl.GetOptionValue( "b" ) );
			Assert.AreEqual( "foo", m_cl.GetOptionValue( "b" ) );
		}
	}
}