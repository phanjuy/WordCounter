using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace WordCounter;

internal class TextFileProcessor(string inputFilePath, string outputFilePath)
{
    public string InputFilePath { get; } = inputFilePath;
    public string OutputFilePath { get; } = outputFilePath;

    private readonly Dictionary<string, int> _histogram = [];

    public void Process()
    {
        IEnumerable<string> lines = File.ReadLines(InputFilePath);

        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                List<string> words = Split(line);
                CountWords(words);
            }
        }
        var result = Format(_histogram);

        File.WriteAllLines(OutputFilePath, result);
    }

    private static List<string> Split(string text)
    {
        var pattern = @"\W+";
        return Regex.Split(text.ToLowerInvariant(), pattern).ToList();
    }

    private Dictionary<string, int> CountWords(IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            if (!_histogram.TryGetValue(word, out int value))
            {
                _histogram[word] = 0;
            }
            _histogram[word] = ++value;
        }
        return _histogram;
    }

    private static List<string> Format(Dictionary<string, int> dictionary)
    {
        return dictionary
            .ToImmutableSortedDictionary()
            .Select(entry => $"{entry.Key}{":"}{entry.Value}")
            .ToList();
    }
}