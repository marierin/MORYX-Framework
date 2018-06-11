﻿using System.Collections.Generic;

namespace Marvin.Workflows
{
    /// <summary>
    /// Interface of the workflow
    /// </summary>
    public interface IWorkflow
    {
        /// <summary>
        /// Workplan this workflow is an instance of
        /// </summary>
        IWorkplan Workplan { get; }

        /// <summary>
        /// All places
        /// </summary>
        IEnumerable<IPlace> Places { get; }

        /// <summary>
        /// All transitions of the workflow
        /// </summary>
        IEnumerable<ITransition> Transitions { get; }
    }
}