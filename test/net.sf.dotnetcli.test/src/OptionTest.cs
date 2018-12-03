using System;
using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	///This is a test class for OptionTest and is intended
	///to contain all OptionTest Unit Tests
	///</summary>
	[TestFixture]
	public class OptionTest
	{
		[Serializable]
		private class DefaultOption : Option
		{
			private readonly string defaultValue;

			public DefaultOption( string opt, string description, string defaultValue )
				: base( opt, true, description )
			{
				this.defaultValue = defaultValue;
			}

			public new string GetValue()
			{
				return base.GetValue() != null ? base.GetValue() : defaultValue;
			}
		}

		[Test]
		public void ClearTest()
		{
			Option option = new Option( "x", true, "" );
			Assert.AreEqual( 0, option.ValuesList.Count );
			option.AddValueForProcessing( "a" );
			Assert.AreEqual( 1, option.ValuesList.Count );
			option.ClearValues();
			Assert.AreEqual( 0, option.ValuesList.Count );
		}

		[Test]
		public void CloneTest()
		{
			Option a = new Option( "a", true, "" );
			Option b = ( Option ) a.Clone();
			Assert.AreEqual( a, b );
			Assert.AreNotSame( a, b );
			a.Description = "a";
			Assert.AreEqual( "", b.Description );
			b.NumberOfArgs = 2;
			b.AddValueForProcessing( "b1" );
			b.AddValueForProcessing( "b2" );
			Assert.AreEqual( 1, a.NumberOfArgs );
			Assert.AreEqual( 0, a.ValuesList.Count );
			Assert.AreEqual( 2, b.Values.Length );
		}

		[Test]
		public void SubclassTest()
		{
			Option option = new DefaultOption( "f", "file", "myfile.txt" );
			Option clone = ( Option ) option.Clone();
			Assert.AreEqual( "myfile.txt", ( ( DefaultOption ) clone ).GetValue() );
			Assert.AreEqual( typeof ( DefaultOption ), clone.GetType() );
		}
	}
}