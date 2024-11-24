namespace Markdown.interfaces;

public interface IHtmlTagConverter
{
    IList<Token> ConvertToHtml(IList<Token> tokens);
}