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
    private Func<BinaryTreeNode<T>, IEnumerator<T>> traversalStrategy;

    public BinaryTree(BinaryTreeNode<T> root, Func<BinaryTreeNode<T>, IEnumerator<T>> traversalStrategy)
    {
        this.root = root;
        this.traversalStrategy = traversalStrategy;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return traversalStrategy(root);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class PreOrderIterator<T> : IEnumerator<T>
{
    private Stack<BinaryTreeNode<T>> stack;
    private BinaryTreeNode<T> current;

    public PreOrderIterator(BinaryTreeNode<T> root)
    {
        stack = new Stack<BinaryTreeNode<T>>();
        if (root != null)
        {
            stack.Push(root);
        }
    }

    public T Current { get; set; }

    object IEnumerator.Current => Current;

    public void Dispose() {}

    public bool MoveNext()
    {
        if (stack.Count == 0)
        {
            return false;
        }

        BinaryTreeNode<T> current = stack.Pop();
        Current = current.Value;

        if (current.Right != null)
        {
            stack.Push(current.Right);
        }
        if (current.Left != null)
        {
            stack.Push(current.Left);
        }

        return true;
    }

    public void Previous()
    {
        if (current == null)
            return;
        if (current.Left != null)
        {
            current = current.Left;
            while (current.Right != null)
            {
                current = current.Right;
            }
        }
        else
        {
            while (current != null && current.Left == null)
            {
                current = current.Left;
            }
            if (current != null)
            {
                current = current.Left;
                while (current.Right != null)
                {
                    current = current.Right;
                }
            }
        }
    }

    public void Reset()
    {
        throw new Exception();
    }
}

public class PostOrderIterator<T> : IEnumerator<T>
{
    private Stack<BinaryTreeNode<T>> stack;
    private BinaryTreeNode<T> current;

    public PostOrderIterator(BinaryTreeNode<T> root)
    {
        stack = new Stack<BinaryTreeNode<T>>();
        current = root;
    }

    public T Current { get; set; }

    object IEnumerator.Current => Current;

    public void Dispose() {}

    public bool MoveNext()
    {
        while (true)
        {
            while (current != null)
            {
                stack.Push(current);
                stack.Push(current);
                current = current.Left;
            }

            if (stack.Count == 0)
            {
                return false;
            }

            current = stack.Pop();

            if (stack.Count > 0 && stack.Peek() == current)
            {
                current = current.Right;
            }
            else
            {
                Current = current.Value;
                current = null;
                return true;
            }
        }
    }

    public void Previous()
    {
        if (current == null)
            return;
        if (current.Left != null)
        {
            current = current.Left;
            while (current.Right != null)
            {
                current = current.Right;
            }
        }
        else
        {
            while (current != null && current.Left == null)
            {
                current = current.Left;
            }
            if (current != null)
            {
                current = current.Left;
                while (current.Right != null)
                {
                    current = current.Right;
                }
            }
        }
    }

    public void Reset()
    {
        throw new Exception();
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

        BinaryTree<int> preOrderTree = new BinaryTree<int>(root, (r) => new PreOrderIterator<int>(r));
        Console.WriteLine("Прямой обход дерева:");
        foreach (var nodeValue in preOrderTree)
        {
            Console.Write(nodeValue + " ");
        }
        Console.WriteLine();

        BinaryTree<int> postOrderTree = new BinaryTree<int>(root, (r) => new PostOrderIterator<int>(r));
        Console.WriteLine("Обратный обход дерева:");
        foreach (var nodeValue in postOrderTree)
        {
            Console.Write(nodeValue + " ");
        }
        Console.WriteLine();

        var inOrderTraversalValues = BinaryTreeExtensions.LambdaInOrderTraversal(root);
        Console.WriteLine("Прямой обход дерева:");
        foreach (var nodeValue in inOrderTraversalValues)
        {
            Console.Write(nodeValue + " ");
        }

        Console.ReadKey();
    }
}
