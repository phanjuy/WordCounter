using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace WordCounter;

internal class WordHistogram(string inputFilePath, string outputFilePath)
{
    public Dictionary<string, int> GetWordHistogram(string pattern = @"\W+")
    {
        Dictionary<string, int> histogram = [];

        using StreamReader reader = File.OpenText(inputFilePath);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();

            if (!string.IsNullOrEmpty(line))
            {
                var words = Regex.Split(line.ToLowerInvariant(), pattern);

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
}