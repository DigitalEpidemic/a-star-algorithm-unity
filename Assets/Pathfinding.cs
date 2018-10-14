using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public Transform seeker, target;

    AStarGrid grid;

    void Awake() {
        grid = GetComponent<AStarGrid>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            FindPath(seeker.position, target.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos) {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        //List<Node> openSet = new List<Node>(); // Set of nodes to be evaluated
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // Set of nodes to be evaluated
        HashSet<Node> closedSet = new HashSet<Node>(); // Set of nodes already evaluated
        openSet.Add(startNode);

        while (openSet.Count > 0) { // Loop until empty
            Node node = openSet.RemoveFirst(); // Optimized (3ms)
            //Node node = openSet[0];
            //for (int i = 1; i < openSet.Count; i++) { // Unoptimized for now (15ms)
            //    if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
            //        if (openSet[i].hCost < node.hCost) {
            //            node = openSet[i];
            //        }
            //    }
            //}
            //openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode) { // Found path
                sw.Stop();
                print("Path found! " + sw.ElapsedMilliseconds + " ms");
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(node)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                int newMovementCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    } else {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
    } // FindPath

    void RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    // Diagonal Shortcut Method
    int GetDistance(Node nodeA, Node nodeB) {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY) {
            return 14 * distY + 10 * (distX - distY);
        } else {
            return 14 * distX + 10 * (distY - distX);
        }
    }

} // Pathfinding
