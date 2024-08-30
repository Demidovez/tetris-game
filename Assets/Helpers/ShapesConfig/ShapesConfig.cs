

using System.Collections.Generic;
using TetrominoSpace;
using UnityEngine;

public static class ShapesConfig
{
    private static readonly float cos = Mathf.Cos(Mathf.PI / 2f);
    private static readonly float sin = Mathf.Sin(Mathf.PI / 2f);
    public static readonly float[] RotationMatrix = { cos, sin, -sin, cos };

    public static readonly Dictionary<TetrominoEnum, Vector2Int[]> Cells = new()
    {
        { TetrominoEnum.I, new Vector2Int[] { new(-1, 1), new( 0, 1), new( 1, 1), new(2, 1) } },
        { TetrominoEnum.J, new Vector2Int[] { new(-1, 1), new(-1, 0), new( 0, 0), new(1, 0) } },
        { TetrominoEnum.L, new Vector2Int[] { new( 1, 1), new(-1, 0), new( 0, 0), new(1, 0) } },
        { TetrominoEnum.O, new Vector2Int[] { new( 0, 1), new( 1, 1), new( 0, 0), new(1, 0) } },
        { TetrominoEnum.S, new Vector2Int[] { new( 0, 1), new( 1, 1), new(-1, 0), new(0, 0) } },
        { TetrominoEnum.T, new Vector2Int[] { new( 0, 1), new(-1, 0), new( 0, 0), new(1, 0) } },
        { TetrominoEnum.Z, new Vector2Int[] { new(-1, 1), new( 0, 1), new( 0, 0), new(1, 0) } }
    };

    private static readonly Vector2Int[,] WallKicksI = {
        { new(0, 0), new(-2, 0), new( 1, 0), new(-2, -1), new( 1, 2) },
        { new(0, 0), new( 2, 0), new(-1, 0), new( 2,  1), new(-1,-2) },
        { new(0, 0), new(-1, 0), new( 2, 0), new(-1,  2), new( 2,-1) },
        { new(0, 0), new( 1, 0), new(-2, 0), new( 1, -2), new(-2, 1) },
        { new(0, 0), new( 2, 0), new(-1, 0), new( 2,  1), new(-1,-2) },
        { new(0, 0), new(-2, 0), new( 1, 0), new(-2, -1), new( 1, 2) },
        { new(0, 0), new( 1, 0), new(-2, 0), new( 1, -2), new(-2, 1) },
        { new(0, 0), new(-1, 0), new( 2, 0), new(-1,  2), new( 2,-1) }
    };

    private static readonly Vector2Int[,] WallKicksJLOSTZ =
    {
        { new(0, 0), new(-1, 0), new(-1,  1), new(0, -2), new(-1, -2) },
        { new(0, 0), new( 1, 0), new( 1, -1), new(0,  2), new( 1,  2) },
        { new(0, 0), new( 1, 0), new( 1, -1), new(0,  2), new( 1,  2) },
        { new(0, 0), new(-1, 0), new(-1,  1), new(0, -2), new(-1, -2) },
        { new(0, 0), new( 1, 0), new( 1,  1), new(0, -2), new( 1, -2) },
        { new(0, 0), new(-1, 0), new(-1, -1), new(0,  2), new(-1,  2) },
        { new(0, 0), new(-1, 0), new(-1, -1), new(0,  2), new(-1,  2) },
        { new(0, 0), new( 1, 0), new( 1,  1), new(0, -2), new( 1, -2) }
    };

    public static readonly Dictionary<TetrominoEnum, Vector2Int[,]> WallKicks = new()
    {
        { TetrominoEnum.I, WallKicksI },
        { TetrominoEnum.J, WallKicksJLOSTZ },
        { TetrominoEnum.L, WallKicksJLOSTZ },
        { TetrominoEnum.O, WallKicksJLOSTZ },
        { TetrominoEnum.S, WallKicksJLOSTZ },
        { TetrominoEnum.T, WallKicksJLOSTZ },
        { TetrominoEnum.Z, WallKicksJLOSTZ }
    };
}
