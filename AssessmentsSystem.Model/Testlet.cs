using System.Collections.Generic;

namespace AssessmentsSystem.Model
{
    public class Testlet
    {
        public Testlet(string testletId, List<Item> items)
        {
            TestletId = testletId;
            Items = items;
        }

        public string TestletId { get; set; }

        public List<Item> Items { get; }
    }
}