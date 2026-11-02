using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Compiler Extension", Scope = "module")]

[assembly: SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1041:Compiler extensions should be implemented in assemblies targeting netstandard2.0", Justification = "Nullable Annotations", Scope = "module")]
[assembly: SuppressMessage("MicrosoftCodeAnalysisReleaseTracking", "RS2008:Enable analyzer release tracking", Justification = "Demo", Scope = "module")]
