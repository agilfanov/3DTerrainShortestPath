using System.Collections.Generic;

public class PathFinding {
    public static List<int> path(int startNode, int endNode, List<List<Edge>> adjacencyLists) {
        
        // main pathfinding algorithm
        // Based on djikstra's algorithm but modified to fit in with the 3d map and this code
        
        var nodes = adjacencyLists.Count;
        var queueOfNodes = new SortedSet<QueueItem>();
        var distances = new int[nodes];
        var processed = new bool[nodes];
        var parents = new int[nodes];
        var path = new List<int>();
        
        // initially all distances are set to infinity except for the disatnce to the start node
        for (var i = 0; i < nodes; i++) distances[i] = 1000000;

        distances[startNode] = 0;
        queueOfNodes.Add(new QueueItem(0, startNode));
        parents[startNode] = -1;

        
        // Finds smallest distance available to next node and explores it until either reaches end or finds a quicker path
        while (queueOfNodes.Count != 0) {
            var closest = queueOfNodes.Min;
            queueOfNodes.Remove(queueOfNodes.Min);
            var currentNode = closest.node;

            if (processed[currentNode]) continue;
            processed[currentNode] = true;

            for (var i = 0; i < adjacencyLists[currentNode].Count; i++) {
                var closestNode = adjacencyLists[currentNode][i].node;
                var distanceFromCurrentNodeToClosestNode = adjacencyLists[currentNode][i].distance;
                
                if (distances[currentNode] + distanceFromCurrentNodeToClosestNode < distances[closestNode]) {
                    distances[closestNode] = distances[currentNode] + distanceFromCurrentNodeToClosestNode;
                    parents[closestNode] = currentNode;
                    queueOfNodes.Add(new QueueItem(distances[closestNode], closestNode));
                }
            }
        }
        
        // generates the path it took and stores that in the path list
        var parent = parents[endNode];
        path.Add(endNode);
        path.Add(parent);

        while (true) {
            parent = parents[parent];
            if (parent == -1) break;
            path.Add(parent);
        }
        return path;
    }
}