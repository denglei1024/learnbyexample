using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;

var builder = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(Env.Var("AOAI_MODEL_ID"), Env.Var("AOAI_ENDPOINT"), Env.Var("AOAI_API_KEY"));

var kernel = builder.Build();

var prompt = kernel.ImportPluginFromPromptDirectory("Plugins/ConverterPlugin");
var input = """
            {
              "person": {
                "name": "Alice",
                "age": 28,
                "city": "New York",
                "occupation": "Software Engineer"
              },
              "pets": [
                {
                  "name": "Fluffy",
                  "species": "Cat",
                  "age": 5
                },
                {
                  "name": "Max",
                  "species": "Dog",
                  "age": 3
                }
              ],
              "favorite_foods": ["Sushi", "Pizza", "Chocolate"]
            }
            """;
var result = await kernel.InvokeAsync<string>(prompt["Json2Model"],new KernelArguments() { ["input"] = input, ["language"]="java" });
Console.WriteLine(result);
