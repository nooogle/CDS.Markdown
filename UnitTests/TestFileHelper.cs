using System;
using System.IO;
using System.Threading.Tasks;

namespace UnitTests
{
    /// <summary>
    /// Provides async file helpers compatible with both .NET 8 and .NET Framework 4.8.
    /// </summary>
    public static class TestFileHelper
    {
#if NET8_0_OR_GREATER
        public static Task WriteAllTextAsync(string path, string contents)
            => File.WriteAllTextAsync(path, contents);
#else
        public static Task WriteAllTextAsync(string path, string contents)
            => Task.Run(() => File.WriteAllText(path, contents));
#endif
    }
}
