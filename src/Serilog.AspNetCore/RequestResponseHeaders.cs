namespace Rocket.Surgery.Extensions.Serilog.AspNetCore
{
    /// <summary>
    /// Header names for requests / responses.
    /// </summary>
    static class RequestResponseHeaders
    {
        /// <summary>
        /// Standard parent Id header.
        /// </summary>
        public const string StandardParentIdHeader = "x-ms-request-id";

        /// <summary>
        /// Standard root id header.
        /// </summary>
        public const string StandardRootIdHeader = "x-ms-request-root-id";

        /// <summary>
        /// Request-Id header.
        /// </summary>
        public const string RequestIdHeader = "Request-Id";

        /// <summary>
        /// Correlation-Context header.
        /// </summary>
        public const string CorrelationContextHeader = "Correlation-Context";
    }
}
