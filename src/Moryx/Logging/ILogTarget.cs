// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;

namespace Moryx.Logging
{
    /// <summary>
    /// Represents a generic target logger.
    /// Can be implemented to provide any type of logging framework.
    /// </summary>
    public interface ILogTarget
    {
        /// <summary>
        /// Simply log the message wit the given <see cref="LogLevel"/>
        /// </summary>
        void Log(LogLevel logLevel, string message);

        /// <summary>
        /// Log the message with the given <see cref="LogLevel"/> and additional exception
        /// </summary>
        void Log(LogLevel logLevel, string message, Exception exception);
    }
}
