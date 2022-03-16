using System.Collections.Generic;
using AssessmentsSystem.BL.Interfaces.Randomization;
using AssessmentsSystem.Model;

namespace AssessmentsSystem.BL.Interfaces.Services
{
    public interface ITestletService
    {
        void SetTestletRandomizationStrategy(ITestletRandomizationStrategy _testletRandomizationStrategy);

        IList<Item> GetRandomizedItems(IList<Item> sourceItemsList);
    }
}