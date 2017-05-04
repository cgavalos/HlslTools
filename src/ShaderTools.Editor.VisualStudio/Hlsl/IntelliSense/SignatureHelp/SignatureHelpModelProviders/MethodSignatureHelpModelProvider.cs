using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Linq;
using ShaderTools.CodeAnalysis.Hlsl.Compilation;
using ShaderTools.CodeAnalysis.Hlsl.Symbols;
using ShaderTools.CodeAnalysis.Hlsl.Syntax;
using ShaderTools.CodeAnalysis.Text;

namespace ShaderTools.Editor.VisualStudio.Hlsl.IntelliSense.SignatureHelp.SignatureHelpModelProviders
{
    [Export(typeof(ISignatureHelpModelProvider))]
    internal sealed class MethodSignatureHelpModelProvider : SignatureHelpModelProvider<MethodInvocationExpressionSyntax>
    {
        protected override SignatureHelpModel GetModel(SemanticModel semanticModel, MethodInvocationExpressionSyntax node, SourceLocation position)
        {
            var targetType = semanticModel.GetExpressionType(node.Target);
            var name = node.Name;
            var signatures = targetType
                .LookupMembers<FunctionSymbol>(name.Text)
                .OrderBy(f => f.Parameters.Length)
                .ToSignatureItems()
                .ToImmutableArray();

            if (signatures.Length == 0)
                return null;

            var span = node.GetTextSpanRoot();
            if (span == null)
                return null;

            var parameterIndex = node.ArgumentList.GetParameterIndex(position);

            return new SignatureHelpModel(span.Value.Span, signatures,
                GetSelected(semanticModel.GetSymbol(node), signatures, parameterIndex),
                parameterIndex);
        }
    }
}