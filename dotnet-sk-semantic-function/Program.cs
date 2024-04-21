using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var builder = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(Env.Var("AOAI_MODEL_ID"), Env.Var("AOAI_ENDPOINT"), Env.Var("AOAI_API_KEY"));

var kernel = builder.Build();
var skPrompt = """
               担任高级{{$language}}开发人员。将此 JSON 文档转换为 {{$language}} 数据模型。

               --- 开始 ---
               {{$input}}
               --- 结束 ---
               """;
var openAiPromptExecutionSettings = new OpenAIPromptExecutionSettings
{
    MaxTokens = 8000,
    Temperature = 1
};
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
var function = kernel.CreateFunctionFromPrompt(skPrompt, openAiPromptExecutionSettings, "converter", "将JSON文档转换为数据模型");

//转换为C#数据模型
await kernel.InvokeAsync(function, new KernelArguments() { ["input"] = input, ["language"] = "c#" })
    .ContinueWith((convertResult) => { Console.WriteLine(convertResult.Result); });