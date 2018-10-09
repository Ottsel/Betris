using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private const float MaxSpeed = .1f;
    private const float Acceleration = .1f;

    private int[,] _board;

    private bool _isDead;
    public Text Debug;
    [HideInInspector] public GameObject Piece, NextPiece;
    public GameObject[] Pieces;


    [HideInInspector] public float Speed = 1;

    private void Start()
    {
        _board = new int[20, 12];
        Piece = Pieces[Random.Range(0, Pieces.Length)];
        SetPieceStart(Piece);
        Drop(Piece);
        NextPiece = Pieces[Random.Range(0, Pieces.Length)];
        StartCoroutine(Fall());
    }

    private void Update()
    {
        var debugText = string.Empty;
        Debug.text = string.Empty;
        var currentRow = 0;

        for (var index0 = 0; index0 < _board.GetLength(0); index0++)
        for (var index1 = 0; index1 < _board.GetLength(1); index1++)
        {
            var unit = _board[index0, index1];
            if (index0 != currentRow)
            {
                currentRow = index0;
                debugText += "\n";
            }

            switch (unit)
            {
                case 0:
                    debugText += "O,";
                    break;
                case 1:
                    debugText += "1,";
                    break;
                default:
                    debugText += "2,";
                    break;
            }
        }

        Debug.text = debugText;

        if (_isDead) return;

        if (Piece.CompareTag("Done"))
        {
            Drop(NextPiece);
            SetPieceStart(Piece);
            StartCoroutine(Fall());
            return;
        }

        if (Input.GetKeyDown(KeyCode.E)) Piece.transform.Rotate(Vector3.forward, -90);
        if (Input.GetKeyDown(KeyCode.Q)) Piece.transform.Rotate(Vector3.forward, 90);
        if (Input.GetKeyDown(KeyCode.A))
        {
            var newBoard = new int[20, 12];
            for (var index0 = 0; index0 < _board.GetLength(0); index0++)
            for (var index1 = 0; index1 < _board.GetLength(1); index1++)
            {

                for (var i0 = 0; i0 < _board.GetLength(0); i0++)
                for (var i1 = 0; i1 < _board.GetLength(1); i1++)
                {
                    if (_board[i0, i1] == 1 && (i1 == 0 || _board[i0, i1 - 1] == 2)) return;

                }

                if (_board[index0, index1] == 1)
                {
                    if (index1 == 0)
                    {
                        newBoard[index0, index1] = 1;
                    }
                    else
                    {
                        newBoard[index0, index1 - 1] = 1;
                    }
                }

                if (_board[index0, index1] != 2) continue;
                newBoard[index0, index1] = 2;
                if (index0 == 0)
                {
                    _isDead = true;
                }
            }


            _board = newBoard;
        }

        if (!Input.GetKeyDown(KeyCode.D)) return;
        {
            var newBoard = new int[20, 12];
            for (var index0 = 0; index0 < _board.GetLength(0); index0++)
            for (var index1 = 0; index1 < _board.GetLength(1); index1++)
            {
                
                for (var i0 = 0; i0 < _board.GetLength(0); i0++)
                for (var i1 = 0; i1 < _board.GetLength(1); i1++)
                {
                    if (_board[i0, i1] == 1 && (i1 == 11 || _board[i0,i1+1] == 2)) return;

                }
                
                if (_board[index0, index1] == 1)
                {
                    if (index1 == 11)
                    {
                        newBoard[index0, index1] = 1;
                    }
                    else
                    {
                        newBoard[index0, index1 + 1] = 1;
                    }
                }

                if (_board[index0, index1] != 2) continue;
                newBoard[index0, index1] = 2;
                if (index0 == 0)
                {
                    _isDead = true;
                }
            }

            _board = newBoard;
        }
    }

    private void SetPieceStart(GameObject piece)
    {
        switch (piece.name)
        {
            case "B":
                _board[0, 6] = 1;
                _board[0, 7] = 1;
                _board[1, 6] = 1;
                _board[1, 7] = 1;
                break;
            case "Brain":
                _board[0, 6] = 1;
                _board[1, 6] = 1;
                _board[2, 6] = 1;
                _board[3, 6] = 1;
                break;
        }
    }

    private void Drop(GameObject piece)
    {
        Piece = Instantiate(piece);
        Piece.name = piece.name;
    }

    private void LevelUp()
    {
        Speed -= Acceleration;
    }

    private IEnumerator Fall()
    {
        while (!_isDead && !Piece.CompareTag("Done"))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                yield return new WaitForSeconds(MaxSpeed);
            else
                yield return new WaitForSeconds(Speed);

            var newBoard = new int[20, 12];
            for (var index0 = 0; index0 < _board.GetLength(0); index0++)
            for (var index1 = 0; index1 < _board.GetLength(1); index1++)
            {
                if (_board[index0, index1] == 1)
                {
                    if (index0 + 1 == 19)
                    {
                        for (var i0 = 0; i0 < _board.GetLength(0); i0++)
                        for (var i1 = 0; i1 < _board.GetLength(1); i1++)
                        {
                            if (_board[i0, i1] == 1)
                            {
                                newBoard[i0 + 1, i1] = 2;
                            }
                        }

                        Piece.tag = "Done";
                    }
                    else if (_board[index0 + 1, index1] == 2)
                    {
                        for (var i0 = 0; i0 < _board.GetLength(0); i0++)
                        for (var i1 = 0; i1 < _board.GetLength(1); i1++)
                        {
                            if (_board[i0, i1] != 1) continue;
                            if (i0 == 0)
                            {
                                _isDead = true;
                            }

                            newBoard[i0, i1] = 2;
                        }

                        Piece.tag = "Done";
                    }
                    else if (!Piece.CompareTag("Done"))
                    {
                        newBoard[index0 + 1, index1] = 1;
                    }
                }

                if (_board[index0, index1] != 2) continue;
                newBoard[index0, index1] = 2;
                if (index0 == 0)
                {
                    _isDead = true;
                }
            }

            _board = newBoard;
        }
    }
}