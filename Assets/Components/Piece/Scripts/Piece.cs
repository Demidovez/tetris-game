using System;
using TetrominoSpace;
using UnityEngine;

namespace PieceSpace
{
    public class Piece : MonoBehaviour
    {
        public Tetromino Tetromino { get; private set; }
        public Vector3Int Position { get; private set; } // а нужно ли?
        public Vector3Int[] Cells { get; private set; } // а нужно ли?

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(Vector2Int.left);
            } else if (Input.GetKeyDown(KeyCode.D))
            {
                Move(Vector2Int.right);
            }
        }

        public void Initialize(Vector3Int position, Tetromino tetromino)
        {
            Position = position;
            Tetromino = tetromino;

            if (Cells == null)
            {
                Cells = new Vector3Int[Tetromino.Cells.Length];
            }

            for (int i = 0; i < Tetromino.Cells.Length; i++)
            {
                Cells[i] = (Vector3Int) Tetromino.Cells[i];
            }
        }

        private void Move(Vector2Int translation)
        {
            Vector3Int newPosition = Position;
            newPosition.x += translation.x;
            newPosition.y += translation.y;
        }
    }
}
