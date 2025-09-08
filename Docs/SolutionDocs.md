# Solution Overview

This document describes the current "aspire" orchestration setup and the projects in this solution. It summarizes roles, where to find key files, and how the orchestration is configured.

## High-level architecture

- vfml2025.AppHost
  - Acts as the orchestration host for local development.
  - Uses a DistributedApplication builder to register service projects so they can be run together for developer scenarios.
  - Holds the service discovery wiring and configuration such that services can find each other by name.
  - Key file: vfml2025.AppHost/AppHost.cs (calls AddProject for each service)

- Chatbot
  - Backend service project (current placeholder console app).
  - Intended to implement chatbot logic and expose APIs that the UI can call.
  - Key file: Chatbot/Program.cs

- BlazorUI
  - Blazor WebAssembly frontend that provides the chat UI.
  - Contains pages and components used by the running site.
  - Key files:
    - BlazorUI/Program.cs (WASM host)
    - BlazorUI/App.razor (router)
    - BlazorUI/Pages/Index.razor
    - BlazorUI/Pages/Chat.razor
    - BlazorUI/Components/ChatWindow.razor
    - BlazorUI/Components/ChatInput.razor

- vfml2025.ServiceDefaults
  - Shared helper extension methods and defaults used by service projects.
  - Adds service discovery, HTTP resilience, health checks, and OpenTelemetry by default.
  - Key file: vfml2025.ServiceDefaults/Extensions.cs

## Orchestration and "Aspire" defaults

The orchestration is driven by vfml2025.AppHost which creates a DistributedApplication builder and registers projects:

- AddProject<Projects.Chatbot>("chatbot") registers the backend service.
- AddProject<Projects.BlazorUI>("frontend") registers the Blazor WebAssembly frontend.

vfml2025.ServiceDefaults provides opinionated defaults for services:

- OpenTelemetry: configures logging, metrics, and tracing. It will enable OTLP exporter when the OTEL_EXPORTER_OTLP_ENDPOINT configuration is set.
- Health checks: registers a readiness endpoint (`/health`) and a liveness endpoint (`/alive`) in the development environment.
- Service discovery: registers service discovery services and configures HttpClient defaults to use service discovery and a resilience handler.

## Running the solution locally

- Start the orchestrator (AppHost) to run the registered projects together during development. From the vfml2025.AppHost project directory run:

  dotnet run

- Alternatively run an individual project:
  - BlazorUI: run from the BlazorUI directory (dotnet run) and open the hosted URL. The app includes a simple `/chat` page.
  - Chatbot: the backend is currently a console app placeholder; implement ASP.NET endpoints or a service host and run accordingly.

## Observability and configuration

- Not easily available for WASM projects like BlazorUI however when we move the backend project to be an API service it should be simple to wire up for that.


