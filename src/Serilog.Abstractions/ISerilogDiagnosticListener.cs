namespace Rocket.Surgery.Extensions.Serilog
{
    /// <summary>
    /// Base diagnostic listener type for Rocket Surgery
    /// </summary>
    public interface ISerilogDiagnosticListener
    {
        /// <summary>
        /// Gets a value indicating which listener this instance should be subscribed to
        /// </summary>
        string ListenerName { get; }
    }
}

