using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace WordCounter;

internal class WordHistogram(string inputFilePath, string outputFilePath)
{
    private readonly Dictionary<string, int> _histogram = [];

    public void Process()
    {
        GetWordHistogram();
        WriteHistogramToFile();
    }

    private void GetWordHistogram()
    {
        StreamReader reader = File.OpenText(inputFilePath);
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

    private void WriteHistogramToFile()
    {
        StreamWriter writer = File.CreateText(outputFilePath);
        foreach (var entry in _histogram.ToImmutableSortedDictionary())
        {
            writer.WriteLine($"{entry.Key}: {entry.Value}");
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