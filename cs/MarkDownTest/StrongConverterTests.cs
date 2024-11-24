using FluentAssertions;
using Markdown;
using Markdown.interfaces;
using NUnit.Framework;

namespace MarkDownTest;

public class StrongConverterTests
{
    private StrongTagConverter _converter;

    [SetUp]
    public void Setup()
    {
        _converter = new StrongTagConverter();
    }

    [Test]
    public void ConvertToHtml_SimpleStrong_ReturnsTextWithStrongTags()
    {
        var tokens = new List<Token> 
        { 
            new Token { Text = "__strong__", Type = TokenType.Strong } 
        };
        
        var result = _converter.ConvertToHtml(tokens.ToList());
        
        result.Single().Text.Should().Be("<strong>strong</strong>", 
            "Strong text should be wrapped in strong tags");
    }

    [Test]
    public void ConvertToHtml_NoStrong_ReturnsUnmodifiedTokens()
    {
        var tokens = new List<Token> 
        { 
            new Token { Text = "Regular text", Type = TokenType.Header } 
        };
        
        var result = _converter.ConvertToHtml(tokens.ToList());
        
        result.Should().BeEquivalentTo(tokens, 
            "Non-strong tokens should remain unchanged");
    }
}