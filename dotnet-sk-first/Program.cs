using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var builder = Kernel.CreateBuilder();

var kernel = builder.AddAzureOpenAIChatCompletion(Env.Var("AOAI_MODEL_ID"), Env.Var("AOAI_ENDPOINT"),Env.Var("AOAI_API_KEY"))
.Build();

var history = new ChatHistory();

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

var executionSettings = new OpenAIPromptExecutionSettings{
 Temperature= 0.5,
 MaxTokens = 8000
};

Console.Write("你: ");
string? userInput;
while((userInput=Console.ReadLine())!=null){
    history.AddUserMessage(userInput);
    var response = await chatCompletionService.GetChatMessageContentAsync(history, executionSettings, kernel);
    Console.WriteLine("AI: "+ response);
    history.AddMessage(response.Role, response.Content??string.Empty);
    Console.Write("你：");
}