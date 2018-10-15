# a-star-algorithm-unity
Creating the A* algorithm manually in Unity

# Pseudocode
```
OPEN // Set of nodes to be evaluated
CLOSED // Set of nodes already evaluated
add the start node to OPEN
 
loop
    current = node in OPEN with the lowest fCost
    remove current from OPEN
    add current to CLOSED
 
    if current is the target node // Path found
        return
 
    foreach neighbour of the current node
        if neighbour is not traversable or neighbour is in CLOSED
            skip to the next neighbour
 
            if new path to neighbour is shorter OR neighbour is not in OPEN
                set fCost of neighbour
                set parent of neighbour to current
                if neighbour is not in OPEN
                    add neighbour to OPEN
```
