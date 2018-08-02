using System;
using System.Collections.Generic;
using System.Text;

namespace S.NET.STree
{
    public class SNode
    {
        private List<SNode> _items;
        public string Name { get; set; }
        public IReadOnlyCollection<SNode> Items { get { return _items.AsReadOnly(); } }
        public SNode()
        {
            this._items = new List<SNode>();
        }
        public SNode(string name) : this()
        {
            this.Name = name;
        }
        public void AddNode(SNode node)
        {
            this._items.Add(node);
        }
    }
}
