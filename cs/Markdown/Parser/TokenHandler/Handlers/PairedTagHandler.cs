namespace Markdown.Parser.TokenHandler.Handlers;

public class PairedTagHandler : BaseTokenHandler
{
    public PairedTagHandler(Delimiter delimiter) : base(delimiter)
    {
    }

    public override bool TryHandle(ParsingContext context, out Token token, out int skip)
    {
        token = null;
        skip = 0;

        if (!IsMatch(context.Text, context.Position, Delimiter.Opening))
            return false;

        var isClosing = false;
        var tempStack = new Stack<TokenType>();
        
        while (context.OpenTags.Count > 0)
        {
            var openTag = context.OpenTags.Pop();
            tempStack.Push(openTag);

            if (openTag != Delimiter.Type)
            {
                continue;
            }
            
            isClosing = true;
            tempStack.Pop();
            break;
        }
        
        while (tempStack.Count > 0)
        {
            context.OpenTags.Push(tempStack.Pop());
        }

        if (!isClosing)
        {
            context.OpenTags.Push(Delimiter.Type);
        }

        token = new Token(
            Delimiter.Opening,
            Delimiter.Type,
            isClosing ? TagState.Close : TagState.Open,
            context.Position);
        skip = Delimiter.Opening.Length;
        return true;
    }
}
