using System.Collections.Generic;
using System.CommandLine;

public static class ParseResultExtensions
{
    public static SymbolResult GetCommandObject(this ParseResult result, string alias)
    {
        return result.RootCommandResult.Children.GetByAlias(alias);
    }
}