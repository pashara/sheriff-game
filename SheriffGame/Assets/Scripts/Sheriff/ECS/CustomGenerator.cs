using System.IO;
using System.Text.RegularExpressions;

namespace Sheriff.ECS
{
    public class CustomGenerator
    {
        static void Main()
        {
            // Считываем весь текст из файла класса CardComponentsLookup
            string lookupClassContent = File.ReadAllText("path_to_CardComponentsLookup.cs");

            // Регулярное выражение для поиска названий и ID компонентов
            string pattern = @"public const int (\w+) = (\d+);";

            foreach (Match match in Regex.Matches(lookupClassContent, pattern))
            {
                string componentName = match.Groups[1].Value;
                string componentId = match.Groups[2].Value;

                // Создаем содержимое файла
                string fileContent = GenerateFileContent(componentName, componentId);

                // Записываем содержимое в файл
                File.WriteAllText($"{componentName}Extension.cs", fileContent);
            }
        }
        
        static string GenerateFileContent(string componentName, string componentId)
        {
            return
                $@"using Sheriff.ECS;
using UniRx;

public partial class CardEntity
{{
    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.{componentName}Component> On{componentName}() =>
        this.OnChange<Sheriff.ECS.Components.{componentName}Component>({componentId});
}}";
        }
    }
}