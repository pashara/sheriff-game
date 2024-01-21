using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class ReactivePropertyGenerator
{
    private static string _namespace;

    /*
     * D:\sheriff-game\SheriffGame\Assets\Scripts\Sheriff\ECS\Generated\
     * D:\sheriff-game\SheriffGame\Assets\Scripts\Sheriff\ECS\ReactiveGenerated\
     * "Sheriff.ECS"
     */
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        if (args.Length < 3)
        {
            Console.WriteLine("Not correct arguments");
            return;
        }

        var startDirectory = args[0];
        var exportDirectory = args[1];
        _namespace = args[2];

        if (!Directory.Exists(startDirectory))
        {
            Console.WriteLine($"Dir not exist: {startDirectory}");
            return;
        }

        ProcessDirectory(startDirectory, exportDirectory);
    }

    static void ProcessDirectory(string directoryPath, string exportDirectory)
    {
        string[] fileEntries = Directory.GetFiles(directoryPath, "*ComponentsLookup.cs", SearchOption.AllDirectories);
        foreach (string fileName in fileEntries)
        {
            ProcessFile(fileName, exportDirectory);
        }
    }

    static void ProcessFile(string filePath, string exportDirectory)
    {
        string lookupClassContent = File.ReadAllText(filePath);
        string entityName = Path.GetFileNameWithoutExtension(filePath);
        entityName = string.Join("", entityName.Take(entityName.Length - "ComponentsLookup".Length));

        string patternTypes = @"typeof\(([\w\.]+)\)";
        var componentFullNames = Regex.Matches(lookupClassContent, patternTypes);

        string patternNames = @"public const int (\w+) = (\d+);";
        var componentNameMatches = Regex.Matches(lookupClassContent, patternNames);


        List<(string fullComponentName, string componentName, string componentId)> elements = new();
        
        
        for (int i = 0; i < Math.Min(componentNameMatches.Count, componentFullNames.Count); i++)
        {
            string componentName = componentNameMatches[i].Groups[1].Value;
            string componentId = componentNameMatches[i].Groups[2].Value;
            string fullComponentName = componentFullNames[i].Groups[1].Value;
            
            elements.Add((fullComponentName, componentName, componentId));
        }
        
        string fileContent = GenerateFileContent(entityName, elements);

        Directory.CreateDirectory(exportDirectory);
        string outputFile = Path.Combine(exportDirectory, $"{entityName}_ReactiveExtension.cs");
        File.WriteAllText(outputFile, fileContent);
        
    }

    static string GenerateFileContent(
        string entityName, 
        List<(string fullComponentName, string componentName, string componentId)> elements)
    {
        return
            $@"using {_namespace};
using UniRx;

public partial class {entityName}Entity
{{"+string.Join("\n", elements.Select(x => $@"
    public IReadOnlyReactiveProperty<{x.fullComponentName}> On{x.componentName}() =>
        this.OnChange<{x.fullComponentName}>({x.componentId});")) + 
$"\n}}";
    }
}