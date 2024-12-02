namespace Markdown.Parser.TokenHandler.Handlers;

public class HeaderHandler : BaseTokenHandler
{
    public HeaderHandler() : base(new Delimiter("#", "", TokenType.Header))
    {
    }

    public override bool TryHandle(ParsingContext context, out Token token, out int skip)
    {
        token = null;
        skip = 0;

        if (!context.IsStartOfLine || !IsMatch(context.Text, context.Position, Delimiter.Opening))
            return false;

        var level = 1;
        var pos = context.Position + 1;

        while (pos < context.Text.Length && context.Text[pos] == '#' && level < 6)
        {
            level++;
            pos++;
        }

        if (pos >= context.Text.Length || context.Text[pos] != ' ')
            return false;
        
        context.OpenTags.Push(Delimiter.Type);

        token = new Token(
            new string('#', level),
            TokenType.Header,
            TagState.Open,
            context.Position,
            level);

        skip = level + 1;
        return true;
    }
}