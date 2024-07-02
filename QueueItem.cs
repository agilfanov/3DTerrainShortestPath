using System;
using System.Collections;
using System.Collections.Generic;

public class QueueItem : IComparable<QueueItem> {
    
    // class of a queue item. A queue item has a distance from the
    // specified start node in the pathfinding algorithm and the node is to what node that distance is
    public int node;
    public int distance;

    private int code;

    private static int counter = 0;
    public QueueItem(int distance, int node) {
        this.node = node;
        this.distance = distance;
        code = counter++;
    }
    
    public override string ToString() {
        return $"( node: {node}  distance:  {distance})";
    }
    
    // comparator, so that priority queue can sort the nodes in terms of min distance to start
    public int CompareTo(QueueItem other) {
        int diff  = distance - other.distance;

        if (diff == 0) {
            return code - other.code;
        }

        return diff;
    }
}