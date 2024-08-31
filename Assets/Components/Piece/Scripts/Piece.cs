using BoardSpace;
using GhostSpace;
using TetrominoSpace;
using UnityEngine;

namespace PieceSpace
{
    delegate int RoundFunc(float number);
    
    public class Piece : MonoBehaviour
    {
        public Ghost Ghost;
        public Tetromino Tetromino { get; private set; }
        public Vector3Int Position { get; private set; }
        public Vector3Int[] Cells { get; private set; }

        private int _rotationIndex;
        private const float _moveDelay = 0.1f;
        private const float _lockDelay = 0.5f;
        private const float _stepDelay = 1f;
        private float _moveTime;
        private float _lockTime;
        private float _stepTime;

        private void Update()
        {
            Board.Instance.Clear(this);

            _lockTime += Time.deltaTime;
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Rotate(-1);
            } else if (Input.GetKeyDown(KeyCode.E))
            {
                Rotate(1);
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HardDrop();
            }
            
            if (Time.time > _moveTime)
            {
                HandleMoveInputs();
            }

            if (Time.time > _stepTime)
            {
                Step();
            }

            Board.Instance.Set(this);
        }

        public void Initialize(Vector3Int position, Tetromino tetromino)
        {
            Ghost.TrackingPiece = this;
            
            Position = position;
            Tetromino = tetromino;
            _rotationIndex = 0;
            _stepTime = Time.time + _stepDelay;
            _moveTime = Time.time + _moveDelay;
            _lockTime = 0f;

            Cells ??= new Vector3Int[Tetromino.Cells.Length];

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
                _moveTime = Time.time + _moveDelay;
                _lockTime = 0f;
            }

            return isValidate;
        }

        private void HardDrop()
        {
            while (Move(Vector2Int.down)) {}
            
            Lock();
        }

        private void Rotate(int direction)
        {
            int originalRotationIndex = _rotationIndex;
            
            _rotationIndex += Wrap(_rotationIndex + direction, 0, 4);

            ApplyRotationMatrix(direction);

            if (!TestWallKicks(_rotationIndex, direction))
            {
                _rotationIndex = originalRotationIndex;
                ApplyRotationMatrix(-direction);
            }
        }

        private bool TestWallKicks(int rotationIndex, int rotationDirection)
        {
            int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

            for (int i = 0; i < Tetromino.WallKicks.GetLength(1); i++)
            {
                Vector2Int translation = Tetromino.WallKicks[wallKickIndex, i];

                if (Move(translation))
                {
                    return true;
                }
            }

            return false;
        }

        private int GetWallKickIndex(int rotationIndex, int rotationDirection)
        {
            int wallKickIndex = rotationIndex * 2;

            if (rotationDirection < 0)
            {
                wallKickIndex++;
            }

            return Wrap(wallKickIndex, 0, Tetromino.WallKicks.GetLength(0));
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

        private void HandleMoveInputs()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (Move(Vector2Int.down))
                {
                    _stepTime = Time.time + _stepDelay;
                }
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(Vector2Int.left);
            } else if (Input.GetKeyDown(KeyCode.D))
            {
                Move(Vector2Int.right);
            }
        }

        private void Step()
        {
            _stepTime = Time.time + _stepDelay;

            Move(Vector2Int.down);

            if (_lockTime > _lockDelay)
            {
                Lock();
            }
        }

        private void Lock()
        {
            Board.Instance.Set(this);
            Board.Instance.ClearLines();
            Board.Instance.SpawnPiece();
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
