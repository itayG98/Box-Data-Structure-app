using System;
using System.Collections;

namespace DataStructure
{
    /// <summary>
    /// A Binary search tree data structure
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// 

    public enum Order { InOrderV, PreOrderV, RightPostOrderV };
    public enum Direction { Left, Right };
    public class BST<K, V> where K : IComparable
    {
        private TreeNode _root;
        public BST<K, V>.TreeNode Root { get => _root; private set => _root = value; }

        public BST()
        {
            Root = null;
        }
        public BST(K key, V value)
        {
            Root = new TreeNode(key, value);
        }

        //===============================================================================================
        public V FindValue(K key)
        //A Tree can Search K Type And return a V type value.Return deafult if not exist.
        {
            if (!IsEmpty())
                return default;
            return FindValue(key, Root);
        }
        private V FindValue(K key, TreeNode root)
        {
            if (root == null)
                return default;

            int comp = Root.CompareTo(key);
            if (comp == 0)
                return root.Value;
            else if (comp > 0)
                return FindValue(key, root.Right);
            else
                return FindValue(key, root.Left);
        }

        //===============================================================================================
        public TreeNode FindNode(K key) => FindNode(key, Root);
        //A Tree can Search K Type And return a TreeNode.Return deafult if not exist.

        private TreeNode FindNode(K key, TreeNode node)
        {
            if (node == null)
                return default;

            int comp = node.CompareTo(key);
            if (comp == 0)
                return node;
            else if (comp < 0)
                return FindNode(key, node.Right);
            else
                return FindNode(key, node.Left);
        }

        //===============================================================================================

        public void AddNode(K key, V val, TreeNode node)
        {
            int comp = node.CompareTo(key);
            if (comp > 0)
            {
                if (node.Left == null)
                    node.Left = new TreeNode(key, val);
                else
                    AddNode(key, val, node.Left);
            }
            else if (comp < 0)
            {
                if (node.Right == null)
                    node.Right = new TreeNode(key, val);
                else
                    AddNode(key, val, node.Right);
            }
        }
        public void AddNode(K key, V val)
        {
            if (IsEmpty())
            {
                Root = new TreeNode(key, val);
                return;
            }
            AddNode(key, val, Root);
        }
        public void AddNode(TreeNode node) => AddNode(node.Key, node.Value);
        //===============================================================================================
        public void Remove(TreeNode node)
        {
            if (node == null)
                return;
            else if (Root == node)
            {
                if (node.Left == null)
                    Root = node.Right;
                else if (node.Right == null)
                    Root = node.Left;
                else
                {
                    var newRoot = FindMaxNode(node.Left);
                    var toElavte = FindMinNode(newRoot);
                    toElavte.Left = node.Left;
                    newRoot.Right = node.Right;
                }
            }
            else
            {
                var fatherNode = FindFather(Root, node, out Direction dir);
                if (dir == Direction.Right)
                {
                    if (node.Left == null)
                        fatherNode.Right = node.Right;
                    else if (node.Right == null)
                        fatherNode.Right = node.Left;
                    else
                    {
                        var toElavete = FindMaxNode(node.Left);
                        toElavete.Right = node.Right;
                        fatherNode.Right = toElavete;
                        var leftNode = FindMinNode(toElavete);
                        leftNode.Left = node.Left;
                    }
                }
                else if (dir == Direction.Left)
                {
                    if (node.Left == null)
                        fatherNode.Left = node.Right;
                    else if (node.Right == null)
                        fatherNode.Left = node.Left;
                    else
                    {
                        fatherNode.Left = node.Right;
                        var toElavete = FindMinNode(node.Right);
                        toElavete.Left = node.Left;
                    }
                }
            }
        }

        //===============================================================================================
        private TreeNode FindFather(TreeNode fatherNode, TreeNode SonNode, out Direction direction)
        {
            if (fatherNode.Left.Equals(SonNode))
            {
                direction = Direction.Left;
                return fatherNode;
            }
            else if (fatherNode.Right.Equals(SonNode))
            {
                direction = Direction.Right;
                return fatherNode;
            }
            var right = FindFather(SonNode.Right, SonNode, out direction);
            if (right != null)
                return right;
            var left = FindFather(SonNode.Left, SonNode, out direction);
            if (right != null)
                return left;
            return null;
        }
        private TreeNode FindMaxNode(TreeNode node)
        {
            if (node.Right == null)
                return node;
            return FindMaxNode(node.Right);
        }
        private TreeNode FindMinNode(TreeNode node)
        {
            if (node.Right == null)
                return node;
            return FindMaxNode(node.Right);
        }

