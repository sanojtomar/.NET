using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Implementation1;
using System.Collections.Generic;

namespace Implementation1Test
{
    [TestClass]
    public class Implementation1UntiTest
    {
        [TestMethod]
        public void Test_Process_Equal()
        {
            string firstName = "Joe";
            string lastName = "Browns";

            List<string> expectedItems = new List<string>();

            for (int i = 1; i <= 100; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    expectedItems.Add(string.Format("{0} {1}", firstName, lastName));
                    continue;
                }

                if (i % 3 == 0)
                {
                    expectedItems.Add(firstName);
                    continue;
                }

                if (i % 5 == 0)
                {
                    expectedItems.Add(lastName);
                    continue;
                }
                expectedItems.Add(i.ToString());
            }

            DoWork doWork = new DoWork();
            List<string> actualItems = doWork.Process(firstName, lastName) as List<string>;

            CollectionAssert.AreEqual(expectedItems, actualItems);

        }

        [TestMethod]
        public void Test_Process_NoEqual()
        {
            string firstName = "Joe";
            string lastName = "Browns";

            List<string> expectedItems = new List<string>();

            for (int i = 1; i <= 100; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    expectedItems.Add(string.Format("{0} {1}", firstName, lastName));
                    continue;
                }

                if (i % 3 == 0)
                {
                    expectedItems.Add(firstName);
                    continue;
                }

                if (i % 5 == 0)
                {
                    expectedItems.Add(lastName);
                    continue;
                }
                expectedItems.Add(i.ToString());
            }

            DoWork doWork = new DoWork();
            List<string> actualItems = doWork.Process("John", "Barrow") as List<string>;

            CollectionAssert.AreNotEqual(expectedItems, actualItems);

        }
    }
}
