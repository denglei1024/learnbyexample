using dotnet_sk_plugins.Plugins.Plugin.Time;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var builder = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(Env.Var("AOAI_MODEL_ID"), Env.Var("AOAI_ENDPOINT"), Env.Var("AOAI_API_KEY"));

builder.Plugins.AddFromType<TimePlugin>();

//1、手动调用
var kernel = builder.Build();
var answer = await kernel.InvokeAsync<string>(
    "TimePlugin",
    "GetToday"
);

Console.WriteLine("今天是: " + answer);

//2、AI自动调用
ChatHistory history = [];
IChatCompletionService chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
Console.Write("你: ");
string? userInput;
while ((userInput = Console.ReadLine())!="exit")
{
    history.AddUserMessage(userInput);

    var executionSettings = new OpenAIPromptExecutionSettings
    {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    };
    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(
        history,
        executionSettings,
        kernel
    );
    var first = true;
    var fullMessage = "";
    await foreach (var content in result)
    {
        if (content.Role.HasValue && first)
        {
            Console.WriteLine("ChartGPT: " + content.Content);
            first = false;
        }

        fullMessage += content.Content;
    }
    Console.WriteLine();
    history.AddAssistantMessage(fullMessage);
    Console.Write("你: ");
}