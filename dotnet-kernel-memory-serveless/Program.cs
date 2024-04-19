using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.Memory;

IConfiguration configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var apiKey = configuration["OPENAI_KEY"];
if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("请设置环境变量 OPENAI_KEY");
    return;
}



var embeddingConfig = new AzureOpenAIConfig()
{
    APIKey = env["API_KEY"],
    Deployment = env["EMBEDDING_NAME"],
    Endpoint = env["ENDPOINT"],
    APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
    Auth = AzureOpenAIConfig.AuthTypes.APIKey
};
var textConfig = new AzureOpenAIConfig()
{
    APIKey = env["API_KEY"],
    Deployment = env["TEXT_NAME"],
    Endpoint = env["ENDPOINT"],
    APIType = AzureOpenAIConfig.APITypes.ChatCompletion,
    Auth = AzureOpenAIConfig.AuthTypes.APIKey
};
var kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(env["TEXT_NAME"], env["ENDPOINT"], env["API_KEY"])
    .Build();

var memory = new KernelMemoryBuilder()
    .WithAzureOpenAITextEmbeddingGeneration(embeddingConfig)
    .WithAzureOpenAITextGeneration(textConfig)
    .WithSimpleVectorDb()
    .Build<MemoryServerless>();

await memory.ImportWebPageAsync("https://raw.githubusercontent.com/microsoft/kernel-memory/main/README.md");
await memory.ImportWebPageAsync("https://juejin.cn/post/7323408577709080610");
Console.WriteLine("文档已经准备好，开始提问吧！");
while (true)
{
    var userInput = Console.ReadLine();
    var answer = await memory.AskAsync(userInput);
    Console.WriteLine(answer.Result);
    Console.WriteLine("参考:");
    foreach (var source in answer.RelevantSources)
    {
        Console.WriteLine($" - {source.SourceName}, {source.Link}[{source.Partitions.First()}{source.Partitions.First().LastUpdate:D}]");
    }
}

Console.WriteLine($"Hello, {configuration["OPENAI_KEY"]}!");