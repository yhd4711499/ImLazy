using System.Linq;
using ImLazy.Addins.Conditions;
using ImLazy.RunTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImLazy.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod_LexerRuntime()
        {
            var count = LexerRuntime.Instance.Subjects.Count();
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
            var executor = new Executor(
                CacheMap<object>.ConditionCacheMap,
                CacheMap<object>.ActionCacheMap,
                CacheMap<object>.RuleCacheMap
                );
            executor.Execute(DataStorage.Instance.Folders);
        }
    }
}
