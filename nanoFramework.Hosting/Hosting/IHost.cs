﻿using System;

namespace nanoFramework.Hosting
{
    /// <summary>
    /// A program abstraction.
    /// </summary>
    public interface IHost : IDisposable
    {
        /// <summary>
        /// The programs configured services.
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// Start the program.
        /// </summary>
        void Start();

        /// <summary>
        /// Attempts to gracefully stop the program.
        /// </summary>
        void Stop();
    }
}