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
        public Tilemap Tilemap { get; private set; }
        public Piece ActivePiece { get; private set; }
        public Vector3Int SpawnPosition;
        public Tetromino[] Tetrominoes;

        private void Awake()
        {
            Instance = this;
            Tilemap = GetComponentInChildren<Tilemap>();
            ActivePiece = GetComponentInChildren<Piece>();

            for (int i = 0; i < Tetrominoes.Length; i++)
            {
                Tetrominoes[i].Initialize();
            }
        }

        private void Start()
        {
            SpawnPiece();
        }

        private void SpawnPiece()
        {
            Tetromino tetromino = Tetrominoes[Random.Range(0, Tetrominoes.Length)];
            
            ActivePiece.Initialize(SpawnPosition, tetromino);
            
            Set(ActivePiece);
        }

        public void Set(Piece piece)
        {
            foreach (var tilePosition in piece.Cells)
            {
                Tilemap.SetTile(tilePosition + piece.Position, piece.Tetromino.Tile);
            }
        }
    }
}
