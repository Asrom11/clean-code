using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkDownTest;

public class MarkdownConverterTests
{
    private MarkdownConverter _converter;

    [SetUp]
    public void Setup()
    {
        _converter = new MarkdownConverter();
    }

    [Test]
    public void Convert_ComplexMarkdown_ReturnsCorrectlyFormattedHtml()
    {
        var tokens = new List<Token>
        {
            new() { Text = "# ", Type = TokenType.Header },
            new() { Text = "Header with ", Type = TokenType.Header },
            new() { Text = "_italic_", Type = TokenType.Italic },
            new() { Text = " and ", Type = TokenType.Header },
            new() { Text = "__bold__", Type = TokenType.Strong }
        };
        
        var result = _converter.Convert(tokens);
        
        result.Should().Be("<h1>Header with <em>italic</em> and <strong>bold</strong></h1>",
            "Complex markdown should be converted with proper nesting");
    }

    [Test]
    public void Convert_EmptyTokenList_ReturnsEmptyString()
    {
        var tokens = new List<Token>();
        
        var result = _converter.Convert(tokens);
        
        result.Should().BeEmpty("Empty token list should result in empty string");
    }
}