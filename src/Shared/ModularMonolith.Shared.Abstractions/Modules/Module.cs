﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolith.Shared.Abstractions.Modules;

public abstract class Module
{
    public string Name => GetType().Name.Replace("Module", string.Empty);
    public abstract string Path { get; }

    public abstract void AddModule(IServiceCollection services, IConfiguration configuration);
    public abstract void UseModule(WebApplication app);
}