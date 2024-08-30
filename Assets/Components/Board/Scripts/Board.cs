using System;
using PieceSpace;
using TetrominoSpace;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace BoardSpace
{
    public class Board : MonoBehaviour
    {
        public static Board Instance;
        public Vector3Int SpawnPosition;
        public Tetromino[] Tetrominoes;
        public Vector2Int BoardSize = new(10, 20);

        private Tilemap _tilemap;
        private Piece _activePiece;
        
        private RectInt Bounds
        {
            get
            {
                Vector2Int position = new Vector2Int(-BoardSize.x / 2, -BoardSize.y / 2);

                return new RectInt(position, BoardSize);
            }
        }
        
        private void Awake()
        {
            Instance = this;
            _tilemap = GetComponentInChildren<Tilemap>();
            _activePiece = GetComponentInChildren<Piece>();

            for (int i = 0; i < Tetrominoes.Length; i++)
            {
                Tetrominoes[i].Initialize();
            }
        }

        private void Start()
        {
            SpawnPiece();
        }

        public void SpawnPiece()
        {
            Tetromino tetromino = Tetrominoes[Random.Range(0, Tetrominoes.Length)];
            
            _activePiece.Initialize(SpawnPosition, tetromino);

            if (IsValidatePosition(_activePiece, SpawnPosition))
            {
                Set(_activePiece);
            }
            else
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            _tilemap.ClearAllTiles();
        }

        public void Set(Piece piece)
        {
            foreach (var tilePosition in piece.Cells)
            {
                _tilemap.SetTile(tilePosition + piece.Position, piece.Tetromino.Tile);
            }
        }
        
        public void Clear(Piece piece)
        {
            foreach (var tilePosition in piece.Cells)
            {
                _tilemap.SetTile(tilePosition + piece.Position, null);
            }
        }
        
        public bool IsValidatePosition(Piece piece, Vector3Int position)
        {
            foreach (var cell in piece.Cells)
            {
                Vector3Int tilePosition = cell + position;
                
                if (!Bounds.Contains((Vector2Int) tilePosition))
                {
                    return false;
                }
                
                if (_tilemap.HasTile(tilePosition))
                {
                    return false;
                }
            }

            return true;
        }

        public void ClearLines()
        {
            RectInt bounds = Bounds;

            int row = bounds.yMax;

            while (row < bounds.yMax)
            {
                if (IsLineFull(row))
                {
                    LineClear(row);
                }
                else
                {
                    row++;
                }
            }
        }

        private void LineClear(int row)
        {
            RectInt bounds = Bounds;

            for (int col = bounds.xMin; col < bounds.xMin; col++) // TODO: наверное ошибка xMin
            {
                Vector3Int position = new Vector3Int(col, row, 0);
                _tilemap.SetTile(position, null);
            }

            while (row < bounds.yMax)
            {
                for (int col = bounds.xMin; col < bounds.xMax; col++)
                {
                    Vector3Int position = new Vector3Int(col, row + 1, 0);
                    TileBase above = _tilemap.GetTile(position);
                    
                    position = new Vector3Int(col, row, 0);
                    _tilemap.SetTile(position, above);
                }

                row++;
            }
        }

        private bool IsLineFull(int row)
        {
            RectInt bounds = Bounds;

            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 0);

                if (!_tilemap.HasTile(position))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
