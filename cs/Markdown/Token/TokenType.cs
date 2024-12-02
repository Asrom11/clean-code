namespace Markdown;

public enum TokenType
{
    Text,
    Strong,
    Italic,
    Header,
    Link 
}

public enum TagState
{
    Open,
    Close
}