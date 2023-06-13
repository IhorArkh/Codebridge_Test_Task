namespace DogsAPI
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RequestRateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _limit;
        private readonly Queue<DateTime> _requestTimes;

        public RequestRateLimitMiddleware(RequestDelegate next, int limit)
        {
            _next = next;
            _limit = limit;
            _requestTimes = new Queue<DateTime>();
        }

        public async Task Invoke(HttpContext context)
        {
            while (_requestTimes.Count > 0 && DateTime.UtcNow - _requestTimes.Peek() > TimeSpan.FromSeconds(1))
            {
                _requestTimes.Dequeue();
            }

            if (_requestTimes.Count >= _limit)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }

            _requestTimes.Enqueue(DateTime.UtcNow);

            await _next(context);
        }
    }

}
