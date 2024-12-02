namespace Markdown.Parser.TokenHandler;

public interface ITokenHandler
{
    bool TryHandle(ParsingContext context, out Token token, out int skip);
}