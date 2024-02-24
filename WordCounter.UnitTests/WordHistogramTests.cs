using FluentAssertions;
using System.IO.Abstractions.TestingHelpers;

namespace WordCounter.UnitTests;

public class WordHistogramTests

{
    [Fact]
    public void GetWordHistogram_ShouldReturnExpectedHistogram_GivenFixedInputFile()
    {
        // Arrange
        var inputPath = @"c:\root\in\test.txt";
        var outputPath = @"c:\root\out\test.txt";

        var inputFile = new MockFileData(
            "Do the things, you do so well\n" +
            "Do the things, you do so well");
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(inputPath, inputFile);

        var expected = new Dictionary<string, int>()
        {
            ["do"] = 4,
            ["the"] = 2,
            ["things"] = 2,
            ["you"] = 2,
            ["so"] = 2,
            ["well"] = 2,
        };

        var sut = new WordHistogram(inputPath, outputPath, fileSystem);

        // Act
        var actual = sut.GetWordHistogram();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void WriteToFile_ShouldCreateNewFile_GivenValidInput()
    {
        // Arrange
        var inputPath = @"c:\root\in\test.txt";
        var outputPath = @"c:\root\out\test.txt";
        var fileSystem = new MockFileSystem();
        fileSystem.AddDirectory(@"c:\root\out\");

        var histogram = new Dictionary<string, int>()
        {
            ["do"] = 4,
            ["the"] = 2,
            ["things"] = 2,
            ["you"] = 2,
            ["so"] = 2,
            ["well"] = 2,
        };

        var expected = "do: 4\r\nso: 2\r\nthe: 2\r\nthings: 2\r\nwell: 2\r\nyou: 2\r\n";

        var sut = new WordHistogram(inputPath, outputPath, fileSystem);

        // Act
        sut.WriteToFile(histogram);

        // Assert
        fileSystem.FileExists(outputPath).Should().BeTrue();

        MockFileData outputFile = fileSystem.GetFile(outputPath);
        outputFile.TextContents.Should().BeEquivalentTo(expected);
    }

}
