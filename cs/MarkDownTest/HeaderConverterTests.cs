using FluentAssertions;
using Markdown;
using Markdown.interfaces;
using NUnit.Framework;

namespace MarkDownTest;

public class HeaderConverterTests
{
    private HeaderTagConverter _converter;

    [SetUp]
    public void Setup()
    {
        _converter = new HeaderTagConverter();
    }

    [Test]
    public void ConvertToHtml_SimpleHeader_ReturnsHeaderWithH1Tags()
    {
        var tokens = new List<Token> 
        { 
            new() { Text = "# Header", Type = TokenType.Header } 
        };
        
        var result = _converter.ConvertToHtml(tokens.ToList());
        
        result.Single().Text.Should().Be("<h1>Header</h1>", 
            "Header should be wrapped in h1 tags");
    }

    [Test]
    public void ConvertToHtml_NoHeader_ReturnsUnmodifiedTokens()
    {
        var tokens = new List<Token> 
        { 
            new() { Text = "Regular text", Type = TokenType.Italic } 
        };
        
        var result = _converter.ConvertToHtml(tokens.ToList());
        
        result.Should().BeEquivalentTo(tokens, 
            "Non-header tokens should remain unchanged");
    }
}