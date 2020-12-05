using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace RegexUtilsTests
{
    public class BlazorBootTest : XUnitOutputTest
    {
        public BlazorBootTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void BlazorBootDeserializationTest()
        {
            string filename = "blazor.boot.json";
            Assert.True(File.Exists(filename));
            string stringContent = File.ReadAllText(filename);

            var blazorBoot = JsonSerializer.Deserialize<RegexTesterBlazorClientSide.Compiler.BlazorBoot>(stringContent);

            var assemblies = blazorBoot?.resources?.assembly;

            Assert.NotNull(assemblies);

            Assert.True(assemblies.Keys.Any());
        }
    }
}
