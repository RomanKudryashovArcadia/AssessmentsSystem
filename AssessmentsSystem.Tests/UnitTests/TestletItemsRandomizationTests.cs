using System;
using System.Collections.Generic;
using System.Linq;
using AssessmentsSystem.BL.Exceptions;
using AssessmentsSystem.BL.Randomization;
using AssessmentsSystem.BL.Services;
using AssessmentsSystem.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssessmentsSystem.Tests.UnitTests
{
    [TestClass]
    public class TestletItemsRandomizationTests
    {
        private int _countOfPretestItems = 2;

        private readonly List<Item> _items = new List<Item>
        {
            new Item {ItemId = "item 01", ItemType = ItemTypeEnum.Operational},
            new Item {ItemId = "item 02", ItemType = ItemTypeEnum.Pretest},
            new Item {ItemId = "item 03", ItemType = ItemTypeEnum.Operational},
            new Item {ItemId = "item 04", ItemType = ItemTypeEnum.Operational},
            new Item {ItemId = "item 05", ItemType = ItemTypeEnum.Pretest},
            new Item {ItemId = "item 06", ItemType = ItemTypeEnum.Operational},
            new Item {ItemId = "item 07", ItemType = ItemTypeEnum.Operational},
            new Item {ItemId = "item 08", ItemType = ItemTypeEnum.Pretest},
            new Item {ItemId = "item 09", ItemType = ItemTypeEnum.Operational},
            new Item {ItemId = "item 10", ItemType = ItemTypeEnum.Pretest}
        };

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestletItemsRandomization_ArgumentNullException()
        {
            var testletService = new TestletService(new DefaultTestletRandomizationStrategy());
            testletService.GetRandomizedItems(null);
        }

        [TestMethod]
        public void TestletItemsRandomization_EmptyList()
        {
            var testletService = new TestletService(new DefaultTestletRandomizationStrategy());

            var resultListOfItems = testletService.GetRandomizedItems(new List<Item>());

            Assert.AreEqual(resultListOfItems.Count, 0);
        }

        [TestMethod]
        public void TestletItemsRandomization_LengthsAreTheSame()
        {
            var testletService = new TestletService(new DefaultTestletRandomizationStrategy());

            var testlet = CreateTestlet();

            var resultListOfItems = testletService.GetRandomizedItems(testlet.Items);

            Assert.AreEqual(testlet.Items.Count, resultListOfItems.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TestletRandomizationException))]
        public void TestletItemsRandomization_DefaultTestletRandomizationStrategySetup_StrategyIsNotDefined()
        {
            var testletService = new TestletService(null);

            var testlet = CreateTestlet();

            var resultListOfItems = testletService.GetRandomizedItems(testlet.Items);

            Assert.AreEqual(testlet.Items.Count, resultListOfItems.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TestletRandomizationException))]
        public void TestletItemsRandomization_DefaultTestletRandomizationStrategySetup_IncorrectNumberOfPretestItems()
        {
            var testletService = new TestletService(new DefaultTestletRandomizationStrategy(5));

            var testlet = CreateTestlet();

            var resultListOfItems = testletService.GetRandomizedItems(testlet.Items);

            Assert.AreEqual(testlet.Items.Count, resultListOfItems.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TestletRandomizationException))]
        public void TestletItemsRandomization_DefaultTestletRandomizationStrategySetup_IncorrectNumberOfOperationalItems()
        {
            var testletService = new TestletService(new DefaultTestletRandomizationStrategy(operationalItemsCount: 9));

            var testlet = CreateTestlet();

            var resultListOfItems = testletService.GetRandomizedItems(testlet.Items);

            Assert.AreEqual(testlet.Items.Count, resultListOfItems.Count);
        }

        [TestMethod]
        public void TestletItemsRandomization_FirstItemsArePretests()
        {
            var testletService = new TestletService(new DefaultTestletRandomizationStrategy());

            var testlet = CreateTestlet();

            var resultListOfItems = testletService.GetRandomizedItems(testlet.Items);

            Assert.IsTrue(resultListOfItems.Take(_countOfPretestItems).All(x => x.ItemType == ItemTypeEnum.Pretest));
        }

        [TestMethod]
        public void TestletItemsRandomization_FirstPretestItemsAppearOnlyOnce()
        {
            var testletService = new TestletService(new DefaultTestletRandomizationStrategy());

            var testlet = CreateTestlet();

            var resultListOfItems = testletService.GetRandomizedItems(testlet.Items);

            var firstItems = resultListOfItems.Take(_countOfPretestItems);

            var otherItems = resultListOfItems.Skip(_countOfPretestItems);

            var otherItemsContainsFirstItems = otherItems
                .Select(x => x.ItemId)
                .Intersect(firstItems.Select(x => x.ItemId))
                .Any();

            Assert.IsTrue(resultListOfItems.Count == testlet.Items.Count);

            Assert.IsFalse(otherItemsContainsFirstItems);
        }

        [TestMethod]
        public void TestletItemsRandomization_ItemsAreRandomized()
        {
            var testletService = new TestletService(new DefaultTestletRandomizationStrategy());

            var testlet = CreateTestlet();

            var resultListOfItems1 = testletService.GetRandomizedItems(testlet.Items);
            var resultListOfItems2 = testletService.GetRandomizedItems(testlet.Items);

            var result1Ids = resultListOfItems1.Select(x => x.ItemId).ToList();
            var result2Ids = resultListOfItems2.Select(x => x.ItemId).ToList();
            
            var sequenceEqual = result1Ids.SequenceEqual(result2Ids);

            Assert.IsTrue(!sequenceEqual);
        }

        private Testlet CreateTestlet()
        {
            return new Testlet("testlet1", _items);
        }
    }
}