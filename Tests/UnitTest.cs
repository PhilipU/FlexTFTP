
using FlexTFTP;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class UnitTest
    {
        [Test]
        public void FlexSystemPaths()
        {
            string path = TargetPathParser.GetPathByName("3-01050A01_application.s19");
            NUnit.Framework.Assert.That(path != null, Is.True, "FlexSystem-M Path Should be found");
        }
    }
}