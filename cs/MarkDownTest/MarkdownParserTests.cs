using FluentAssertions;
using Markdown;
using Markdown.Parser;
using Markdown.Parser.Interface;
using NUnit.Framework;

namespace MarkDownTest;

public class MarkdownParserTests
{
    private IMarkdownParser _parser;

    [SetUp]
    public void Setup()
    {
        _parser = new MarkdownParser();
    }

    [Test]
    public void Parse_PlainText_ReturnsTextToken()
    {
        var input = "Simple text";
        var expectedTokens = new[]
        {
            Token.CreateText("Simple text", 0)
        };
        
        var tokens = _parser.Parse(input);
        
        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }

    [Test]
    public void Parse_TextWithSymbols_ReturnsTextToken()
    {
        var input = "Text with symbols !@#$%^&*()";
        var expectedTokens = new[]
        {
            Token.CreateText("Text with symbols !@#$%^&*()", 0)
        };
        
        var tokens = _parser.Parse(input);
        
        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }

    [TestCase("# Header", 1)]
    [TestCase("## Header", 2)]
    [TestCase("###### Header", 6)]
    public void Parse_Header_ReturnsHeaderAndTextTokens(string input, int expectedLevel)
    {
        var expectedTokens = new[]
        {
            Token.CreateHeader(expectedLevel, 0),
            Token.CreateText("Header", expectedLevel + 1)
        };
        
        var tokens = _parser.Parse(input);

        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }

    [TestCase("#Header")]
    [TestCase("####### Header")]
    [TestCase("Text# Header")]
    public void Parse_InvalidHeader_ReturnsSingleTextToken(string input)
    {
        var expectedTokens = new[]
        {
            Token.CreateText(input, 0)
        };
        
        var tokens = _parser.Parse(input);
        
        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }

    [Test]
    public void Parse_StrongEmphasis_ReturnsCorrectTokens()
    {
        var input = "__bold__";
        var expectedTokens = new[]
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("bold", 2),
            Token.CreateStrong(false, 6)
        };
        
        var tokens = _parser.Parse(input);
        
        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }

    [Test]
    public void Parse_ItalicEmphasis_ReturnsCorrectTokens()
    {
        var input = "_italic_";
        var expectedTokens = new[]
        {
            Token.CreateItalic(true, 0),
            Token.CreateText("italic", 1),
            Token.CreateItalic(false, 7)
        };
        
        var tokens = _parser.Parse(input);
        
        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }

    [Test]
    public void Parse_NestedEmphasis_ReturnsCorrectTokens()
    {
        var input = "__bold _italic_ text__";
        var expectedTokens = new[]
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("bold ", 2),
            Token.CreateItalic(true, 7),
            Token.CreateText("italic", 8),
            Token.CreateItalic(false, 14),
            Token.CreateText(" text", 15),
            Token.CreateStrong(false, 20)
        };
        
        var tokens = _parser.Parse(input);
        
        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }

    [Test]
    public void Parse_NestedTags_ClosesInCorrectOrder()
    {
        var input = "__bold _italic__ text_";
        var expectedTokens = new[]
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("bold ", 2),
            Token.CreateItalic(true, 7), 
            Token.CreateText("italic", 8), 
            Token.CreateStrong(false, 14), 
            Token.CreateText(" text", 16),
            Token.CreateItalic(false, 21) 
        };
    
        var tokens = _parser.Parse(input);
    
        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }
    
    [Test]
    public void Parse_ComplexMarkdown_ReturnsCorrectTokenSequence()
    {
        var input = "# Header\n__bold _italic_ text__";
        var expectedTokens = new[]
        {
            Token.CreateHeader(1, 0),
            Token.CreateText("Header\n", 2),
            Token.CreateStrong(true, 9),
            Token.CreateText("bold ", 11),
            Token.CreateItalic(true, 16),
            Token.CreateText("italic", 17),
            Token.CreateItalic(false, 23),
            Token.CreateText(" text", 24),
            Token.CreateStrong(false, 29)
        };
        
        var tokens = _parser.Parse(input);
        
        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }
    
    [Test]
    public void Parse_NestedTags_HandlesNestedStrongAndItalic()
    {
        var input = "__bold _italic_ bold__";
        var expectedTokens = new[]
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("bold ", 2),    
            Token.CreateItalic(true, 7),      
            Token.CreateText("italic", 8),      
            Token.CreateItalic(false, 14),     
            Token.CreateText(" bold", 15),    
            Token.CreateStrong(false, 20),   
        };

        var tokens = _parser.Parse(input);

        tokens.Should().BeEquivalentTo(expectedTokens,
            options => options.WithStrictOrdering());
    }
    
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("\n")]
    public void Parse_MinimalInput_ReturnsTextToken(string input)
    {
        var expectedTokens = input.Length == 0 
            ? Array.Empty<Token>() 
            : new[] { Token.CreateText(input, 0) };
        
        var tokens = _parser.Parse(input);
        
        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }

    [Test]
    public void Parse_MixedContent_PreservesWhitespaceInTextTokens()
    {
        var input = "Text  __with  spaces__  here";
        var expectedTokens = new[]
        {
            Token.CreateText("Text  ", 0),
            Token.CreateStrong(true, 6),
            Token.CreateText("with  spaces", 8),
            Token.CreateStrong(false, 20),
            Token.CreateText("  here", 22)
        };
        
        var tokens = _parser.Parse(input);
        
        tokens.Should().BeEquivalentTo(expectedTokens, 
            options => options.WithStrictOrdering());
    }
    
    [Test]
    public void Parse_Link_ReturnsLinkToken()
    {
        var input = "[пример ссылки](https://example.com)";
        var expectedTokens = new[]
        {
            Token.CreateLink("пример ссылки", "https://example.com", 0)
        };
    
        var tokens = _parser.Parse(input);
    
        tokens.Should().BeEquivalentTo(expectedTokens, options => options.WithStrictOrdering());
    }
    
    
    [Test]
    public void Parse_StrongTextWithLink_ReturnsCorrectTokens()
    {
        var input = "__Посетите [сайт](https://example.com)__";
        var expectedTokens = new[]
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("Посетите ", 2),
            Token.CreateLink("сайт", "https://example.com", 11),
            Token.CreateStrong(false, 38)
        };
    
        var tokens = _parser.Parse(input);
        
        tokens.Should().BeEquivalentTo(expectedTokens, options => options.WithStrictOrdering());
    }
}