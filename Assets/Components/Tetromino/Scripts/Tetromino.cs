using UnityEngine;
using UnityEngine.Tilemaps;

namespace TetrominoSpace
{
    public enum TetrominoEnum
    {
        I,
        J,
        L,
        O,
        S,
        T,
        Z
    }

    [System.Serializable]
    public struct Tetromino
    {
        public TetrominoEnum TetrominoSymbol;
        public Tile Tile;
        public Vector2Int[] Cells;
        public Vector2Int[,] WallKicks { get; private set; }

        public void Initialize()
        {
            Cells = ShapesConfig.Cells[TetrominoSymbol];
            WallKicks = ShapesConfig.WallKicks[TetrominoSymbol];
        }
    }
}