﻿global using Carter;
global using Mapster;
global using MediatR;
global using FluentValidation;
global using System.Reflection;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.Extensions.DependencyInjection;
global using Measurements.Domain.Models;
global using Measurements.Application.DTOs;
global using Measurements.Application.Mappings;
global using Measurements.Infrastructure.Database;
global using Measurements.Application.Measurements.GetMeasurement;
global using Measurements.Application.Measurements.CreateMeasurement;
global using Measurements.Application.Measurements.DeleteMeasurement;
global using CommonServiceLibrary.Behaviors;
global using CommonServiceLibrary.Exceptions.Handlers;