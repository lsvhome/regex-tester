﻿namespace RegexTesterBlazorClientSide
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
                try
                {

                    Debug.WriteLine("Info #867587709809-01: InitializeInternal -");
                    var response = await client.GetJsonAsync<BlazorBoot>("_framework/blazor.boot.json");
                    Debug.WriteLine($"Info #867587709809-02: InitializeInternal {response?.assemblies?.Length}");
                    var l = new List<string>();
                    Debug.WriteLine("Info #867587709809-01: InitializeInternal 02-01");
                    //l.Add("System.Core.dll");

                    if (response?.assemblies != null)
                        l.AddRange(response.assemblies?.Where(x => x.EndsWith(".dll")));
                    //var f = response.assemblyReferences?.Where(x => x.EndsWith(".dll")).ToList();

                    Debug.WriteLine("Info #867587709809-01: InitializeInternal 02-02");

                    var f1 = l.Select(x => client.GetAsync("_framework/_bin/" + x))?.ToArray() ?? new Task<HttpResponseMessage>[0];

                    Debug.WriteLine("Info #867587709809-01: InitializeInternal 02-03");

                    var assemblies = await Task.WhenAll(f1);


                    Debug.WriteLine($"Info #867587709809-03: InitializeInternal {assemblies.Length}");
                    var references = new List<MetadataReference>(assemblies.Length);
                    foreach (var asm in assemblies)
                    {
                        Debug.WriteLine($"Info #867587709809-04: InitializeInternal");
                        using (var task = await asm.Content.ReadAsStreamAsync())
                        {
                            references.Add(MetadataReference.CreateFromStream(task));
                        }
                    }

                    Debug.WriteLine("Info #867587709809-05: InitializeInternal");
                    Compiler.references = references;
                    Debug.WriteLine("Info #867587709809-06: InitializeInternal");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error #867587709809-00001: InitializeInternal {ex.ToString()}");

                }
            }

            initializationTask = InitializeInternal();
        }

        public static void WhenReady(Func<Task> action)
        {
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
                        Console.WriteLine(diag.ToString());
                        break;
                    case DiagnosticSeverity.Warning:
                        Console.WriteLine(diag.ToString());
                        break;
                    case DiagnosticSeverity.Error:
                        error = true;
                        Console.WriteLine(diag.ToString());
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
            public string[] assemblies { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Auto generated name")]
            public string[] cssReferences { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Auto generated name")]
            public string[] jsReferences { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Auto generated name")]
            public bool linkerEnabled { get; set; }
        }
    }
}
