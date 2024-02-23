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

        var histogram = GetWordHistogram(words);

        var result = Format(histogram);

        File.WriteAllLines(OutputFilePath, result);
    }

    private static List<string> Split(string text)
    {
        var pattern = @"\W+";
        return Regex.Split(text.ToLowerInvariant(), pattern).ToList();
    }

    private static Dictionary<string, int> GetWordHistogram(IEnumerable<string> words)
    {
        return words.GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());
    }

    private static List<string> Format(Dictionary<string, int> dictionary)
    {
        return dictionary
            .Select(entry => $"{entry.Key}{":"}{entry.Value}")
            .ToList();
    }
}