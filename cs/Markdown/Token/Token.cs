namespace Markdown;

public class Token
{
    public string Text { get; set; }
    public TokenType Type { get; set; }
    public int Position { get; set; }
}
