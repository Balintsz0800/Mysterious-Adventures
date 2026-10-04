using System;
using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.Tilemaps;
 using Random = UnityEngine.Random;

 public class NPCMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stoppingDistance = 0.05f;

    [Header("PathFinding")] 
    [SerializeField] private Tilemap walkableTile;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float oblstacCheckRadius = 0.5f;
    [SerializeField] private int maxPathInterations = 600;
    
    [Header("Wandering")]
    [SerializeField] private bool wanderOnStart = true;
    [SerializeField] private float wanderRadius = 5f;
    [SerializeField] private float minWanderDis = 1.5f;
    [SerializeField] private float maxWanderDis = 5f;
    [SerializeField] private float minIdle = 1f;
    [SerializeField] private float maxIdle = 4f;
    [SerializeField] private int wanderAttempts = 10;
    
    private Rigidbody2D rb;
    private Animation NPCanim;
    
    private Transform target;
    private Vector2 targetPos;

    private bool hasTarget;
    private bool isWandering;
    
    private List<Vector2> path = new List<Vector2>();
    private int pathIndex;
    
    private Coroutine idleCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        NPCanim = GetComponent<Animation>();
    }

    private void Start()
    {
        if (wanderOnStart)
        {
            StartWandering();
        }
    }

    private void Update()
    {
        if (rb == null || NPCanim == null)
        {
            return;
        }

        if (target != null)
        {
            targetPos = target.position;
        }

        if (!hasTarget)
        {
            StopMovement();

            if (isWandering && idleCoroutine == null)
            {
                StartIdle();
            }
            return;
        }

        if (target != null && Vector2.Distance(rb.position, targetPos) <= stoppingDistance)
        {
            ReachTarget();
            return;
        }

        if (path.Count == 0)
        {
            CreatePath();
            
            if (path.Count == 0)
            {
                StopMovement();

                if (isWandering)
                {
                    hasTarget = false;
                    StartIdle();
                }
            
                return;
            }
        }

        FollowPath();
    }

    public void GoTo(Transform newTarget)
    {
        if (newTarget == null)
        {
            Stop();
            return;
        }
        
        CancelIdle();
        
        isWandering = false;
        target = newTarget;
        targetPos = newTarget.position;
        hasTarget = true;

        CreatePath();
    }

    public void GoTo(Vector2 newPos)
    {
        CancelIdle();
        
        isWandering = false;
        target = null;
        targetPos = newPos;
        hasTarget = true;

        CreatePath();
    }

    public void Stop()
    {
        CancelIdle();
        
        isWandering = false;
        hasTarget = false;
        target = null;

        path.Clear();
        pathIndex = 0;

        StopMovement();
    }

    public void StartWandering()
    {
        CancelIdle();
        
        isWandering = true;
        hasTarget = false;
        target = null;
        
        path.Clear();
        pathIndex = 0;

        StartIdle();
    }

    private void FollowPath()
    {
        if (pathIndex >= path.Count)
        {
            ReachTarget();
            return;
        }
        
        Vector2 nextPos = path[pathIndex];
        Vector2 direction = nextPos - rb.position;

        if (direction.magnitude <= .08f)
        {
            pathIndex++;

            if (pathIndex >= path.Count)
            {
                ReachTarget();
            }
            return;
        }
        
        direction.Normalize();
        
        rb.linearVelocity = direction * speed;
        
        NPCanim.horizontal = direction.x;
        NPCanim.vertical = direction.y;
        NPCanim.isMoving = true;
    }

    private void ReachTarget()
    {
        rb.linearVelocity = Vector2.zero;
        NPCanim.isMoving = false;

        path.Clear();
        pathIndex = 0;
        hasTarget = false;

        if (isWandering)
        {
            StartIdle();
        }
    }

    private void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        NPCanim.isMoving = false;
    }

    private void StartIdle()
    {
        if (!isWandering)
        {
            return;
        }
        
        CancelIdle();
        
        float idleTime = Random.Range(minIdle, maxIdle);
        idleCoroutine = StartCoroutine(IdleThenWander(idleTime));
    }

    private IEnumerator IdleThenWander(float idleTime)
    {
        StopMovement();
        
        yield return new  WaitForSeconds(idleTime);
        
        idleCoroutine = null;

        if (isWandering)
        {
            ChooseWanderTarget();
        }
    }

    private void ChooseWanderTarget()
    {
        if (walkableTile == null)
        {
            StartIdle();
            return;
        }

        for (int i = 0; i < wanderAttempts; i++)
        {
            Vector2 direction = Random.insideUnitCircle;

            if (direction.sqrMagnitude < 0.01f)
            {
                continue;
            }
            
            direction.Normalize();
            
            float distance = Random.Range(minWanderDis, Mathf.Min(maxWanderDis, wanderRadius));
            
            Vector2 newPosition = rb.position + direction * distance;

            if (!isWalkable(newPosition))
            {
                continue;
            }
            
            List<Vector2> newPath = FindPath(rb.position, newPosition);

            if (newPath == null || newPath.Count == 0)
            {
                continue;
            }

            target = null;
            targetPos = newPosition;
            path = newPath;
            pathIndex = 0;
            hasTarget = true;
            
            return;
        }
        
        StartIdle();
    }

    private void CreatePath()
    {
        if (walkableTile == null)
        {
            return;
        }

        path.Clear();
        pathIndex = 0;

        Vector2 startPos = rb.position;
        Vector2 destination = targetPos;

        if (target != null)
        {
            destination = target.position;
        }

        if (Vector2.Distance(startPos, destination) <= stoppingDistance)
        {
            ReachTarget();
            return;
        }
        
        List<Vector2> newPath = FindPath(startPos, destination);

        if (newPath == null || newPath.Count == 0)
        {
            hasTarget = false;
            return;
        }
        
        path = newPath;
    }

    private List<Vector2> FindPath(Vector2 startPos, Vector2 destination)
    {
        Vector3Int startCell = walkableTile.WorldToCell(startPos);
        Vector3Int targetCell = walkableTile.WorldToCell(destination);

        if (!isWalkable(targetCell))
        {
            targetCell = FindNearbyWalkableCell(targetCell);

            if (!isValidCell(targetCell))
            {
                return null;
            }
        }

        List<PathNode> openNodes = new List<PathNode>();
        HashSet<Vector3Int> closedCells = new HashSet<Vector3Int>();
        
        PathNode startNode = new PathNode(startCell, null, 0f, GetDistance(startCell, targetCell));
        
        openNodes.Add(startNode);
        
        int iterations = 0;
        
        while (openNodes.Count > 0)
        {
            iterations++;

            if (iterations > maxPathInterations)
            {
                return null;
            }
            
            PathNode currentNode = GetBestNode(openNodes);
            
            openNodes.Remove(currentNode);
            closedCells.Add(currentNode.cell);

            if (currentNode.cell == targetCell)
            {
                return CreatePath(currentNode);
            }

            foreach (Vector3Int neighbour in GetNeighbours(currentNode.cell))
            {
                if (closedCells.Contains(neighbour))
                {
                    continue;
                }

                if (!isWalkable(neighbour))
                {
                    continue;
                }

                if (isDiagonal(currentNode.cell, neighbour) && !CanMoveDiagonally(currentNode.cell, neighbour))
                {
                    continue;
                }
                
                float newCost = currentNode.gCost + GetDistance(currentNode.cell,  neighbour);
                
                PathNode existingNode = openNodes.Find(node => node.cell == neighbour);

                if (existingNode == null)
                {
                    PathNode newNode = new PathNode(neighbour, currentNode, newCost, GetDistance(neighbour, targetCell));
                    
                    openNodes.Add(newNode);
                }
                else if (newCost < existingNode.gCost)
                {
                    existingNode.gCost = newCost;
                    existingNode.parent = currentNode;
                }
            }
        }
        return null;
    }

    private List<Vector2> CreatePath(PathNode targetNode)
    {
        List<Vector2> newPath = new List<Vector2>();
        PathNode currentNode = targetNode;

        while (currentNode != null)
        {
            newPath.Add(walkableTile.GetCellCenterWorld(currentNode.cell));

            currentNode = currentNode.parent;
        }
        
        newPath.Reverse();

        if (newPath.Count > 0)
        {
            newPath.RemoveAt(0);
        }
        
        return newPath;
    }

    private PathNode GetBestNode(List<PathNode> nodes)
    {
        PathNode bestNode = nodes[0];

        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].FCost < bestNode.FCost)
            {
                bestNode = nodes[i];
            }
        }
        
        return bestNode;
    }

    private float GetDistance(Vector3Int firstCell, Vector3Int secondCell)
    {
        int xDistance = Mathf.Abs(firstCell.x - secondCell.x);
        int yDistance = Mathf.Abs(firstCell.y - secondCell.y);

        if (xDistance > yDistance)
        {
            return 1.414f * yDistance  + (xDistance - yDistance);
        }
        
        return 1.414f * xDistance + (yDistance - xDistance);
    }

    private IEnumerable<Vector3Int> GetNeighbours(Vector3Int cell)
    {
        yield  return cell + Vector3Int.up;
        yield  return cell + Vector3Int.down;
        yield  return cell + Vector3Int.left;
        yield  return cell + Vector3Int.right;
        yield return cell + new Vector3Int(1, 1, 0);
        yield return cell + new Vector3Int(-1, 1, 0);
        yield return cell + new Vector3Int(1, -1, 0);
        yield return cell + new Vector3Int(-1, -1, 0);
    }

    private bool isDiagonal(Vector3Int firstCell, Vector3Int secondCell)
    {
        return firstCell.x != secondCell.x && firstCell.y != secondCell.y;
    }

    private bool CanMoveDiagonally(Vector3Int firstCell, Vector3Int secondCell)
    {
        Vector3Int horizontalCell = new Vector3Int(secondCell.x, firstCell.y, firstCell.z);
        
        Vector3Int verticalCell = new Vector3Int(firstCell.x, secondCell.y, firstCell.z);
        
        return isWalkable(horizontalCell) && isWalkable(verticalCell);
    }

    private bool isWalkable(Vector3Int cell)
    {
        if (!walkableTile.HasTile(cell))
        {
            return false;
        }

        Vector2 cellPos = walkableTile.GetCellCenterWorld(cell);

        Collider2D obstacle = Physics2D.OverlapCircle(cellPos, oblstacCheckRadius, obstacleLayer);
        
        return obstacle == null;
    }

    private bool isWalkable(Vector2 position)
    {
        Vector3Int cell = walkableTile.WorldToCell(position);
        return isWalkable(cell);
    }

    private Vector3Int FindNearbyWalkableCell(Vector3Int centerCell)
    {
        for (int radius = 1; radius <= 4; radius++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Vector3Int cell = centerCell + new Vector3Int(x, y, 0);

                    if (isWalkable(cell))
                    {
                        return cell;
                    }
                }
            }
        }
        
        return new Vector3Int(int.MaxValue, int.MinValue, 0);
    }

    private bool isValidCell(Vector3Int cell)
    {
        return cell.x != int.MinValue && cell.y != int.MinValue;
    }

    private void CancelIdle()
    {
        if (idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine);
            idleCoroutine = null;
        }
    }

    private void OnDisable()
    {
        CancelIdle();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private class PathNode
    {
        public Vector3Int cell;
        public PathNode parent;
        public float gCost;
        public float hCost;

        public float FCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public PathNode(Vector3Int cell, PathNode parent, float gCost, float hCost)
        {
            this.cell = cell;
            this.parent = parent;
            this.gCost = gCost;
            this.hCost = hCost;
        }
    }
}

