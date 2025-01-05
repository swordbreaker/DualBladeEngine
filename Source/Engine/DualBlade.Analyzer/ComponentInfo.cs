using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace DualBlade.Analyzer;

public record struct ComponentInfo(
    string StructName,
    string Ns,
    IEnumerable<string> Usings,
    bool HasDefaultCtor,
    IEnumerable<FieldDeclarationSyntax> Fields,
    IEnumerable<MethodDeclarationSyntax> Methods);