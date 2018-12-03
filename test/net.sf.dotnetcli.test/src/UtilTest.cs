using NUnit.Framework;

namespace net.sf.dotnetcli
{
	/// <summary>
	/// Summary description for UtilTest
	/// </summary>
	[TestFixture]
	public class UtilTest
	{
		[Test]
		public void StripLeadingHyphensTest()
		{
			Assert.AreEqual( "f", Util.StripLeadingHyphens( "-f" ) );
			Assert.AreEqual( "foo", Util.StripLeadingHyphens( "--foo" ) );
			Assert.IsNull( Util.StripLeadingHyphens( null ) );
		}
	}
}