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
        private readonly int _pretestItemsCount;
        private readonly int _operationalItemsCount;
        private readonly int _countOfFirstPretestItems;
        private readonly bool _shouldShuffleFirst;
        public readonly Random _random;

        public DefaultTestletRandomizationStrategy(
            int pretestItemsCount = 4,
            int operationalItemsCount = 6,
            int countOfFirstPretestItems = 2,
            bool shouldShuffleFirst = true)
        {
            _pretestItemsCount = pretestItemsCount;
            _operationalItemsCount = operationalItemsCount;
            _countOfFirstPretestItems = countOfFirstPretestItems;
            _shouldShuffleFirst = shouldShuffleFirst;
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

            if (_shouldShuffleFirst)
            {
                shuffledArray.Shuffle(_random);
            }

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