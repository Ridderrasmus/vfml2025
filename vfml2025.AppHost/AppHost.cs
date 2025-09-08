var builder = DistributedApplication.CreateBuilder(args);


// Backend API project
var backend = builder.AddProject<Projects.Chatbot>("chatbot");

// Blazor WebAssembly frontend project
var frontend = builder.AddProject<Projects.BlazorUI>("frontend");


builder.Build().Run();
