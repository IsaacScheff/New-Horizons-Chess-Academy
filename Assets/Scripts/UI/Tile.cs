using UnityEngine;

public class Tile : MonoBehaviour {
    [SerializeField] private Color _light, _dark;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _possibleMove;
    public bool IsPossibleMove { get { return _possibleMove.activeSelf; } }
    [SerializeField] private SpriteRenderer _pieceRenderer; // Renderer for the piece sprite
    [SerializeField] private PieceSprites pieceSprites; // Scriptable object with all the piece sprites

    // Enum to represent the type of piece on the tile, including None for empty
    public enum PieceType {
        None,
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King,
        PawnBlack,
        RookBlack,
        KnightBlack,
        BishopBlack,
        QueenBlack,
        KingBlack
    }

    private PieceType _currentPiece = PieceType.None;
    public PieceType CurrentPiece { get { return _currentPiece; } }

    public void SetColor(bool isOffSet) {
        _spriteRenderer.color = isOffSet ? _light : _dark;
    }
    private void ResizePieceSprite() {
        // Set the local scale to 1/4 of the original size
        _pieceRenderer.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    }
    void OnMouseEnter() {
        _highlight.SetActive(true);
    }
    void OnMouseExit() {
        _highlight.SetActive(false);
    }
    void OnMouseDown() {
        BoardManager.Instance.TileClicked(name);
    }
    public void HighlightPossibleMove() {
        _possibleMove.SetActive(true);
    }
    public void ClearPossibleMove() {
        _possibleMove.SetActive(false);
    }

    public void SetPiece(PieceType newPiece) {
        _currentPiece = newPiece;
        UpdatePieceSprite();
        ResizePieceSprite();
    }

    private void UpdatePieceSprite() {
        switch (_currentPiece) {
            case PieceType.Pawn:
                _pieceRenderer.sprite = pieceSprites.pawnSprite;
                break;
            case PieceType.Knight:
                _pieceRenderer.sprite = pieceSprites.knightSprite;
                break;
            case PieceType.Bishop:
                _pieceRenderer.sprite = pieceSprites.bishopSprite;
                break;
            case PieceType.Rook:
                _pieceRenderer.sprite = pieceSprites.rookSprite;
                break;
            case PieceType.Queen:
                _pieceRenderer.sprite = pieceSprites.queenSprite;
                break;
            case PieceType.King:
                _pieceRenderer.sprite = pieceSprites.kingSprite;
                break;
            case PieceType.PawnBlack:
                _pieceRenderer.sprite = pieceSprites.pawnBlackSprite;
                break;
            case PieceType.KnightBlack:
                _pieceRenderer.sprite = pieceSprites.knightBlackSprite;
                break;
            case PieceType.BishopBlack:
                _pieceRenderer.sprite = pieceSprites.bishopBlackSprite;
                break;
            case PieceType.RookBlack:
                _pieceRenderer.sprite = pieceSprites.rookBlackSprite;
                break;
            case PieceType.QueenBlack:
                _pieceRenderer.sprite = pieceSprites.queenBlackSprite;
                break;
            case PieceType.KingBlack:
                _pieceRenderer.sprite = pieceSprites.kingBlackSprite;
                break;
            case PieceType.None:
                _pieceRenderer.sprite = null; // No sprite for empty tile
                break;
        }
    }
    public bool IsPieceBlack() {
        return _currentPiece.ToString().EndsWith("Black");
    }
    public bool IsPieceWhite() {
        return _currentPiece != PieceType.None && !IsPieceBlack();
    }
}

