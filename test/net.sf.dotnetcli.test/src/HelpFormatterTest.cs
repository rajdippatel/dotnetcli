using System.IO;
using System.Text;
using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	///This is a test class for HelpFormatterTest and is intended
	///to contain all HelpFormatterTest Unit Tests
	///</summary>
	[TestFixture]
	public class HelpFormatterTest
	{
		[Test]
		public void AutomaticUsageTest()
		{
			HelpFormatter hf = new HelpFormatter();
			Options options = null;
			string expected = "usage: app [-a]";
			MemoryStream ms = new MemoryStream();
			StreamWriter pw = new StreamWriter( ms );

			options = new Options().AddOption( "a", false, "aaaa aaaa aaaa aaaa aaaa" );
			hf.PrintUsage( pw, 60, "app", options );
			pw.Flush();
			Assert.AreEqual(
				expected,
				Encoding.ASCII.GetString( ms.ToArray() ).Trim(),
				"simple auto usage" );
			ms.SetLength( 0 );

			expected = "usage: app [-a] [-b]";
			options = new Options().AddOption( "a", false, "aaaa aaaa aaaa aaaa aaaa" );
			options.AddOption( "b", false, "bbb" );
			hf.PrintUsage( pw, 60, "app", options );
			pw.Flush();
			Assert.AreEqual(
				expected,
				Encoding.ASCII.GetString( ms.ToArray() ).Trim(),
				"simple auto usage" );
			ms.SetLength( 0 );
		}

		/// <summary>
		///A test for FindWrapPos
		///</summary>
		[Test]
		public void FindWrapPosTest()
		{
			HelpFormatter hf = new HelpFormatter();
			string text = "This is a test.";

			Assert.AreEqual( 7, hf.FindWrapPos( text, 8, 0 ), "wrap position" );

			Assert.AreEqual( -1, hf.FindWrapPos( text, 8, 8 ), "wrap position 2" );

			text = "aaaa aa";
			Assert.AreEqual( 4, hf.FindWrapPos( text, 3, 0 ), "wrap position 3" );
		}

		/// <summary>
		///A test for PrintOptions
		///</summary>
		[Test]
		public void PrintOptionsTest()
		{
			HelpFormatter hf = new HelpFormatter();
			StringBuilder sb = new StringBuilder();

			int leftPad = 1;
			int descPad = 3;

			string lpad = hf.CreatePadding( leftPad );
			string dpad = hf.CreatePadding( descPad );

			Options options = null;
			string expected = null;

			options = new Options().AddOption( "a", false, "aaaa aaaa aaaa aaaa aaaa" );
			expected = lpad + "-a" + dpad + "aaaa aaaa aaaa aaaa aaaa";
			hf.RenderOptions( sb, 60, options, leftPad, descPad );
			Assert.AreEqual( expected, sb.ToString(), "simple non-wrapped option" );

			int nextLineTabStop = leftPad + descPad + "-a".Length;
			expected = lpad + "-a" + dpad + "aaaa aaaa aaaa" + hf.NewLine +
			           hf.CreatePadding( nextLineTabStop ) + "aaaa aaaa";
			sb.Length = 0;
			hf.RenderOptions( sb, nextLineTabStop + 17, options, leftPad, descPad );
			Assert.AreEqual( expected, sb.ToString(), "simple wrapped option" );

			options = new Options().AddOption( "a", "aaa", false, "dddd dddd dddd dddd" );
			expected = lpad + "-a,--aaa" + dpad + "dddd dddd dddd dddd";
			sb.Length = 0;
			hf.RenderOptions( sb, 60, options, leftPad, descPad );
			Assert.AreEqual( expected, sb.ToString(), "long non-wrapped option" );

			nextLineTabStop = leftPad + descPad + "-a,--aaa".Length;
			expected = lpad + "-a,--aaa" + dpad + "dddd dddd" + hf.NewLine +
			           hf.CreatePadding( nextLineTabStop ) + "dddd dddd";
			sb.Length = 0;
			hf.RenderOptions( sb, 25, options, leftPad, descPad );
			Assert.AreEqual( expected, sb.ToString(), "long wrapped option" );

			options =
				new Options().AddOption( "a", "aaa", false, "dddd dddd dddd dddd" ).
					AddOption( "b", false, "feeee eeee eeee eeee" );
			expected = lpad + "-a,--aaa" + dpad + "dddd dddd" + hf.NewLine +
			           hf.CreatePadding( nextLineTabStop ) + "dddd dddd" + hf.NewLine +
			           lpad + "-b      " + dpad + "feeee eeee" + hf.NewLine +
			           hf.CreatePadding( nextLineTabStop ) + "eeee eeee";
			sb.Length = 0;
			hf.RenderOptions( sb, 25, options, leftPad, descPad );
			Assert.AreEqual( expected, sb.ToString(), "multiple wrapped options" );
		}

		/// <summary>
		///A test for PrintUsage
		///</summary>
		[Test]
		public void PrintUsageTest()
		{
			Option optionA = new Option( "a", "first" );
			Option optionB = new Option( "b", "second" );
			Option optionC = new Option( "c", "third" );
			Options opts = new Options();
			opts.AddOption( optionA );
			opts.AddOption( optionB );
			opts.AddOption( optionC );
			HelpFormatter hf = new HelpFormatter();
			MemoryStream ms = new MemoryStream();
			StreamWriter printWriter = new StreamWriter( ms );
			hf.PrintUsage( printWriter, 80, "app", opts );
			printWriter.Close();
			string actual = Encoding.ASCII.GetString( ms.ToArray() );
			Assert.AreEqual( "usage: app [-a] [-b] [-c]" + hf.NewLine, actual );
		}

		/// <summary>
		///A test for PrintWrapped
		///</summary>
		[Test]
		public void PrintWrappedTest()
		{
			HelpFormatter hf = new HelpFormatter();
			StringBuilder sb = new StringBuilder();

			string text = "This is a test.";
			string expected;

			expected = "This is a" + hf.NewLine + "test.";
			hf.RenderWrappedText( sb, 12, 0, text );
			Assert.AreEqual( expected, sb.ToString(), "single line text" );

			sb.Length = 0;
			expected = "This is a" + hf.NewLine + "    test.";
			hf.RenderWrappedText( sb, 12, 4, text );
			Assert.AreEqual( expected, sb.ToString(), "single line padded text" );

			text = "aaaa aaaa aaaa" + hf.NewLine + "aaaaaa" + hf.NewLine + "aaaaa";

			expected = text;
			sb.Length = 0;
			hf.RenderWrappedText( sb, 16, 0, text );
			Assert.AreEqual( expected, sb.ToString(), "multi line text" );

			expected = "aaaa aaaa aaaa" + hf.NewLine + "    aaaaaa" + hf.NewLine +
			           "    aaaaa";
			sb.Length = 0;
			hf.RenderWrappedText( sb, 16, 4, text );
			Assert.AreEqual( expected, sb.ToString(), "multi-line padded text" );
		}
	}
}