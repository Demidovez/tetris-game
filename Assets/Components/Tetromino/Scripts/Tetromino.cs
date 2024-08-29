using UnityEngine;
using UnityEngine.Tilemaps;

namespace TetrominoSpace
{
    public enum TetrominoEnum
    {
        I,
        O,
        T,
        J,
        L,
        S,
        Z
    }

    [System.Serializable]
    public struct Tetromino
    {
        public TetrominoEnum TetrominoSymbol;
        public Tile Tile;
        public Vector2Int[] Cells;

        public void Initialize()
        {
            Cells = ShapesConfig.Cells[TetrominoSymbol];
        }
    }
}