using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;

var builder = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(Env.Var("AOAI_MODEL_ID"), Env.Var("AOAI_ENDPOINT"), Env.Var("AOAI_API_KEY"));

var kernel = builder.Build();
// 导入语义函数
kernel.ImportPluginFromPromptDirectory("Plugins/JsonConverter");

// 定义函数
var function = kernel.cre