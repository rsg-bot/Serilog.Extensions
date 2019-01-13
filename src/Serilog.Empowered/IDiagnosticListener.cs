namespace Rocket.Surgery.Extensions.Serilog.Empowered
{
    /// <summary>
    /// Base diagnostic listener type for Rocket Surgery
    /// </summary>
    interface IDiagnosticListener
    {
        /// <summary>
        /// Gets a value indicating which listener this instance should be subscribed to
        /// </summary>
        string ListenerName { get; }
    }
}
