using System;
using System.Collections.Generic;
using AssessmentsSystem.BL.Exceptions;
using AssessmentsSystem.BL.Interfaces.Randomization;
using AssessmentsSystem.BL.Randomization;
using AssessmentsSystem.Model;

namespace AssessmentsSystem.BL.Services
{
    public class TestletService
    {
        private readonly ITestletRandomizationStrategy _testletRandomizationStrategy;

        public TestletService() : this(new DefaultTestletRandomizationStrategy())
        {
        }

        public TestletService(ITestletRandomizationStrategy testletRandomizationStrategy)
        {
            _testletRandomizationStrategy = testletRandomizationStrategy;
        }

        public IList<Item> GetRandomizedItems(IList<Item> sourceItemsList)
        {
            if (sourceItemsList == null)
            {
                throw new ArgumentNullException(nameof(sourceItemsList));
            }

            if (sourceItemsList.Count == 0)
            {
                return sourceItemsList;
            }

            if (_testletRandomizationStrategy == null)
            {
                throw new TestletRandomizationException("Testlet randomization strategy is not defined");
            }

            return _testletRandomizationStrategy.GetRandomizedItems(sourceItemsList);
        }
    }
}