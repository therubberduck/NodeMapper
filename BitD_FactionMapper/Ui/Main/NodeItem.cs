﻿using BitD_FactionMapper.Model;

namespace BitD_FactionMapper.Ui.Main
{
    public class NodeItem
    {
        private readonly Node _node;
        public NodeItem(Node n)
        {
            _node = n;
        }

        public string NodeId => _node.NodeId;

        public override string ToString()
        {
            return _node.Title;
        }
    }
}