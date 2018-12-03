using System;
using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	/// Summary description for OptionBuilderTest
	/// </summary>
	[TestFixture]
	public class OptionBuilderTest
	{
		[Test]
		public void BaseOptionCharOptTest()
		{
			Option option =
				OptionBuilder.Factory.WithDescription( "option description" ).Create( 'o' );

			Assert.AreEqual( "o", option.Opt );
			Assert.AreEqual( "option description", option.Description );
			Assert.IsTrue( !option.HasArg );
		}

		[Test]
		public void BaseOptionStringOptTest()
		{
			Option option =
				OptionBuilder.Factory.WithDescription( "option description" ).Create( "o" );

			Assert.AreEqual( "o", option.Opt );
			Assert.AreEqual( "option description", option.Description );
			Assert.IsTrue( !option.HasArg );
		}

		[Test]
		public void CompleteOptionTest()
		{
			Option simple =
				OptionBuilder.Factory.WithLongOpt( "simple option" ).HasArg().IsRequired().
					HasArgs().WithType( typeof ( float ) ).WithDescription(
					"this is a simple option" ).Create( "s" );

			Assert.AreEqual( "s", simple.Opt );
			Assert.AreEqual( "simple option", simple.LongOpt );
			Assert.AreEqual( "this is a simple option", simple.Description );
			Assert.AreEqual( typeof ( float ), simple.Type );
			Assert.IsTrue( simple.HasArg );
			Assert.IsTrue( simple.IsRequired );
			Assert.IsTrue( simple.HasArgs );
		}

		[Test]
		public void IllegalOptionsTest()
		{
			// bad single character option
			try
			{
				Option opt =
					OptionBuilder.Factory.WithDescription( "option description" ).Create( '"' );
				Assert.Fail( "ArgumentException not caught" );
			}
			catch ( ArgumentException )
			{
				// success
			}

			// bad character in option string
			try
			{
				Option opt = OptionBuilder.Factory.Create( "opt`" );
				Assert.Fail( "ArgumentException not caught" );
			}
			catch ( ArgumentException )
			{
				// success
			}

			// valid option 
			try
			{
				Option opt = OptionBuilder.Factory.Create( "opt" );
				// success
			}
			catch ( ArgumentException )
			{
				Assert.Fail( "ArgumentException caught" );
			}
		}

		[Test]
		public void OptionArgNumbersTest()
		{
			Option opt =
				OptionBuilder.Factory.WithDescription( "option description" ).HasArgs( 2 ).
					Create( 'o' );
			Assert.AreEqual( 2, opt.NumberOfArgs );
		}

		[Test]
		public void SpecialOptCharsTest()
		{
			// '?'
			try
			{
				Option opt =
					OptionBuilder.Factory.WithDescription( "help options" ).Create( '?' );
				Assert.AreEqual( "?", opt.Opt );
			}
			catch ( ArgumentException )
			{
				Assert.Fail( "ArgumentException caught" );
			}

			// '@'
			try
			{
				Option opt =
					OptionBuilder.Factory.WithDescription( "help options" ).Create( '@' );
				Assert.AreEqual( "@", opt.Opt );
			}
			catch ( ArgumentException )
			{
				Assert.Fail( "ArgumentException caught" );
			}
		}

		[Test]
		public void TwoCompleteOptionsTest()
		{
			Option simple =
				OptionBuilder.Factory.WithLongOpt( "simple option" ).HasArg().IsRequired().
					HasArgs().WithType( typeof ( float ) ).WithDescription(
					"this is a simple option" ).Create( "s" );

			Assert.AreEqual( "s", simple.Opt );
			Assert.AreEqual( "simple option", simple.LongOpt );
			Assert.AreEqual( "this is a simple option", simple.Description );
			Assert.AreEqual( typeof ( float ), simple.Type );
			Assert.IsTrue( simple.HasArg );
			Assert.IsTrue( simple.IsRequired );
			Assert.IsTrue( simple.HasArgs );

			simple =
				OptionBuilder.Factory.WithLongOpt( "dimple option" ).HasArg().
					WithDescription( "this is a dimple option" ).Create( 'd' );

			Assert.AreEqual( "d", simple.Opt );
			Assert.AreEqual( "dimple option", simple.LongOpt );
			Assert.AreEqual( "this is a dimple option", simple.Description );
			Assert.IsNull( simple.Type );
			Assert.IsFalse( simple.IsRequired );
			Assert.IsFalse( simple.HasArgs );
		}
	}
}