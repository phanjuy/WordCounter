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
        using StreamReader reader = File.OpenText(InputFilePath);
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    List<string> words = Split(line);
                    CountWords(words);
                }
            }
        }

        using (StreamWriter writer = new StreamWriter(OutputFilePath))
        {
            foreach (var entry in _histogram.ToImmutableSortedDictionary())
            {
                writer.WriteLine($"{entry.Key}: {entry.Value}");
            }
        }
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
}