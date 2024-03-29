﻿using WordCounter;

Directory.SetCurrentDirectory(@"D:\Code\GitHub\WordCounter");
string currentDirectory = Environment.CurrentDirectory;

string inputFilePath = Path.Combine(currentDirectory, "inputFile.txt");
string outputFilePath = Path.Combine(currentDirectory, "outputFile.txt");

var processor = new WordHistogram(inputFilePath, outputFilePath);
var histogram = processor.GetWordHistogram();
processor.WriteToFile(histogram);