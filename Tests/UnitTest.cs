
using FlexTFTP;

namespace Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void FlexSystemPaths()
        {
            string path = TargetPathParser.GetPathByName("3-01050A01_application.s19");
            Assert.IsNotNull(path);
        }
    }
}