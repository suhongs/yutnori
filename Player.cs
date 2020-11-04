using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Net.NetworkInformation;

namespace ConsoleApp1
{
    class Player
    {
        private int score = 0;          //최대 4점까지
        private int numPieces = 0;      //board에 현재 있는 말 수

        public Piece[] pieces = new Piece[4];

        public Player()         //Player 생성 시 말 4개 할당!
        {
            for (int i = 0; i < pieces.Length; i++)
            {
                pieces[i] = new Piece();
            }
        }

        public int FindAvailablePiece()
        {
            for (int i = 0; i < 4; i++)
            {
                if (pieces[i].GetLocation() == -1)        //사용 가능한 말의 인덱스 리턴
                    return i;
            }
            return -1;          //사용 가능한 말 없음
        }

        public int GetScore() { return score; }
        public void AddScore(int i) { score += i; }

        public int GetNumPiecesOnBoard() { return numPieces; }
        public void AddNumPieces(int i) { numPieces += i; }

        public void SubtractNumPieces(int i) { numPieces -= i; }
        public Boolean HasNoPiecesOmBoard() { return (numPieces == score); }
        public Boolean HasAllPiecesOnBoard() { return (numPieces == 4); }
        public Boolean HasWon() { return (score == 4); }
        public void Reset()
        {
            score = 0;
            numPieces = 0;
            for (int i = 0; i < 4; i++)
            {
                pieces[i].Reset();
            }
        }
    }
}

