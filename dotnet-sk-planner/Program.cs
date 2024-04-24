using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning.Handlebars;

var builder = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(Env.Var("AOAI_MODEL_ID"), Env.Var("AOAI_ENDPOINT"), Env.Var("AOAI_API_KEY"));
builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));

var kernel = builder.Build();

var planner = new HandlebarsPlanner();
kernel.ImportPluginFromPromptDirectory("plugins");