using BoardSpace;
using PieceSpace;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GhostSpace
{
    public class Ghost : MonoBehaviour
    {
        public Tile GhostTile;
        internal Piece TrackingPiece;

        private Tilemap _tilemap;
        private Vector3Int[] _cells;
        private Vector3Int _position;

        private void Awake()
        {
            _tilemap = GetComponentInChildren<Tilemap>();
            _cells = new Vector3Int[4];
        }

        private void LateUpdate()
        {
            Clear();
            Copy();
            Drop();
            Set();
        }

        private void Clear()
        {
            foreach (var cell in _cells)
            {
                Vector3Int tilePosition = cell + _position;
                _tilemap.SetTile(tilePosition, null);
            }
        }

        private void Copy()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i] = TrackingPiece.Cells[i];
            }
        }

        private void Drop()
        {
            Vector3Int position = TrackingPiece.Position;

            int current = position.y;
            int bottom = -Board.Instance.BoardSize.y / 2 - 1;
            
            Board.Instance.Clear(TrackingPiece);

            for (int row = current; row >= bottom; row--)
            {
                position.y = row;

                if (Board.Instance.IsValidatePosition(TrackingPiece, position))
                {
                    _position = position;
                }
                else
                {
                    break; 
                }
            }
            
            Board.Instance.Set(TrackingPiece);
        }

        private void Set()
        {
            foreach (var cell in _cells)
            {
                Vector3Int tilePosition = cell + _position;
                _tilemap.SetTile(tilePosition, GhostTile);
            }
        }
    }
}