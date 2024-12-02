using Markdown.Parser.TokenHandler.Handlers;
using static Markdown.Parser.TokenHandler.Delimiter;

namespace Markdown.Parser.TokenHandler;

public static class TokenHandlerFactory
{
    private static readonly Dictionary<TokenType, Delimiter> Delimiters = new()
    {
        { TokenType.Strong, new Delimiter("__", "__", TokenType.Strong) },
        { TokenType.Italic, new Delimiter("_", "_", TokenType.Italic) }
    };

    public static IList<ITokenHandler> CreateHandlers()
    {
        return new List<ITokenHandler>
        {
            new LinkHandler(),
            new PairedTagHandler(Delimiters[TokenType.Strong]),
            new PairedTagHandler(Delimiters[TokenType.Italic]),
            new HeaderHandler()
        };
    }
}