using System.Collections.Generic;
using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	/// Summary description for OptionsTest
	/// </summary>
	[TestFixture]
	public class OptionsTest
	{
		[Test]
		public void HelpOptionsTest()
		{
			Option longOnly1 = OptionBuilder.Factory.WithLongOpt( "long-only1" ).Create();

			Option longOnly2 = OptionBuilder.Factory.WithLongOpt( "long-only2" ).Create();

			Option shortOnly1 = OptionBuilder.Factory.Create( "1" );

			Option shortOnly2 = OptionBuilder.Factory.Create( "2" );

			Option bothA = OptionBuilder.Factory.WithLongOpt( "bothA" ).Create( "a" );

			Option bothB = OptionBuilder.Factory.WithLongOpt( "bothB" ).Create( "b" );

			Options options = new Options();
			options.AddOption( longOnly1 );
			options.AddOption( longOnly2 );
			options.AddOption( shortOnly1 );
			options.AddOption( shortOnly2 );
			options.AddOption( bothA );
			options.AddOption( bothB );

			List<Option> allOptions = new List<Option>();
			allOptions.Add( longOnly1 );
			allOptions.Add( longOnly2 );
			allOptions.Add( shortOnly1 );
			allOptions.Add( shortOnly2 );
			allOptions.Add( bothA );
			allOptions.Add( bothB );

			List<Option> helpOptions = new List<Option>( options.HelpOptionsReadOnly );

			Assert.IsTrue(
				helpOptions.Count == allOptions.Count, "Both lists are of the same length" );

			helpOptions.Sort( new HelpFormatter.OptionComparator() );
			allOptions.Sort( new HelpFormatter.OptionComparator() );

			for ( int x = 0; x < helpOptions.Count; ++x )
			{
				Assert.IsTrue( helpOptions[ x ].Equals( allOptions[ x ] ) );
			}
		}

		[Test]
		public void MissingOptionExceptionTest()
		{
			Options options = new Options();
			options.AddOption( OptionBuilder.Factory.IsRequired().Create( "f" ) );
			try
			{
				new PosixParser().Parse( options, new string[0] );
				Assert.Fail( "Expected MissingOptionException to be thrown" );
			}
			catch ( MissingOptionException e )
			{
				Assert.AreEqual( "Missing required option: f", e.Message );
			}
		}

		[Test]
		public void MissingOptionsExceptionTest()
		{
			Options options = new Options();
			options.AddOption( OptionBuilder.Factory.IsRequired().Create( "f" ) );
			try
			{
				new PosixParser().Parse( options, new string[0] );
				Assert.Fail( "Expected MissingOptionException to be thrown" );
			}
			catch ( MissingOptionException e )
			{
				Assert.AreEqual( "Missing required option: f", e.Message );
			}
		}
	}
}