        //===============================================================================================

        public BST<K, V> GetTreeByMinKey(K key)
        {
            BST<K, V> fitKey = new BST<K, V>();
            foreach (TreeNode node in RightPostOrder(Root))
            {
                if (node.CompareTo(key) >= 0)
                    fitKey.AddNode(node.Key, node.Value);
                else
                    break;
            }
            return fitKey;
        }
        public BST<K, V> GetTreeByMaxKey(K key)
        {
            BST<K, V> fitKey = new BST<K, V>();
            foreach (TreeNode node in Inorder(Root))
            {
                if (node.CompareTo(key) <= 0)
                    fitKey.AddNode(node.Key, node.Value);
                else
                    break;
            }
            return fitKey;
        }
        public BST<K, V> GetTreeByRange(K min, K max)
        {
            if (max.CompareTo(min) >= 0)
            {
                BST<K, V> fitKey = new BST<K, V>();
                TreeNode commonFather = Root;
                while (commonFather != null)
                {
                    if (commonFather.CompareTo(min) < 0)
                        commonFather = commonFather.Right;
                    else if (commonFather.CompareTo(max) > 0)
                        commonFather = commonFather.Left;
                    else
                        break ;
                }
                foreach (TreeNode node in Inorder(commonFather))
                {
                    if (node.CompareTo(min) >= 0 && node.CompareTo(max) <= 0)
                        fitKey.AddNode(node.Key, node.Value);
                    else if (node.CompareTo(max) > 0)
                        break;
                }
                return fitKey;
            }
            return null;
        }

        //===============================================================================================
        public bool IsEmpty() => Root == null;
        //===============================================================================================


        public IEnumerable GetEnumerator(Order ord)

        {
            switch (ord)
            {
                case Order.InOrderV:
                    return InorderValue(Root);
                case Order.PreOrderV:
                    return PreOrderValue(Root);
                case Order.RightPostOrderV:
                    return RightPostOrderValue(Root);
                default:
                    return null;
            }
        }
        private IEnumerable InorderValue(TreeNode node)
        {
            if (node != null)
            {
                foreach (var n in InorderValue(node.Left))
                    yield return n;
                yield return node.Value;
                foreach (var n in InorderValue(node.Right))
                    yield return n;
            }
        }
        private IEnumerable Inorder(TreeNode node)
        {
            if (node != null)
            {
                foreach (var n in Inorder(node.Left))
                    yield return n;
                yield return node;
                foreach (var n in Inorder(node.Right))
                    yield return n;
            }
        }
        private IEnumerable PreOrderValue(TreeNode node)
        {
            if (node != null)
            {
                yield return node.Value;
                foreach (var n in PreOrderValue(node.Left))
                    yield return n;
                foreach (var n in PreOrderValue(node.Right))
                    yield return n;
            }
        }
        private IEnumerable RightPostOrderValue(TreeNode node)
        {
            if (node != null)
            {
                foreach (var n in RightPostOrderValue(node.Right))
                    yield return n;
                yield return node.Value;
                foreach (var n in RightPostOrderValue(node.Left))
                    yield return n;
            }
        }
        private IEnumerable RightPostOrder(TreeNode node)
        {
            if (node != null)
            {
                foreach (TreeNode n in RightPostOrder(node.Right))
                    yield return n;
                yield return node;
                foreach (TreeNode n in RightPostOrder(node.Left))
                    yield return n;
            }
        }

        //===============================================================================================

        public class TreeNode
        {
            private K _key;
            private V _value;
            private TreeNode _left;
            private TreeNode _Right;
            public K Key { get => _key; set => _key = value; }
            public V Value { get => _value; set => _value = value; }
            public BST<K, V>.TreeNode Left { get => _left; set => _left = value; }
            public BST<K, V>.TreeNode Right { get => _Right; set => _Right = value; }

            public TreeNode(K key, V value)
            {
                Key = key;
                Value = value;
                Left = null;
                Right = null;
            }

            public bool IsLeaf() => Left == null && Right == null;

            public int CompareTo(K key) => Key.CompareTo(key);

        }
    }
}

