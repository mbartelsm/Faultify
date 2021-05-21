using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Faultify.TestRunner.ProjectDuplication;
using NUnit.Framework;

namespace Faultify.Tests.UnitTests
{
    /// <summary>
    /// Test the file duplication system and duplication pool.
    /// </summary>
    internal class FileDuplicationTests
    {
        [Test]
        public void TestPoolPop()
        {
            TestProjectDuplicationPool pool = new TestProjectDuplicationPool(new List<TestProjectDuplication>
            {
                new TestProjectDuplication(null, null, 0),
            });

            Assert.AreEqual(pool.PopTestProject().DuplicationNumber, 0);
            Assert.AreEqual(pool.PopTestProject(), null);
        }

        [Test]
        public void GetTestProject()
        {
            TestProjectDuplicationPool pool = new TestProjectDuplicationPool(new List<TestProjectDuplication>
            {
                new TestProjectDuplication(null, null, 0),
                new TestProjectDuplication(null, null, 1),
            });

            TestProjectDuplication project1 = pool.GetTestProject();
            TestProjectDuplication project2 = pool.GetTestProject();

            Assert.AreEqual(project1.DuplicationNumber, 0);
            Assert.AreEqual(project2.DuplicationNumber, 1);
        }

        [Test]
        public void AcquireTestProjectParallel()
        {
            TestProjectDuplicationPool pool = new TestProjectDuplicationPool(new List<TestProjectDuplication>
            {
                new TestProjectDuplication(null, null, 0),
                new TestProjectDuplication(null, null, 1),
                new TestProjectDuplication(null, null, 2),
                new TestProjectDuplication(null, null, 3),
                new TestProjectDuplication(null, null, 4),
                new TestProjectDuplication(null, null, 5),
            });

            Parallel.ForEach(Enumerable.Range(0, 6), i =>
            {
                TestProjectDuplication project1 = pool.GetTestProject();
                Assert.NotNull(project1, null);
            });

            Assert.IsNull(pool.GetFreeProject());
        }

        [Test]
        public void AcquireTestProjectParallelAndFree()
        {
            TestProjectDuplicationPool pool = new TestProjectDuplicationPool(new List<TestProjectDuplication>
            {
                new TestProjectDuplication(null, null, 0),
                new TestProjectDuplication(null, null, 1),
                new TestProjectDuplication(null, null, 2),
                new TestProjectDuplication(null, null, 3),
                new TestProjectDuplication(null, null, 4),
                new TestProjectDuplication(null, null, 5),
            });

            Parallel.ForEach(Enumerable.Range(0, 6), i =>
            {
                TestProjectDuplication project1 = pool.GetTestProject();
                Assert.NotNull(project1);
                project1.MarkAsFree();
            });

            Assert.IsNotNull(pool.GetFreeProject());
        }
    }
}
