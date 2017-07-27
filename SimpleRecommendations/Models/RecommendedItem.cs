using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRecommendations.Models
{
    public class RecommendedItem
    {
        public string RecommendedItemId { get; set; }
        public decimal Score { get; set; }
    }
}
