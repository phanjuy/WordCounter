using System.Text.RegularExpressions;

namespace WordCounter;

internal class TextFileProcessor(string inputFilePath, string outputFilePath)
{
    public string InputFilePath { get; } = inputFilePath;
    public string OutputFilePath { get; } = outputFilePath;

    public void Process()
    {
        string text = File.ReadAllText(InputFilePath);

        var words = Split(text);

        string processedText = text.ToUpperInvariant();

        File.WriteAllText(OutputFilePath, processedText);
    }

    private static List<string> Split(string text)
    {
        var pattern = @"\W+";
        return Regex.Split(text.ToLowerInvariant(), pattern).ToList();
    }
}
