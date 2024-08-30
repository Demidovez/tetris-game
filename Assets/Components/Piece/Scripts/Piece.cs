using System;
using BoardSpace;
using TetrominoSpace;
using UnityEngine;

namespace PieceSpace
{
    public class Piece : MonoBehaviour
    {
        public Tetromino Tetromino { get; private set; }
        public Vector3Int Position { get; private set; }
        public Vector3Int[] Cells { get; private set; }

        private int _rotationIndex;
        delegate int RoundFunc(float number);

        private void Update()
        {
            Board.Instance.Clear(this);
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Rotate(-1);
            } else if (Input.GetKeyDown(KeyCode.E))
            {
                Rotate(1);
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(Vector2Int.left);
            } else if (Input.GetKeyDown(KeyCode.D))
            {
                Move(Vector2Int.right);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Move(Vector2Int.down);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                HardDrop();
            }

            Board.Instance.Set(this);
        }

        public void Initialize(Vector3Int position, Tetromino tetromino)
        {
            Position = position;
            Tetromino = tetromino;
            _rotationIndex = 0;

            if (Cells == null)
            {
                Cells = new Vector3Int[Tetromino.Cells.Length];
            }

            for (int i = 0; i < Tetromino.Cells.Length; i++)
            {
                Cells[i] = (Vector3Int) Tetromino.Cells[i];
            }
        }

        private bool Move(Vector2Int translation)
        {
            Vector3Int newPosition = Position;
            newPosition.x += translation.x;
            newPosition.y += translation.y;
            
            bool isValidate = Board.Instance.IsValidatePosition(this, newPosition);

            if (isValidate)
            {
                Position = newPosition;
            }

            return isValidate;
        }

        private void HardDrop()
        {
            while (Move(Vector2Int.down)) {}
        }

        private void Rotate(int direction)
        {
            int originalRotationIndex = _rotationIndex;
            
            _rotationIndex += Wrap(_rotationIndex + direction, 0, 4);

            ApplyRotationMatrix(direction);
        }

        private void ApplyRotationMatrix(int direction)
        {
            float[] matrix = ShapesConfig.RotationMatrix;
            
            for (int i = 0; i < Tetromino.Cells.Length; i++)
            {
                Vector3 cell = Cells[i];

                RoundFunc roundFunc = Mathf.RoundToInt;
                
                switch (Tetromino.TetrominoSymbol)
                {
                    case TetrominoEnum.I:
                    case TetrominoEnum.O:
                        cell.x -= 0.5f;
                        cell.y -= 0.5f;
                        roundFunc = Mathf.CeilToInt;
                        break;
                }
                
                int x = roundFunc((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                int y = roundFunc((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));

                Cells[i] = new Vector3Int(x, y, 0);
            }
        }

        private int Wrap(int input, int min, int max)
        {
            if (input < min)
            {
                return max - (min - input) % (max - min);
            }

            return min + (input - min) % (max - min);
        }
    }
}
