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
        return InOrderTraversal().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private IEnumerable<T> InOrderTraversal()
    {
        if (root == null) yield break;

        Stack<BinaryTreeNode<T>> stack = new Stack<BinaryTreeNode<T>>();
        BinaryTreeNode<T> current = root;

        while (stack.Count > 0 || current != null)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }

            current = stack.Pop();
            yield return current.Value;
            current = current.Right;
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

        foreach (var nodeValue in tree)
        {
            Console.Write(nodeValue + " ");
        }
        Console.WriteLine();

        var inOrderTraversalValues = BinaryTreeExtensions.LambdaInOrderTraversal(root);
        foreach (var nodeValue in inOrderTraversalValues)
        {
            Console.Write(nodeValue + " ");
        }
        Console.ReadKey();
    }
}
