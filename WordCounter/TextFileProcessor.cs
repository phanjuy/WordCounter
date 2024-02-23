namespace WordCounter;

internal class TextFileProcessor(string inputFilePath, string outputFilePath)
{
    public string InputFilePath { get; } = inputFilePath;
    public string OutputFilePath { get; } = outputFilePath;

    public void Process()
    {
        string text = File.ReadAllText(InputFilePath);

        string processedText = text.ToUpperInvariant();

        File.WriteAllText(OutputFilePath, processedText);
    }
}
