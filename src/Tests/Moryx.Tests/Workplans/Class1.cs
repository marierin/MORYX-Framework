using Moryx.Workplans;
using Moryx.AbstractionLayer;
using Moryx.Tests.Workplans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moryx.Tests.Workplans.Dummies;

namespace Moryx.Tests.Workplans
{

    [TestFixture]
    public class Test
    {
        private Workplan testWorkplan;
        private Workplan comparativeWorkplan;
        private Workplan[] workPlans;

        [SetUp]
        public void SetUp()
        {
            testWorkplan = new Workplan { Name = "FirstPlan" };
            comparativeWorkplan = new Workplan { Name = "SecondPlan" };
            workPlans = new Workplan[] {testWorkplan,comparativeWorkplan};

            foreach (var workPlan in workPlans)
            {
                var start = workPlan.AddConnector("Start", NodeClassification.Start);
                var end = workPlan.AddConnector("End", NodeClassification.End);
                var failed = workPlan.AddConnector("Failed", NodeClassification.Failed);

                var input = start;
                var output1 = workPlan.AddConnector("A");
                var output2 = workPlan.AddConnector("B");
                var output3 = workPlan.AddConnector("C");
                var output4 = workPlan.AddConnector("D");
                workPlan.AddStep(new AssemblingTask(), new AssemblingParameters(), input, output1, output2, failed);

                input = output1;

                workPlan.AddStep(new ColorizingTask(), new AssemblingParameters(), input, end, end, failed);

                input = output2;
                workPlan.AddStep(new PackagingTask(), new AssemblingParameters(), input, end, input, failed);
            }

            //first Workplan
            /*var start = testWorkplan.AddConnector("Start", NodeClassification.Start);
            var end = testWorkplan.AddConnector("End", NodeClassification.End);
            var failed = testWorkplan.AddConnector("Failed", NodeClassification.Failed);

            var input = start;
            var output1 = testWorkplan.AddConnector("A");
            var output2 = testWorkplan.AddConnector("B");
            var output3 = testWorkplan.AddConnector("C");
            var output4 = testWorkplan.AddConnector("D");
            testWorkplan.AddStep(new AssemblingTask(), new AssemblingParameters(), input, output1, output2, failed);

            input = output1;
          
            testWorkplan.AddStep(new ColorizingTask(), new AssemblingParameters(), input, end, end, failed);

            input = output2;
            testWorkplan.AddStep(new PackagingTask(), new AssemblingParameters(), input, end, input, failed);

            //second Workplan
            var s = comparativeWorkplan.AddConnector("Start", NodeClassification.Start);
            var e = comparativeWorkplan.AddConnector("End", NodeClassification.End);
            var f = comparativeWorkplan.AddConnector("Failed", NodeClassification.Failed);

            var i = s;
            var o1 = comparativeWorkplan.AddConnector("A");
            var o2 = comparativeWorkplan.AddConnector("B");
            var o3 = comparativeWorkplan.AddConnector("C");
            comparativeWorkplan.AddStep(new AssemblingTask(), new AssemblingParameters(), i, o1, o2, f);

            i = o1;
            comparativeWorkplan.AddStep(new ColorizingTask(), new AssemblingParameters(), i, e, e, f);

            i = o2;
            comparativeWorkplan.AddStep(new PackagingTask(), new AssemblingParameters(), i, e, i, f);*/



        }
        [Test]
        public void TestComparingWorkplans()
        {
            bool result = testWorkplan.Equals(comparativeWorkplan);
            Assert.That(result, Is.True);
            
        }

        [Test]
        public void TestComparingWorkplans2()
        {
            var lastStep = comparativeWorkplan.Steps.Last();
            var end = lastStep.Outputs[0];
            var failed = lastStep.Outputs[2];
            var connector = comparativeWorkplan.AddConnector("A");
            lastStep.Outputs[0] = connector;
            comparativeWorkplan.AddStep(new PackagingTask(), new AssemblingParameters(), connector, end, failed, failed);
            
            bool result = testWorkplan.Equals(comparativeWorkplan);
            Assert.That(result, Is.True);

        }
    }

}
