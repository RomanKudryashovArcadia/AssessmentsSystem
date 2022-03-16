using System;
using System.Collections.Generic;
using System.Linq;
using AssessmentsSystem.BL.Exceptions;
using AssessmentsSystem.BL.Extensions;
using AssessmentsSystem.BL.Interfaces.Randomization;
using AssessmentsSystem.Model;

namespace AssessmentsSystem.BL.Randomization
{
    public class DefaultTestletRandomizationStrategy : ITestletRandomizationStrategy
    {
        private const int PRETEST_ITEMS_COUNT = 4;
        private const int OPERATIONAL_ITEMS_COUNT = 6;
        private const int COUNT_OF_FIRST_PRETEST_ITEMS = 2;

        private readonly int _pretestItemsCount;
        private readonly int _operationalItemsCount;
        private readonly int _countOfFirstPretestItems;
        public readonly Random _random;

        public DefaultTestletRandomizationStrategy(
            int pretestItemsCount = PRETEST_ITEMS_COUNT,
            int operationalItemsCount = OPERATIONAL_ITEMS_COUNT,
            int countOfFirstPretestItems = COUNT_OF_FIRST_PRETEST_ITEMS)
        {
            _pretestItemsCount = pretestItemsCount;
            _operationalItemsCount = operationalItemsCount;
            _countOfFirstPretestItems = countOfFirstPretestItems;
            _random = new Random();
        }

        public IList<Item> GetRandomizedItems(IList<Item> sourceItemsList)
        {
            if (sourceItemsList == null)
            {
                throw new ArgumentNullException(nameof(sourceItemsList));
            }

            var itemsCountsByType = sourceItemsList
                .GroupBy(item => item.ItemType)
                .ToDictionary(x => x.Key, x => x.Count());

            itemsCountsByType.TryGetValue(ItemTypeEnum.Pretest, out var realPretestItemsCount);
            itemsCountsByType.TryGetValue(ItemTypeEnum.Operational, out var realOperationalItemsCount);

            if (realPretestItemsCount != _pretestItemsCount)
            {
                throw new TestletRandomizationException($"Pretest items count must be {_pretestItemsCount} but found {realPretestItemsCount}");
            }

            if (realOperationalItemsCount != _operationalItemsCount)
            {
                throw new TestletRandomizationException($"Operational items count must be {_operationalItemsCount} but found {realOperationalItemsCount}");
            }

            var shuffledArray = sourceItemsList.ToArray();

            shuffledArray.Shuffle(_random);

            var array = new Item[sourceItemsList.Count];
            var index = 0;
            var pretestIndex = 0;

            foreach (var item in shuffledArray)
            {
                if (item.ItemType == ItemTypeEnum.Pretest && pretestIndex < _countOfFirstPretestItems)
                {
                    array[pretestIndex] = item;
                    pretestIndex++;
                }
                else
                {
                    array[index + _countOfFirstPretestItems] = item;
                    index++;
                }
            }

            return array;
        }
    }
}