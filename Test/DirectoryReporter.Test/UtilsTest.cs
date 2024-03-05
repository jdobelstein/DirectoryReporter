using Xunit;

namespace DirectoryReporter.Test
{
    public class UtilsTest
    {
        [Fact]
        public void GetShortPathName()
        {
            var result = LongFile.GetShortPathName("C:\\Program Files (x86)\\TestDir");
            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}