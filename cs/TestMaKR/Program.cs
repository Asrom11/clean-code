using Markdown;

var md = new Md();
var result = md.Render("__bold _italic__ text__");
Console.WriteLine(result);