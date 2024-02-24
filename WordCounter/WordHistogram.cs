using System.Collections.Immutable;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace WordCounter;

internal class WordHistogram(string inputFilePath, string outputFilePath, IFileSystem fileSystem)
{
    public WordHistogram(string inputFilePath, string outputFilePath)
        : this(inputFilePath, outputFilePath, new FileSystem()) { }

    public Dictionary<string, int> GetWordHistogram(string pattern = @"\W+")
    {
        Dictionary<string, int> histogram = [];

        using StreamReader reader = fileSystem.File.OpenText(inputFilePath);

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
        using StreamWriter writer = fileSystem.File.CreateText(outputFilePath);

        foreach (var entry in histogram.ToImmutableSortedDictionary())
        {
            writer.WriteLine($"{entry.Key}: {entry.Value}");
        }
    }
}