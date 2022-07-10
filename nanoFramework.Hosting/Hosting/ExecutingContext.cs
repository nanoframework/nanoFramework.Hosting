using System;

namespace nanoFramework.Hosting
{
    public class ExecutingContext
    {
        /// <summary>
        /// Gets whether cancellation has been requested for this service.
        /// </summary>
        public bool IsCancellationRequested { get; set; } = false;

        public Exception Exception { get; set; }
        
    }
}
