using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Msagl.Drawing;

namespace NodeMapper.Ui.Main
{
    public class NodeViewModel
    {
        public Node SelectedNode { get; set; }

        public string NodeDescription => SelectedNode != null ? SelectedNode.LabelText : "None Selected";

        public IEnumerable<string> EdgeItems => SelectedNode != null
            ? SelectedNode.Edges.Select(e => e.Source + " to " + e.Target)
            : Enumerable.Empty<string>();
    }
}