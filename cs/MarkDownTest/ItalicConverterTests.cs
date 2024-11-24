using FluentAssertions;
using Markdown;
using Markdown.interfaces;
using NUnit.Framework;

namespace MarkDownTest;

public class ItalicConverterTests
{
    private ItalicTagConverter _converter;

    [SetUp]
    public void Setup()
    {
        _converter = new ItalicTagConverter();
    }

    [Test]
    public void ConvertToHtml_SimpleItalic_ReturnsTextWithEmTags()
    {
        var tokens = new List<Token> 
        { 
            new Token { Text = "_italic_", Type = TokenType.Italic } 
        };

        var result = _converter.ConvertToHtml(tokens.ToList());
        
        result.Single().Text.Should().Be("<em>italic</em>", 
            "Italic text should be wrapped in em tags");
    }

    [Test]
    public void ConvertToHtml_NoItalic_ReturnsUnmodifiedTokens()
    {
        var tokens = new List<Token> 
        { 
            new Token { Text = "Regular text", Type = TokenType.Strong } 
        };
        
        var result = _converter.ConvertToHtml(tokens.ToList());
        
        result.Should().BeEquivalentTo(tokens, 
            "Non-italic tokens should remain unchanged");
    }
}