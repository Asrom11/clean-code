using FluentAssertions;
using Markdown;
using Markdown.Parser;
using NUnit.Framework;

namespace MarkDownTest;

public class MarkdownParserTests 
{
    private MarkdownParser _parser;

    [SetUp]
    public void Setup()
    {
        _parser = new MarkdownParser();
    }

    [Test]
    public void Parse_EmptyString_ReturnsEmptyTokenList()
    {
        const string input = "";

        var tokens = _parser.Parse(input).ToList();
        
        tokens.Should().BeEmpty("Empty input should result in empty token list");
    }

    [TestCase("# Header", TokenType.Header, "Header")]
    [TestCase("_italic_", TokenType.Italic, "italic")]
    [TestCase("__strong__", TokenType.Strong, "strong")]
    public void Parse_SingleElement_ReturnsCorrectToken(string input, TokenType expectedType, string expectedText)
    {
        var tokens = _parser.Parse(input).ToList();
            
        tokens.Should().ContainSingle()
            .Which.Should().Match<Token>(t => 
                t.Type == expectedType &&
                t.Text == expectedText);
    }

    [Test]
    public void Parse_ComplexMarkdown_ReturnsCorrectTokenSequence()
    {
        const string input = "# Header with __strong__ and _italic_";
        var expectedTokens = new[]
        { 
            new Token { Type = TokenType.Header, Text = "Header with " }, 
            new Token { Type = TokenType.Strong, Text = "strong" },
            new Token { Type = TokenType.Header, Text = " and " },
            new Token { Type = TokenType.Italic, Text = "italic" }
        };
            
        var tokens = _parser.Parse(input).ToList();
            
        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }

    [Test]
    public void Parse_UnderscoresInMiddleOfWord_NotConsideredAsMarkup()
    {
        const string input = "some_word_with_underscores";

        var tokens = _parser.Parse(input).ToList();
            
        tokens.Should().ContainSingle()
            .Which.Text.Should().Be("some_word_with_underscores", 
                "Underscores in middle of word should not be treated as markup");
    }

    [Test]
    public void Parse_EscapedUnderscores_NotConsideredAsMarkup()
    {
        const string input = "Text with \\_escaped\\_ underscores";
            
        var tokens = _parser.Parse(input).ToList();
            
        tokens.Should().ContainSingle()
            .Which.Text.Should().Be("Text with _escaped_ underscores", 
                "Escaped underscores should not be treated as markup");
    }

    [Test]
    public void Parse_HeaderWithoutSpace_NotConsideredAsHeader()
    {
        const string input = "#Not a header";
            
        var tokens = _parser.Parse(input).ToList();
            
        tokens.Should().ContainSingle()
            .Which.Text.Should().Be("#Not a header", 
                "# without space should not be treated as header");
    }

    [Test]
    public void Parse_MultipleLines_HandledCorrectly()
    {
        const string input = "# Header\n_italic_\n__strong__";
        var expectedTokens = new Token[]
        { 
            new() { Type = TokenType.Header, Text = "Header" },
            new() { Type = TokenType.Italic, Text = "italic" },
            new() { Type = TokenType.Strong, Text = "strong" }
        };
            
        var tokens = _parser.Parse(input).ToList();
            
        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }
}