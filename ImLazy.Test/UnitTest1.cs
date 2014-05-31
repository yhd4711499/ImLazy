using System.Linq;
using ImLazy.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImLazy.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod_LexerRuntime()
        {
            var count = LexerAddinHost.Instance.Subjects.Count();
            Assert.AreEqual(count, 2);
        }

        [TestMethod]
        public void TestMethod_Clone_Rule()
        {
//            var r = new Rule
//            {
//                ConditionBranch = new ConditionBranch
//                {
//                    SubConditions = new List<ConditionCorp>(){
//                        AddinInfoFactory.Create<ConditionLeaf>(typeof (UnitTest1))
//                    }
//                },
//                Actions = new List<ActionAddinInfo>()
//                {
//                    AddinInfoFactory.Create<ActionAddinInfo>(typeof (UnitTest1))
//                }
//            };
//            r.Clone();
        }

        [TestMethod]
        public void TestMethod_MEF_Load()
        {
            var count = AddinHost.Instance.ConditionAddins.Count();
            const int expected = 1;
            Assert.AreEqual(expected, count);
        }

        [TestMethod]
        public void TestMethod_Executor_Execute()
        {
            var executor = Executor.Instance;
            executor.Execute(DataStorage.Instance.Folders);
        }
    }
}
