using System.Collections.Generic;

public class ConvertMatrix {
    
    // list contains costs of each terrain type
    private static readonly int[] costs = {2, 10000, 4, 6, 5, 1, 3, 3, 2, 2};

    public static List<List<Edge>> convert(int[,] terrain, float[,] height, bool canUseBoat) {
        // checks if can use boat and if so, cost of water goes down to 2
        if (canUseBoat) costs[1] = 2;


        var size = terrain.GetLength(0);
        
        var adj = new List<List<Edge>>();
        
        // loop goes through all points and then checks how many neighbors each point has and adds them to the adjacency list
        for (var i = 0; i < size * size; i++) {
            var hasLeft = i % size > 0;
            var hasRight = i % size < size - 1;
            var hasBottom = i > size - 1;
            var hasTop = i < size * size - size;

            var nodeEdges = new List<Edge>();

            
            
            if (hasLeft) nodeEdges.Add(createNeighborEdge(i - 1, terrain, size, height, i));
            if (hasRight) nodeEdges.Add(createNeighborEdge(i + 1, terrain, size, height, i));
            if (hasBottom) nodeEdges.Add(createNeighborEdge(i - size, terrain, size, height, i));
            if (hasTop) nodeEdges.Add(createNeighborEdge(i + size, terrain, size, height, i));

            adj.Add(nodeEdges);
        }

        costs[1] = 10000;
        return adj;
    }

    // creates a neighboring edge
    private static Edge createNeighborEdge(int node, int[,] terrain, int size, float[,] height, int nodeFrom) {
        var multiplier = 0;

        var heightTo = height[node % size, node / size];
        var HeightFrom = height[nodeFrom % size, nodeFrom / size];

        if (heightTo > HeightFrom)
            multiplier = 1;
        else
            multiplier = 0;

        var g = 1000 * (heightTo - HeightFrom) * (heightTo - HeightFrom) * multiplier;
        var f = (int) g;


        return new Edge(node, costs[terrain[node % size, node / size]] * (f + 1));
    }
}