namespace ClinicalTrialsApi.WebApi.Middlewares
{
    public class FileUploadMiddleware
    {
        private readonly RequestDelegate _next;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public FileUploadMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.ContentType?.StartsWith("multipart/form-data") == true)
            {
                var form = await context.Request.ReadFormAsync();

                foreach (var file in form.Files)
                {
                    if (file.Length > MaxFileSize)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("File size exceeds the limit of 5 MB.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
