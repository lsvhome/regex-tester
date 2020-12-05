namespace RegexTesterBlazorClientSide
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public static class Compiler
    {
        private static Task initializationTask;

        private static List<MetadataReference> references;

        public static void InitializeMetadataReferences(HttpClient client)
        {
            async Task InitializeInternal()
            {
                var response = await client.GetJsonAsync<BlazorBoot>("_framework/blazor.boot.json");

                string[] assemblyNames = response.resources.assembly.Keys.ToArray().Where(x => x.EndsWith(".dll")).ToArray();

                var assemblies = await Task.WhenAll(assemblyNames.Select(x => client.GetAsync("_framework/" + x)));

                var references = new List<MetadataReference>(assemblyNames.Length);

                foreach (var asm in assemblies)
                {
                    using (var task = await asm.Content.ReadAsStreamAsync())
                    {
                        references.Add(MetadataReference.CreateFromStream(task));
                    }
                }

                Compiler.references = references;
            }

            initializationTask = InitializeInternal();
        }

        public static void WhenReady(Func<Task> action)
        {
            if (initializationTask == null)
            {
                Debug.Fail("Error #845739648: Pattern");
                return;
            }

            if (initializationTask.Status != TaskStatus.RanToCompletion)
            {
                initializationTask.ContinueWith(x => action());
            }
            else
            {
                action();
            }
        }

        public static (bool success, Assembly asm) LoadSource(string source)
        {
            var compilation = CSharpCompilation.Create("DynamicCode")
                .WithOptions(new CSharpCompilationOptions(OutputKind.ConsoleApplication))
                .AddReferences(references)
                .AddSyntaxTrees(CSharpSyntaxTree.ParseText(source));

            ImmutableArray<Diagnostic> diagnostics = compilation.GetDiagnostics();

            bool error = false;
            foreach (Diagnostic diag in diagnostics)
            {
                switch (diag.Severity)
                {
                    case DiagnosticSeverity.Info:
                        Debug.WriteLine(diag.ToString());
                        break;
                    case DiagnosticSeverity.Warning:
                        Debug.WriteLine(diag.ToString());
                        break;
                    case DiagnosticSeverity.Error:
                        error = true;
                        Debug.WriteLine(diag.ToString());
                        break;
                }
            }

            if (error)
            {
                return (false, null);
            }

            using (var outputAssembly = new MemoryStream())
            {
                compilation.Emit(outputAssembly);

                return (true, Assembly.Load(outputAssembly.ToArray()));
            }
        }

        public static string Format(string source)
        {
            var tree = CSharpSyntaxTree.ParseText(source);
            var root = tree.GetRoot();
            var normalized = root.NormalizeWhitespace();
            return normalized.ToString();
        }

        internal class BlazorBoot
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Auto generated name")]
            public string main { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Auto generated name")]
            public string entryPoint { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Auto generated name")]
            public string[] assemblyReferences { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Auto generated name")]
            public string[] cssReferences { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Auto generated name")]
            public string[] jsReferences { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Auto generated name")]
            public bool linkerEnabled { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Auto generated name")]
            public Resources resources { get; set; }

            internal class Resources
            {
                [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Auto generated name")]
                public Dictionary<string, string> assembly { get; set; }
            }
        }
    }
}
