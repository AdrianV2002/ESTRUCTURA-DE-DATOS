using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class Tree
{
    // Simple generic rooted tree represented with adjacency (parents -> children)
    private Dictionary<string, List<string>> children = new Dictionary<string, List<string>>();
    private string root = null;

    public void AddEdge(string parent, string child)
    {
        if (!children.ContainsKey(parent)) children[parent] = new List<string>();
        if (!children.ContainsKey(child)) children[child] = new List<string>(); // ensure exists
        children[parent].Add(child);
        if (root == null) root = parent;
    }

    public void PrintSummary()
    {
        Console.WriteLine("Nodos y sus hijos:");
        foreach (var kv in children)
        {
            Console.WriteLine($" - {kv.Key} : [{string.Join(", ", kv.Value)}]");
        }
        Console.WriteLine($"Root (posible): {root}");
    }

    public void ExportDot(string path)
    {
        var sb = new StringBuilder();
        sb.AppendLine("digraph Tree {");
        foreach (var kv in children)
            foreach (var c in kv.Value)
                sb.AppendLine($"  \"{kv.Key}\" -> \"{c}\";");
        sb.AppendLine("}");
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
        File.WriteAllText(path, sb.ToString());
    }

    public static Tree LoadFromFile(string path)
    {
        // expected format: each line "Parent Child"
        var t = new Tree();
        var indeg = new Dictionary<string,int>();
        foreach (var l in File.ReadAllLines(path))
        {
            var line = l.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
            var parts = line.Split(new[]{' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                t.AddEdge(parts[0], parts[1]);
                if (!indeg.ContainsKey(parts[0])) indeg[parts[0]] = 0;
                if (!indeg.ContainsKey(parts[1])) indeg[parts[1]] = 0;
                indeg[parts[1]]++;
            }
        }
        // attempt to detect root (node with indeg 0)
        foreach (var kv in indeg)
            if (kv.Value == 0) t.root = kv.Key;
        return t;
    }

    public List<string> PreOrder()
    {
        var res = new List<string>();
        void dfs(string node)
        {
            if (node == null) return;
            res.Add(node);
            foreach (var c in children[node]) dfs(c);
        }
        dfs(root);
        return res;
    }

    public List<string> InOrder()
    {
        // Note: In-order is only well-defined for binary trees.
        // We'll implement a naive traversal for general trees: visit first child (inorder), then node, then rest.
        var res = new List<string>();
        void dfs(string node)
        {
            if (node == null) return;
            var ch = children[node];
            if (ch.Count > 0) dfs(ch[0]);
            res.Add(node);
            for (int i = 1; i < ch.Count; i++) dfs(ch[i]);
        }
        dfs(root);
        return res;
    }

    public List<string> PostOrder()
    {
        var res = new List<string>();
        void dfs(string node)
        {
            if (node == null) return;
            foreach (var c in children[node]) dfs(c);
            res.Add(node);
        }
        dfs(root);
        return res;
    }
}
