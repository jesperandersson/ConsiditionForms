using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class EdgeWithId<TVertex> : Edge<TVertex>
    {
        public string Id { get; set; }

        public double EmissionCost { get; set; }

        public double TimeCost { get; set; }

        public EdgeWithId(TVertex source, TVertex target, string id, double emissionCost = 0, double timeCost = 0) : base(source, target)
        {
            Id = id;
            EmissionCost = emissionCost;
            TimeCost = timeCost;
        }

        public double GetAlgorithmCost()
        {
            return 1 * EmissionCost +  1 * TimeCost;
        }

        public double GetTrueCost()
        {
            return 1 * EmissionCost + 1 * TimeCost;
        }
    }
}
