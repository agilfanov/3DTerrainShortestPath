using System;

public class Edge : IComparable<Edge> {
    
    // Edge is similiar to queue item, but now the distance isn't to the start node but to the node that the edge belongs to.
    // For example if node A connects to node B with distance x, that would mean A has edge (B, x).
    public int distance;
    public int node;

    public Edge(int node, int distance) {
        this.node = node;
        this.distance = distance;
    }

        
    public int CompareTo(Edge other) {
        return distance - other.distance;
    }

    public override string ToString() {
        return $"( node: {node}  distance:  {distance})";
    }
}