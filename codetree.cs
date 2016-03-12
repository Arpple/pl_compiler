using System;
using System.Collections.Generic;
using System.Linq;

namespace PL
{
    public class CodeTree
    {
        private Dictionary<string,CodeNode> _labels;

        public CodeNode root;
        private CodeNode lastNode;

        public CodeTree()
        {
            _labels = new Dictionary<string,CodeNode>();
            this.root = new CodeNode();
            this.lastNode = root;
        }

        public void insertNode(CodeNode node)
        {
            lastNode.next = node;
            lastNode = node;
        }

        public void insertLabel(string labelName)
        {
            Console.WriteLine("LABEL");
            _labels.Add(labelName,lastNode);
        }

        public CodeNode getNodeFromLabel(string labelName)
        {
            CodeNode node = null;
            _labels.TryGetValue(labelName, out node);
            return node;
        }

        public void printTree()
        {
            foreach(KeyValuePair<string,CodeNode> entry in _labels)
            {
                if(entry.Value == root)
                {
                    Console.WriteLine(entry.Key);
                    break;
                }
            }
            print(root.next);
        }

        public void print(CodeNode node)
        {
            if(node != null)
            {
                Console.WriteLine(node);
                foreach(KeyValuePair<string,CodeNode> entry in _labels)
                {
                    if(entry.Value == node)
                    {
                        Console.WriteLine(entry.Key);
                        break;
                    }
                }
                print(node.next);
            }
        }

    }
}
