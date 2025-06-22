using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public enum Direction
{
    Up,     // +Y
    Down,   // -Y
    Left,   // -X
    Right,  // +X
    Front,  // +Z
    Back,    // -Z
}

public class MapLoader : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject chunkContainerPrefab;
    public TilePrefabEntry[] tilePrefabs;

    [Header("Path for Map json")]
    public string mapFileName = "Stage1";

    private Dictionary<int, GameObject> prefabDict;
    public static HashSet<Vector3Int> ChunksHashSet { get; private set; } = new();
    public static List<Direction> DirectionList { get; private set; } = new();

    void Start()
    {
        LoadPrefabDict();

        TextAsset json = Resources.Load<TextAsset>($"Maps/{mapFileName}");
        ChunkMap map = JsonUtility.FromJson<ChunkMap>(json.text);

        Direction prevDir = Direction.Back;    // All maps start from Front direction
        Direction lastHorizontalDir = Direction.Front; // Track last horizontal direction
        int x=0, y=0, z=0;
        int n = map.chunks.Length;
        int cnt = n;
        foreach (var chunk in map.chunks)
        {
            cnt--;
            BuildChunk(chunk, prevDir, lastHorizontalDir, x, y, z, cnt);
            prevDir = chunk.dir;
            if (IsChunkHorizontal(chunk.dir))
            {
                lastHorizontalDir = chunk.dir;
            }
            Vector3Int pos = new Vector3Int(x, y, z);
            ChunksHashSet.Add(pos);
            DirectionList.Add(chunk.dir);

            UpdatePosition(ref x, ref y, ref z, chunk.dir);
        }
    }
    void UpdatePosition(ref int x, ref int y, ref int z, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                y++;
                break;
            case Direction.Down:
                y--;
                break;
            case Direction.Left:
                x--;
                break;
            case Direction.Right:
                x++;
                break;
            case Direction.Front:
                z++;
                break;
            case Direction.Back:
                z--;
                break;
            default:
                throw new ArgumentException("Invalid direction: " + dir);
        }
    }
    void LoadPrefabDict()
    {
        prefabDict = new Dictionary<int, GameObject>();
        foreach (var entry in tilePrefabs)
        {
            prefabDict[entry.type] = entry.prefab;
        }
    }
    Vector3Int DirToVec(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return new Vector3Int(0, 1, 0);
            case Direction.Down:
                return new Vector3Int(0, -1, 0);
            case Direction.Left:
                return new Vector3Int(-1, 0, 0);
            case Direction.Right:
                return new Vector3Int(1, 0, 0);
            case Direction.Front:
                return new Vector3Int(0, 0, 1);
            case Direction.Back:
                return new Vector3Int(0, 0, -1);
            default:
                return Vector3Int.zero;
        }
    }

    Direction VecToDir(Vector3 vec)
    {
        if (vec == Vector3.up)     return Direction.Up;
        if (vec == Vector3.down)   return Direction.Down;
        if (vec == Vector3.left)   return Direction.Left;
        if (vec == Vector3.right)  return Direction.Right;
        if (vec == Vector3.forward)return Direction.Front;
        if (vec == Vector3.back)   return Direction.Back;

        throw new ArgumentException("Invalid direction vector: " + vec);
    }
    public static Direction GetOpposite(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return Direction.Down;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
            case Direction.Front: return Direction.Back;
            case Direction.Back: return Direction.Front;
            default: return dir;  // fallback
        }
    }

    bool ShouldSkipTile(int x, int y, int z, Direction prevDir, int cnt)
    {
        // Debug.Log("Direction");
        // Debug.Log(prevDir);
        if (z == 8 && cnt!= 0) return true;

        if (prevDir == Direction.Back && z == 0) return true;
        if (prevDir == Direction.Left && x == 0) return true;
        if (prevDir == Direction.Right && x == 8) return true;
        if (prevDir == Direction.Down && y == 0) return true;
        if (prevDir == Direction.Up && y == 8) return true;

        return false;
    }
    Quaternion GetChunkRotationFromDirection(Direction dir, Direction lastHorizontalDir)
    {
        switch (dir)
        {
            case Direction.Left: return Quaternion.Euler(0, -90, 0);
            case Direction.Right: return Quaternion.Euler(0, 90, 0);
            case Direction.Front: return Quaternion.identity; // No rotation needed
            case Direction.Back: return Quaternion.Euler(0, 180, 0);
            case Direction.Down: 
                switch (lastHorizontalDir)
                {
                    case Direction.Left: return Quaternion.Euler(90, -90, 0);
                    case Direction.Right: return Quaternion.Euler(90, 90, 0);
                    case Direction.Front: return Quaternion.Euler(90, 0, 0);
                    case Direction.Back: return Quaternion.Euler(90, 180, 0);
                    default: throw new ArgumentException("Invalid last horizontal direction: " + lastHorizontalDir);
                }
            default: throw new ArgumentException("Invalid direction: " + dir);
        }
    }
    bool IsChunkHorizontal(Direction dir)
    {
        return !(dir == Direction.Up || dir == Direction.Down);
    }

    void BuildChunk(ChunkData chunk, Direction prevDir, Direction lastHorizontalDir, int cx, int cy, int cz, int cnt)
    {
        // Debug.Log(prevDir);
        Vector3 chunkPos = new Vector3(cx, cy, cz);
        chunkPos *= 70f;

        Quaternion rotation = GetChunkRotationFromDirection(chunk.dir, lastHorizontalDir);
        
        GameObject container = Instantiate(chunkContainerPrefab, chunkPos, rotation, transform);
        container.name = $"Chunk_{cx}_{cy}_{cz}";

        if (chunk.tiles == null)
        {
            Debug.LogError("No tiles");
        }

        bool isCeiling = false;
        for (int i = 0; i < 729; i++)
        {
            int type = chunk.tiles[i];
            if (type == 0) continue;        // Air

            // Not used
            int y = i / 81;
            int z = i % 81 / 9;
            int x = i % 9;


            // int z = i / 81;
            // int y = 8 - (i % 81 / 9);
            // int x = i % 9;


            Vector3 result = -(Quaternion.Inverse(rotation) * DirToVec(prevDir));
            if (i == 0)
            {
                Debug.Log("result");
                Debug.Log(result);
            }
            if (ShouldSkipTile(x, y, z, VecToDir(result), cnt)) continue;
            
            if (type == 1 && y == 8 && IsChunkHorizontal(chunk.dir)) { isCeiling = true; }

            Vector3 localPos = new Vector3(x, y, z);
            localPos *= 10f;
            localPos -= new Vector3(40f, 40f, 40f);
            Vector3 worldPos = container.transform.TransformPoint(localPos);

            if (prefabDict.TryGetValue(type, out GameObject prefab))
            {
                Instantiate(prefab, worldPos, rotation, container.transform);
                if (isCeiling)
                {
                    prefabDict.TryGetValue(2, out prefab);
                    worldPos -= new Vector3(0, 10f, 0);
                    Instantiate(prefab, worldPos, rotation, container.transform);
                    isCeiling = false;
                }
            }
            else
            {
                Debug.LogWarning($"Missing prefab for type {type}");
            }
        }
    }


    // Quaternion GetRotationFromDirection(Direction indir, Direction outdir)
    // {
    //     Vector3 indirVec = DirToVec(indir);
    //     Vector3 outdirVec = DirToVec(outdir);

    //     // Step 1: rotate Vector3.forward → indirVec
    //     Quaternion toIndir = Quaternion.FromToRotation(Vector3.forward, indirVec);

    //     // Step 2: figure out how to rotate indirVec → outdirVec (in the rotated space)
    //     Vector3 outdirInLocal = Quaternion.Inverse(toIndir) * outdirVec;

    //     // Step 3: rotate forward → outdirInLocal
    //     Quaternion adjust = Quaternion.FromToRotation(Vector3.forward, outdirInLocal);

    //     // Final rotation = toIndir followed by adjust
    //     return toIndir * adjust;
    // }
    Direction GetDirectionFromRotation(Quaternion rot)
    {
        Vector3 dir = rot * Vector3.forward;

        float maxDot = float.NegativeInfinity;
        Direction bestMatch = Direction.Front;

        foreach (Direction candidate in System.Enum.GetValues(typeof(Direction)))
        {
            Vector3 candidateVec = DirToVec(candidate);
            float dot = Vector3.Dot(dir.normalized, candidateVec.normalized);
            if (dot > maxDot)
            {
                maxDot = dot;
                bestMatch = candidate;
            }
        }

        return bestMatch;
    }
}

[System.Serializable]
public class TilePrefabEntry
{
    public int type;
    public GameObject prefab;
}

[System.Serializable]
public class ChunkMap
{
    public ChunkData[] chunks;
}

[System.Serializable]
public class ChunkData
{
    public Direction dir;
    public int[] tiles;
}