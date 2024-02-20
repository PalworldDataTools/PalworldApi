﻿namespace PalworldApi.Rest.OpenApi;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
class OpenApiVersionAttribute : Attribute
{
    public OpenApiVersionAttribute(string version)
    {
        Version = version;
    }

    public string Version { get; init; }
}
