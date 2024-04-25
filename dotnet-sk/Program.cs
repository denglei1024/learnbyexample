using dotnet_sk.Plugins.MathPlugin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning.Handlebars;

// create a new kernel builder
var builder = Kernel.CreateBuilder();
    
builder.Services.AddAzureOpenAIChatCompletion(Env.Var("AOAI_MODEL_ID"), Env.Var("AOAI_ENDPOINT"), Env.Var("AOAI_API_KEY"));
builder.Services.AddLogging(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));

// build the kernel
var kernel = builder.Build();

// import plugin
kernel.ImportPluginFromType<MathPlugin>();

var goal = "算一下我有多少钱。如果首先我的投资 2130.23 美元增加了 23%，然后我花 5 美元买了一杯咖啡";

// create a planner and plan the kernel
#pragma warning disable SKEXP0060 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
var planner = new HandlebarsPlanner(new HandlebarsPlannerOptions{ AllowLoops = true});
var plan = await planner.CreatePlanAsync(kernel, goal);
// create pre-defined key-value pairs for the kernel arguments
var keyValuePairs = new KernelArguments(new PromptExecutionSettings())
{
};
try
{
    var planResult = await plan.InvokeAsync(kernel, keyValuePairs);
    Console.WriteLine($"The goal: {goal}");
    Console.WriteLine($"The Plan Processed: {plan}");
    Console.WriteLine("Plan result: " + planResult);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

#pragma warning restore SKEXP0060 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
