using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class CompositeEdge<TVertex> : Edge<TVertex>
    {
        public IEnumerable<EdgeWithId<TVertex>> ComponentEdges { get; set; }

        public CompositeEdge(TVertex source, TVertex target, IEnumerable<EdgeWithId<TVertex>> componentEdges)  
            : base (source, target)
        {
            ComponentEdges = componentEdges;
        }
    }
}
