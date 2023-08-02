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

        [SetUp]
        public void SetUp()
        {
            testWorkplan = new Workplan { Name = "FirstPlan" };
            comparativeWorkplan = new Workplan { Name = "SecondPlan" };

            //first Workplan
            var start = testWorkplan.AddConnector("Start", NodeClassification.Start);
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
            comparativeWorkplan.AddStep(new PackagingTask(), new AssemblingParameters(), i, e, i, f);



        }

        [Test]
        public void TestComparingWorkplans()
        {
            bool result = testWorkplan.Equals(comparativeWorkplan);
            Assert.That(result, Is.True);
            
        }
    }

}
