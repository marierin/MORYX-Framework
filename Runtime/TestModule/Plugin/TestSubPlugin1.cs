﻿using Marvin.Container;
using Marvin.Modules;

namespace Marvin.TestModule
{
    [ExpectedConfig(typeof(TestSubPluginConfig1))]
    [Plugin(LifeCycle.Singleton, typeof(ITestSubPlugin), Name = ComponentName)]
    public class TestSubPlugin1 : ITestSubPlugin
    {
        public const string ComponentName = "TestSubPlugin1";

        public void Initialize(TestSubPluginConfig config)
        {
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}