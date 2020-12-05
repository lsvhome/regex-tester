using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;


namespace RegexUtilsTests
{
    public abstract class XUnitOutputTest : IDisposable
    {
        private readonly TextWriterTraceListener _localTraceListener;

        protected XUnitOutputTest(ITestOutputHelper output)
        {
            Output = output;
            var converter = new Converter(output);
            _localTraceListener = new TextWriterTraceListener(converter);
            Trace.Listeners.Add(_localTraceListener);
            output.WriteLine($"Test begins at {DateTime.Now}");
        }

        public virtual void Dispose()
        {
            Trace.WriteLine($"Test ends at {DateTime.Now}");
            Trace.Listeners.Remove(_localTraceListener);
        }


        private class Converter : TextWriter
        {
            readonly ITestOutputHelper _output;
            public Converter(ITestOutputHelper output)
            {
                _output = output;
            }
            public override Encoding Encoding => Encoding.UTF8;

            public override void WriteLine(string message)
            {
                try
                {
                    _output.WriteLine(message);
                }
                catch (Exception)
                {
                    Debugger.Break();
                }
            }
            public override void WriteLine(string format, params object[] args)
            {
                try
                {
                    _output.WriteLine(format, args);
                }
                catch (Exception)
                {
                    Debugger.Break();
                }
            }
        }

        protected ITestOutputHelper Output { get; }

        internal static Stream GetResourceStream(string fileName)
        {
            Console.WriteLine(fileName);
            // Read file content from disk
            string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(path1))
            {
                return File.OpenRead(path1);
            }

            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, fileName, SearchOption.AllDirectories);
            if (files.Count() == 1)
            {
                if (File.Exists(files.Single()))
                {
                    return File.OpenRead(files.Single());
                }
            }

            // Read embedded file content
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            //string name = $"{assembly.GetName().Name}.{fileName}";
            var allNames = assembly.GetManifestResourceNames();
            string path = allNames.SingleOrDefault(x => x.EndsWith(fileName));
#if DEBUG
            if (string.IsNullOrWhiteSpace(path))
            {
                var sourcesProjectFolder = Directory.GetCurrentDirectory();
                sourcesProjectFolder = sourcesProjectFolder + @"\..\..\..\..\";
                path = Directory.GetFiles(sourcesProjectFolder, fileName, SearchOption.AllDirectories).SingleOrDefault(x => x.EndsWith(fileName));
                Assert.NotNull(path);
                var retStream = new FileStream(path, FileMode.Open);
                Assert.NotNull(retStream);
                return retStream;
            }
#endif
            Assert.NotNull(path);
            var ret = assembly.GetManifestResourceStream(path);
            Assert.NotNull(ret);
            return ret;
        }

        internal static async Task<string> GetResourceString(string fileName)
        {
            using (var xmlStream = GetResourceStream(fileName))
            {
                Assert.NotNull(xmlStream);
                using (var xsdReader = new StreamReader(xmlStream))
                {
                    Assert.NotNull(xsdReader);
                    return await Task.FromResult(await xsdReader.ReadToEndAsync());
                }
            }
        }
    }
}
