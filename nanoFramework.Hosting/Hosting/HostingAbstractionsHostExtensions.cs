namespace nanoFramework.Hosting
{
    public static class HostingAbstractionsHostExtensions
    {
        /// <summary>
        /// Starts the host synchronously.
        /// </summary>
        /// <param name="host">The <see cref="IHost"/> to start.</param>
        public static void Start(this IHost host)
        {
            host.Start();
        }

        /// <summary>
        /// Attempts to gracefully stop the host with the given timeout.
        /// </summary>
        /// <param name="host">The <see cref="IHost"/> to stop.</param>
        public static void Stop(this IHost host)
        {
            host.Stop();
        }

        /// <summary>
        /// Runs an application and block the calling thread until host shutdown.
        /// </summary>
        /// <param name="host">The <see cref="IHost"/> to run.</param>
        public static void Run(this IHost host)
        {
            host.Start();
        }
    }
}