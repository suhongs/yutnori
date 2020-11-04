using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Net.NetworkInformation;

namespace ConsoleApp1
{
    class Piece
    {
        private int location = -1;      //board에서의 현재 위치, -1은 board 밖에 있다는걸 의미
        private int value = 1;          //현재 쌓여있는 piece(말)의 개수

        public int[,] CalculateMoveset(int[] moves)
        {
            List<int> moveList = new List<int>();           //[destination, rollAmout] 의 배열

            foreach (int i in moves)
            {
                //location에서 시작해 i번째 roll로 piece가 갈 수 있는 위치를 모두 moveList에 추가
                moveList.AddRange(Board.CalculateLocation(i, location));
            }

            int[] temp = moveList.ToArray();
            int[,] moveListArr = new int[moveList.Count / 2, 2];
            int tempidx = 0;

            for (int i = 0; i < moveList.Count / 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    moveListArr[i, j] = temp[tempidx++];
                }
            }
            return moveListArr;
        }

        public int GetLocation() { return location; }
        public void SetLocation(int i) { location = i; }

        public int GetValue() { return value; }
        public void ResetValue() { value = 1; }

        public void AddValue(int v) { value += v; }
        public void Reset()         //말을 시작 위치로 리셋
        {
            location = -1;
            value = 1;
        }
    }
}
