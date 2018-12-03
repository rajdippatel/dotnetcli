using System;
using System.Collections;
using System.IO;
using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	/// Summary description for PatternOptionBuilder
	/// </summary>
	[TestFixture]
	public class PatternOptionBuilderTest
	{
		[Test]
		[Category( "NotWorking" )]
		[Ignore]
		public void SimplePatternTest()
		{
			try
			{
				Options options = PatternOptionBuilder.ParsePattern( "a:b@cde>f+n%t/y#" );

				string[] args = new[]
					{
						"-c", "-a", "foo", "-b", "System.Collections.ArrayList", "-e",
						"build.xml", "-f", "System.DateTime", "-n", "4.5", "-t",
						"http://jakarta.apache.org/", "-y", "2000/01/01"
					};

				ICommandLineParser parser = new PosixParser();
				CommandLine line = parser.Parse( options, args );

				Assert.AreEqual( "foo", line.GetOptionValue( "a" ), "flag a" );
				Assert.AreEqual(
					0, ( line.GetOptionObject( "b" ) as ArrayList ).Count, "object flag b" );
				Assert.IsTrue( line.HasOption( "c" ), "boolean true flag c" );
				Assert.IsFalse( line.HasOption( "d" ), "boolean false flag d" );
				Assert.AreEqual(
					"build.xml",
					( line.GetOptionObject( "e" ) as FileInfo ).Name,
					"file flag e" );
				Assert.AreEqual(
					typeof ( DateTime ), line.GetOptionObject( "f" ), "class flag f" );
				Assert.AreEqual( 4.5, line.GetOptionObject( "n" ), "number flag n" );
				Assert.AreEqual(
					"http://jakarta.apache.org/",
					( line.GetOptionObject( "t" ) as Uri ).OriginalString,
					"url flag t" );
				Assert.AreEqual(
					DateTime.Parse( "2000/01/01" ).Ticks,
					Convert.ToDateTime( line.GetOptionObject( "y" ) ).Ticks,
					"date flag y" );

				// tests the char methods of CommandLine that delegate 
				// to the string methods
				Assert.AreEqual( "foo", line.GetOptionValue( 'a' ), "flag a" );
				Assert.AreEqual(
					0, ( line.GetOptionObject( 'b' ) as ArrayList ).Count, "object flag b" );
				Assert.IsTrue( line.HasOption( 'c' ), "boolean true flag c" );
				Assert.IsFalse( line.HasOption( 'd' ), "boolean false flag d" );
				Assert.AreEqual(
					"build.xml",
					( line.GetOptionObject( 'e' ) as FileInfo ).Name,
					"file flag e" );
				Assert.AreEqual(
					typeof ( DateTime ), line.GetOptionObject( 'f' ), "class flag f" );
				Assert.AreEqual( 4.5, line.GetOptionObject( 'n' ), "number flag n" );
				Assert.AreEqual(
					"http://jakarta.apache.org/",
					( line.GetOptionObject( 't' ) as Uri ).OriginalString,
					"url flag t" );
				Assert.AreEqual(
					DateTime.Parse( "2000/01/01" ).Ticks,
					Convert.ToDateTime( line.GetOptionObject( 'y' ) ).Ticks,
					"date flag y" );
			}
			catch ( ParseException e )
			{
				Assert.Fail( e.Message );
			}
			catch ( UriFormatException e )
			{
				Assert.Fail( e.Message );
			}
		}
	}
}