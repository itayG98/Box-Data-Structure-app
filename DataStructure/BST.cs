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
             Root = Remove(Root, node.Key);
        }
        public TreeNode Remove(TreeNode father, K KeyToDelete)
        {
            if (father == null) //If the tree is empty or the key couldnt be found
                return father;

            if (KeyToDelete.CompareTo(father.Key) < 0)
                father.Left = Remove(father.Left, KeyToDelete);
            else if (KeyToDelete.CompareTo(father.Key) > 0)
                father.Right = Remove(father.Right, KeyToDelete);

            // if the keys match we have the father and the node delete the node
            else
            {
                // node with only one child or no child
                if (father.IsLeaf())
                    return null;
                else if (father.Left == null)
                {
                    return father.Right;
                }
                else if (father.Right == null)
                {
                    return father.Left;
                }

                TreeNode newFather = FindMinNode(father.Right);
                father.Value = newFather.Value;
                father.Key = newFather.Key;
                father.Right = Remove(father.Right, father.Key);
            }
            return father;
        }

        //===============================================================================================
        private TreeNode FindMaxNode(TreeNode node)
        {
            if (node == null)
                return null;
            if (node.Right == null)
                return node;
            return FindMaxNode(node.Right);
        }
        private TreeNode FindMinNode(TreeNode node)
        {
            if (node == null)
                return null;
            if (node.Left == null)
                return node;
            return FindMinNode(node.Left);
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
                        break;
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
        public IEnumerable GetNextTreeByRange(K min, K max)
        {
            if (max.CompareTo(min) >= 0)
            {
                TreeNode commonFather = Root;
                while (commonFather != null)
                {
                    if (commonFather.CompareTo(min) < 0)
                        commonFather = commonFather.Right;
                    else if (commonFather.CompareTo(max) > 0)
                        commonFather = commonFather.Left;
                    else
                        break;
                }
                foreach (TreeNode node in Inorder(commonFather))
                {
                    if (node.CompareTo(min) >= 0 && node.CompareTo(max) <= 0)
                        yield return node.Value;
                    else if (node.CompareTo(max) > 0)
                        break;
                }
            }
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
                    return default;
            }
        }
        private IEnumerable InorderValue(TreeNode node)
        {
            if (node != null)
            {
                foreach (V val in InorderValue(node.Left))
                    yield return val;
                yield return node.Value;
                foreach (V val in InorderValue(node.Right))
                    yield return val;
            }
        }
        private IEnumerable Inorder(TreeNode node)
        {
            if (node != null)
            {
                foreach (TreeNode n in Inorder(node.Left))
                    yield return n;
                yield return node;
                foreach (TreeNode n in Inorder(node.Right))
                    yield return n;
            }
        }
        private IEnumerable PreOrderValue(TreeNode node)
        {
            if (node != null)
            {
                yield return node.Value;
                foreach (V val in PreOrderValue(node.Left))
                    yield return val;
                foreach (V val in PreOrderValue(node.Right))
                    yield return val;
            }
        }
        private IEnumerable RightPostOrderValue(TreeNode node)
        {
            if (node != null)
            {
                foreach (V val in RightPostOrderValue(node.Right))
                    yield return val;
                yield return node.Value;
                foreach (V val in RightPostOrderValue(node.Left))
                    yield return val;
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
            }

            public bool IsLeaf() => Left == null && Right == null;

            public int CompareTo(K key) => Key.CompareTo(key);
        }
    }
}

