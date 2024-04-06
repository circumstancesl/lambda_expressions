using System;
using System.Collections;
using System.Collections.Generic;

public class BinaryTreeNode<T>
{
    public T Value { get; set; }
    public BinaryTreeNode<T> Left { get; set; }
    public BinaryTreeNode<T> Right { get; set; }

    public BinaryTreeNode(T value)
    {
        Value = value;
    }

    public static BinaryTreeNode<T> operator ++(BinaryTreeNode<T> node)
    {
        if (node != null)
        {
            dynamic dynamicValue = node.Value;
            node.Value = dynamicValue + 1;
        }
        return node;
    }

    public static BinaryTreeNode<T> operator --(BinaryTreeNode<T> node)
    {
        if (node != null)
        {
            dynamic dynamicValue = node.Value;
            node.Value = dynamicValue - 1;
        }
        return node;
    }
}

public class BinaryTree<T> : IEnumerable<T>
{
    private BinaryTreeNode<T> root;

    public BinaryTree(BinaryTreeNode<T> root)
    {
        this.root = root;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return PreOrderTraversal().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerable<T> PreOrderTraversal()
    {
        if (root == null) yield break;

        Stack<BinaryTreeNode<T>> stack = new Stack<BinaryTreeNode<T>>();
        stack.Push(root);

        while (stack.Count > 0)
        {
            BinaryTreeNode<T> current = stack.Pop();
            yield return current.Value;

            if (current.Right != null)
            {
                stack.Push(current.Right);
            }

            if (current.Left != null)
            {
                stack.Push(current.Left);
            }
        }
    }

    public IEnumerable<T> PostOrderTraversal()
    {
        if (root == null) yield break;

        Stack<BinaryTreeNode<T>> stack = new Stack<BinaryTreeNode<T>>();
        BinaryTreeNode<T> current = root;
        BinaryTreeNode<T> lastNodeVisited = null;

        while (stack.Count > 0 || current != null)
        {
            if (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }
            else
            {
                BinaryTreeNode<T> peekNode = stack.Peek();
                if (peekNode.Right != null && lastNodeVisited != peekNode.Right)
                {
                    current = peekNode.Right;
                }
                else
                {
                    yield return peekNode.Value;
                    lastNodeVisited = stack.Pop();
                }
            }
        }
    }
}

public static class BinaryTreeExtensions
{
    public static IEnumerable<T> LambdaInOrderTraversal<T>(BinaryTreeNode<T> root)
    {
        if (root == null)
        {
            return Array.Empty<T>();
        }

        List<T> result = new List<T>();
        Action<BinaryTreeNode<T>> traverse = null;
        traverse = (node) =>
        {
            if (node.Left != null) traverse(node.Left);
            result.Add(node.Value);
            if (node.Right != null) traverse(node.Right);
        };

        traverse(root);
        return result;
    }
}

public class Program
{
    public static void Main()
    {
        BinaryTreeNode<int> root = new BinaryTreeNode<int>(1);
        root.Left = new BinaryTreeNode<int>(2);
        root.Right = new BinaryTreeNode<int>(3);
        root.Left.Left = new BinaryTreeNode<int>(4);
        root.Left.Right = new BinaryTreeNode<int>(5);

        BinaryTree<int> tree = new BinaryTree<int>(root);

        ++root.Left.Left;

        Console.WriteLine("Прямой обход дерева:");
        foreach (var nodeValue in tree.PreOrderTraversal())
        {
            Console.Write(nodeValue + " ");
        }
        Console.WriteLine();

        --root.Right;

        Console.WriteLine("Обратный обход дерева:");
        foreach (var nodeValue in tree.PostOrderTraversal())
        {
            Console.Write(nodeValue + " ");
        }
        Console.WriteLine();
        Console.ReadKey();
    }
}
