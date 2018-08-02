using System;
using System.Collections.Generic;
using System.Text;

namespace S.NET.STree
{
    public class SNodeFull : SNode
    {
        public bool IsLeaf { get; }
        public SNodeFull(bool isLeaf) : base()
        {
            this.IsLeaf = isLeaf;
        }

        public SNodeFull(string name, bool isLeaf) : base(name)
        {
            this.IsLeaf = isLeaf;
        }

        public SNodeFull RootNode { get; set; }

        public void AddNode(SNodeFull node)
        {
            base.AddNode(node);
            node.RootNode = this;
        }
    }
}
