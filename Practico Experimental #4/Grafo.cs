using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class Graph
{
    // simple undirected graph using adjacency list
    private Dictionary<string, HashSet<string>> adj = new Dictionary<string, HashSet<string>>();

    public void AddEdge(string u, string v)
    {
        if (!adj.ContainsKey(u)) adj[u] = new HashSet<string>();
        if (!adj.ContainsKey(v)) adj[v] = new HashSet<string>();
        adj[u].Add(v);
        adj[v].Add(u);
    }

    public IEnumerable<string> Nodes() => adj.Keys;
    public IEnumerable<(string, string)> Edges()
    {
        var seen = new HashSet<string>();
        foreach (var u in adj.Keys)
            foreach (var v in adj[u])
            {
                string key = string.Compare(u, v) <= 0 ? $"{u}|{v}" : $"{v}|{u}";
                if (!seen.Contains(key))
                {
                    seen.Add(key);
                    yield return (u, v);
                }
            }
    }

    public void PrintSummary()
    {
        Console.WriteLine("Nodos:");
        foreach (var n in Nodes()) Console.WriteLine(" - " + n);
        Console.WriteLine("Aristas:");
        foreach (var e in Edges()) Console.WriteLine($" - {e.Item1} -- {e.Item2}");
    }

    public Dictionary<string,int> DegreeCentrality()
    {
        var res = new Dictionary<string,int>();
        foreach (var k in adj.Keys) res[k] = adj[k].Count;
        return res;
    }

    public void ExportDot(string path)
    {
        var sb = new StringBuilder();
        sb.AppendLine("graph G {");
        foreach (var e in Edges())
            sb.AppendLine($"  \"{e.Item1}\" -- \"{e.Item2}\";");
        sb.AppendLine("}");
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
        File.WriteAllText(path, sb.ToString());
    }

    public static Graph LoadFromFile(string path)
    {
        // expected format: each line "A B" meaning edge A-B (whitespace separated)
        var g = new Graph();
        foreach (var l in File.ReadAllLines(path))
        {
            var t = l.Trim();
            if (string.IsNullOrWhiteSpace(t) || t.StartsWith("#")) continue;
            var parts = t.Split(new[]{' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2) g.AddEdge(parts[0], parts[1]);
        }
        return g;
    }

    public string FirstNode()
    {
        foreach (var k in adj.Keys) return k;
        return null;
    }

    public List<string> BFS(string start)
    {
        var res = new List<string>();
        if (start == null) return res;
        var q = new Queue<string>();
        var seen = new HashSet<string>();
        q.Enqueue(start);
        seen.Add(start);
        while (q.Count>0)
        {
            var u = q.Dequeue();
            res.Add(u);
            foreach (var v in adj[u])
                if (!seen.Contains(v))
                {
                    seen.Add(v);
                    q.Enqueue(v);
                }
        }
        return res;
    }
}
