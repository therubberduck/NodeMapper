using System.Collections.Generic;
using System.Linq;
using BitD_FactionMapper.Model;

namespace BitD_FactionMapper.Ui.Main
{
    public static class UiItemMapper
    {
        public static IEnumerable<EdgeItem> Map(IEnumerable<Edge> edges)
        {
            return edges.Select(e => new EdgeItem(e));
        }
        
        public static IEnumerable<NodeItem> Map(IEnumerable<Node> nodes)
        {
            return nodes.Select(n => new NodeItem(n));
        }
    }
}