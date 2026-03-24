// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;
using AccountManager.API.Extensions;
using AccountManager.API.Middleware;
using AccountManager.Domain;
using AccountManager.Infrastructure;
using AccountManager.Infrastructure.Logging;
using FastEndpoints;
using FluentValidation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Logging
// --------------------
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
    loggerConfiguration.ConfigureSerilog(context.Configuration, services));

// --------------------
// Services
// --------------------
builder.Services.AddFastEndpoints()
    .ConfigureHttpJsonOptions(o =>
    {
        o.SerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDomainLayer(builder.Configuration);

// --------------------
// Build
// --------------------
var app = builder.Build();

app.UseGlobalExceptionHandler();

// --------------------
// Middleware
// --------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseFastEndpoints();

app.Run();
