using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning.Handlebars;

// create a new kernel builder
var builder = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(Env.Var("AOAI_MODEL_ID"), Env.Var("AOAI_ENDPOINT"), Env.Var("AOAI_API_KEY"));
builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));

// build the kernel
var kernel = builder.Build();

// import plugins from the prompt directory
kernel.ImportPluginFromPromptDirectory("plugins");

// create a planner and plan the kernel

#pragma warning disable SKEXP0060 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
var planner = new HandlebarsPlanner(new HandlebarsPlannerOptions{ AllowLoops = true});
var plan = await planner.CreatePlanAsync(kernel, "");
// create pre-defined key-value pairs for the kernel arguments
KernelArguments keyValuePairs = new KernelArguments()
{
    {}
};
var planResult = await plan.InvokeAsync(kernel, keyValuePairs);
#pragma warning restore SKEXP0060 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.



