﻿using ClinicalTrialsApi.WebApi.Configuration;

namespace ClinicalTrialsApi.WebApi.Middlewares
{
    public class FileUploadMiddleware
    {
        private readonly RequestDelegate _next;
        public FileUploadMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ConfigurationService configurationService)
        {
            var maxFileSize = configurationService.MaxFileSizeInMb * 1024 * 1024;

            if (context.Request.ContentType?.StartsWith("multipart/form-data") == true)
            {
                var form = await context.Request.ReadFormAsync();

                foreach (var file in form.Files)
                {
                    if (file.Length > maxFileSize)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync($"File size exceeds the limit of {configurationService.MaxFileSizeInMb} MB.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
