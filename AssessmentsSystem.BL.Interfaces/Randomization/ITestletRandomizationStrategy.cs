using System.Collections.Generic;
using AssessmentsSystem.Model;

namespace AssessmentsSystem.BL.Interfaces.Randomization
{
    public interface ITestletRandomizationStrategy
    {
        IList<Item> GetRandomizedItems(IList<Item> sourceItemsList);
    }
}