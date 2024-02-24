using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace WordCounter;

internal class WordHistogram(string inputFilePath, string outputFilePath)
{
    public Dictionary<string, int> GetWordHistogram()
    {
        Dictionary<string, int> histogram = [];

        using StreamReader reader = File.OpenText(inputFilePath);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();

            if (!string.IsNullOrEmpty(line))
            {
                List<string> words = Split(line);

                foreach (var word in words)
                {
                    if (!histogram.TryGetValue(word, out int count))
                    {
                        histogram[word] = 0;
                    }
                    histogram[word] = ++count;
                }
            }
        }
        return histogram;
    }

    public void WriteToFile(Dictionary<string, int> histogram)
    {
        using StreamWriter writer = File.CreateText(outputFilePath);

        foreach (var entry in histogram.ToImmutableSortedDictionary())
        {
            writer.WriteLine($"{entry.Key}: {entry.Value}");
        }
    }

    private static List<string> Split(string text)
    {
        var pattern = @"\W+";
        return Regex.Split(text.ToLowerInvariant(), pattern).ToList();
    }
}