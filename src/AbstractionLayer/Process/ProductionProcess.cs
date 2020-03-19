// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

namespace Marvin.AbstractionLayer
{
    /// <summary>
    /// Concrete process of producing a single product instance
    /// </summary>
    public class ProductionProcess : Process
    {
        /// <summary>
        /// The product instance produced by this process.
        /// </summary>
        public ProductInstance ProductInstance { get; set; }
    }
}
