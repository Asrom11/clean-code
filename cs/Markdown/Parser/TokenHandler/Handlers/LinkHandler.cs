namespace Markdown.Parser.TokenHandler.Handlers;

public class LinkHandler : BaseTokenHandler
{
    public LinkHandler() : base(new Delimiter("[", "]", TokenType.Link))
    {
    }

    public override bool TryHandle(ParsingContext context, out Token token, out int skip)
    {
        token = null;
        skip = 0;
        var position = context.Position;
        var text = context.Text;

        if (text[position] != '[')
            return false;

        var closingBracketIndex = text.IndexOf(']', position);
        if (closingBracketIndex == -1 || closingBracketIndex + 1 >= text.Length || text[closingBracketIndex + 1] != '(')
            return false;

        var closingParenIndex = text.IndexOf(')', closingBracketIndex + 1);
        if (closingParenIndex == -1)
            return false;

        var linkText = text.Substring(position + 1, closingBracketIndex - position - 1);
        var url = text.Substring(closingBracketIndex + 2, closingParenIndex - closingBracketIndex - 2);

        token = new Token(linkText, TokenType.Link, TagState.Open, position, url: url);
        skip = closingParenIndex - position + 1;

        return true;
    }
}