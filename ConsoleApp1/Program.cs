using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;


namespace ConsoleApp1
{
    class Program
    {

        /*
         * 각 칸에는 번호가 부여되며, 아래와 같은 방법으로 게임이 진행된다.
         * 
         * 윷말의 방향에는 2가지 예외가 존재한다.
         * 1) -1은 빽도를 의미하며, 뒤로 한 칸 간다.
         * 2) 32는 도착지점(0)을 의미한다.
         * 
         * 게임판 번호는 다음과 같다.
         * 
         * 10  9  8  7  6  5
         * 11 25        20 4
         * 12    26   21   3
         *         22
         * 13    23   27   2
         * 14 24        28 1
         * 15 16 17 18 19  0
         * 
         * 모든 윷말은 0번에서 시작하지만, 0번에 있을 때는 말이 보이지 않는다.
         * 0에서 시작해서 한 바퀴 돌아 0으로 이동한다.
         * 그리고 모든 말은 최소한 한 번 이상은 0번을 지나쳐야 한다.
         * 
         * 5, 10, 22번에서는 지름길로 갈 수 있다.
         * 단, 거치는 것만으로는 지름길로 갈 수 없고, 5, 10, 22번에 머무른 후에만 가능하다.
         * 
         */

        public class Board
        {

            public void Parray()
            {
                foreach (int i in rollArray)
                    Console.Write("{0} ", i);
                Console.WriteLine();
            }


            //roll=윷을 던진 결과를 의미함

            const int MAX_ROLLS = 5;                        //최대 5번까지 윷을 던질 수 있음
            private int[] rollArray = new int[MAX_ROLLS];   //윷을 던진 결과
            private int rollIndex = 0;                      
            private int playerTurn = 0;                     //현재 차례(0 = player1, 1=player2

            //treeset>array
            //이 번호들은 다르게 처리해야 함. 단순하게 위치를 생각해서는 안 됨
            static int[] specialTiles = new int[] { 0, 1, 5, 10, 15, 20, 21, 22, 23, 24, 25, 26, 27 };

            //움직임을 나타내는 
            char[] moves = { 'U', 'U', 'U', 'U', 'U', 'L', 'L', 'L', 'L', 'L', 'D', 'D', 'D', 'D', 'D', 'R', 'R', 'R', 'R' };

            /*
             * roll을 0으로 초기화
             */
            public Board()
            {
                for(int i=0; i<MAX_ROLLS; i++)
                {
                    rollArray[i] = 0;
                }
            }

            /*
             * 던질 윷가락 수는 4개이고, 각 윷은 위아래로 향할 수 있다
             * 규칙은 다음의 예외를 제외하고, 위를 향한 윷 개수에 따라 결정된다.
             * all sticks face down is a roll of 5 and 하나의 윷에는 -1을 의미하는 표시가 있다.
             * 확률은 다음과 같다. 
             * (1) 4/16 | (2) 6/16 | (3) 4/16 | (4) 1/16 | (5) 1/16 | (-1) 1/16
             * 
             * @return; 윷을 던져서 나온 값
             */

            public int ThrowSticks()
            {
                int roll = 1;

                Random rand = new Random();
                int num = rand.Next(1, 16);

                if (num == 1) roll = -1;
                else if (num > 1 && num <= 4) roll = 1;
                else if (num > 4 && num <= 10) roll = 2;
                else if (num > 10 && num <= 14) roll = 3;
                else if (num > 14 && num <= 15) roll = 4;
                else if (num > 15 && num <= 16) roll = 5;

                return roll;
    
            }
            /*
             * rollArray에 윷을 던져서 나온 수 추가
             * 
             * @param 윷을 던져서 나온 칸 수
             */
            public void AddRoll(int roll)
            {
                rollArray[rollIndex] = roll;
                if (rollIndex < 4) rollIndex++;
            }

            /*
             * 사용자가 윷을 던질 수 있는 남은 횟수
             * 
             * @return Number of rolls available
             */
            public int GetNonZeroRollCount()
            {
                int count = 0;
                foreach (int i in rollArray)
                {
                    if (i != 0) count++;
                }
                return count;

            }
            
            /*
             * -1(빽도)를 제외한 양의 칸 수의 개수 구하기
             * 
             * @return rollArray에서 -1(빽도)를 제외한 양의 roll의 개수
             */
            public int GetPosRollCount()
            {
                int posCount = 0;

                foreach (int i in rollArray)
                {
                    if (i != 0 && i != -1) posCount++;
                }
                return posCount;
            }

            /*
             * roll array 전체 원소를 반환
             * 
             * @return rollArray
             */
            public int[] GetRollArray()
            {
                return rollArray;
            }

            /*
             * 현재 roll index 반환
             * 
             * @return 현재 roll index
             */
            public int GetRollIndex()
            {
                return rollIndex;
            }

            /*
             * 요구받은 index의 칸 수 반환
             * 
             * @param index
             * @return 해당 인덱스의 roll 값
             */
            public int GetRollAtIndex(int index)
            {
               return rollArray[index];
            }

            public void Reset()
            {
                playerTurn = 0;
                rollIndex = 0;
                ResetRollArray();
           
            }
            /*
             * 모든 roll을 0으로 세팅
             */
            public void ResetRollArray()
            {
                rollIndex = 0;
                for(int i=0; i<MAX_ROLLS; i++)
                {
                    rollArray[i] = 0;
                }
            }
            /*
             * 현재 턴을 설정, 온라인 모드에 이용될 것
             * 
             * 0 = 내 차례
             * 1 = 상대방 차례
             * 
             * @param 차례
             */
            public void SetPlayerTurn(int turn)
            {
                playerTurn = turn;
            }
            /*
             * playerTurn을 바꾸고 rollArray를 비우기
             */
             public void EndTurn()
             {
                playerTurn = (playerTurn + 1) % 2;
                ResetRollArray();
             }

            /*
             * roll을 roll array에서 제거한 후, 0이 아닌 엔트리를 왼쪽으로 순서 조정
             * 이 함수는 플레이어의 윷말을 움직인 후 호출되어야 함
             * 
             * @param i; roll array에서 제거할 롤
             */
            public void RemoveRoll(int i)
            {
                bool wasFull = (MAX_ROLLS == GetNonZeroRollCount());

                //첫 번째 roll 크기에 맞는 원소를 찾고 0을 대입해서 제거
                for(int j = 0; j<rollArray.Length; j++)
                {
                    if (rollArray[j] == i) { rollArray[j] = 0; break; }
                }

                //모든 0이 아닌 엔트리를 왼쪽으로 shift
                for(int k = 0; k<rollArray.Length; k++)
                {
                    if (rollArray[k] == 0)
                    {
                        int index = k + 1;
                        while (index < rollArray.Length && rollArray[index] == 0) index += 1;
                        if (index == rollArray.Length) index--;
                        if (rollArray[index] == 0) break;
                        rollArray[k] = rollArray[index];
                        rollArray[index] = 0;
                    }
                }

                //roll index를 1 감소시킴> rollIndex = 새로운 롤을 추가할 위치 
                if (!wasFull && rollIndex > 0) rollIndex--;
                               
            }

            /*
             * rollArray가 비었는지 확인
             * 
             * @return true; rollArray가 비었으면, 그렇지 않으면 false
             */
            public bool RollEmpty()
            {
                int count = 0;
                for(int i=0; i<MAX_ROLLS; i++)
                {
                    if (rollArray[i] != 0) count++;
                }
                return (count == 0);
            }

            /*
             * 남은 롤이 하나이고, -1인지 확인
             * 
             * @return true 유일하게 남은 롤이 -1이면, 그렇지 않으면 false
             */
            public bool HasOnlyNegativeRoll()
            {
                int count = 0;
                for(int i=0; i<MAX_ROLLS; i++)
                {
                    if (rollArray[i] == 0 || rollArray[i] == -1) count++;
                }
                return (count == MAX_ROLLS);
            }
            /*
             * 이동 가능한 위치를 계산하기 위해 roll 값과 시작 위치를 처리한다.
             * 
             * 2개의 int형 원소를 가진 리스트를 반환한다. 리스트는 다음과 같다:
             * 
             * index 0 = 도착 위치
             * index 1 = 롤의 값
             * 
             * 두 가지 경우의 수가 있을 수 있다.
             * 한 가지 경우의 수만 있다면 오직 1 integer array만 반환한다.
             * 
             * @param move (롤의 값)
             * @param 시작 위치
             * @return [도착지점, 롤의 값]을 저장하고 있는 an ArrayList<int[]>
             */

            public static List<int> CalculateLocation(int move, int location)
            {
                List<int> moveList = new List<int>();

                if (move != 0)
                {
                    if (specialTiles.Contains(location))
                    {
                        if (location == 0)
                        {
                            if (move >= 1) location = 32;
                            else
                            {
                                location = 19;

                                /*
                                 * 두 번째 경우의 수는 secondMove에 담음
                                 */

                                int[] secondMove = new int[2];
                                secondMove[0] = 28;
                                secondMove[1] = move;
                                moveList.AddRange(secondMove);

                            }
                        }
                        else if (location == 1)
                        {
                            location = 1 + move;
                        }
                        else if (location == 5)
                        {
                            if (move >= 1)
                            {
                                location = 19 + move;
                                 
                                int[] secondMove = new int[2];
                                secondMove[0] = 5 + move;
                                secondMove[1] = move;
                                moveList.AddRange(secondMove);
                            }
                                
                            else
                                location--;
                        }
                        else if (location == 10)
                        {
                            if (move == 1 || move == 2)
                            {
                                location = 24 + move;

                                int[] secondMove = new int[2];
                                secondMove[0] = 10 + move;
                                secondMove[1] = move;
                                moveList.AddRange(secondMove);
                            }
                                
                            else if (move == 3)
                            {
                                location = 22;

                                int[] secondMove = new int[2];
                                secondMove[0] = 10 + move;
                                secondMove[1] = move;
                                moveList.AddRange(secondMove);
                            }
                                
                            else if (move == 4 || move == 5)
                            {
                                location = 23 + move;

                                int[] secondMove = new int[2];
                                secondMove[0] = 10 + move;
                                secondMove[1] = move;
                                moveList.AddRange(secondMove);
                            }
                                
                            else
                                location--;

                        }
                        else if (location == 15)
                        {
                            if (move > 0 && move < 5)
                            {
                                location += move;

                                int[] secondMove = new int[2];
                                secondMove[0] = 25 - move;
                                secondMove[1] = move;
                                moveList.AddRange(secondMove);
                            }
                                
                            else if (move == 5)
                            {
                                location = 0;

                                int[] secondMove = new int[2];
                                secondMove[0] = 25 - move;
                                secondMove[1] = move;
                                moveList.AddRange(secondMove);

                            }
                                
                            else
                            {
                                location--;

                                int[] secondMove = new int[2];
                                secondMove[0] = 24;
                                secondMove[1] = move;
                                moveList.AddRange(secondMove);
                            }
                        }
                        else if (location == 20)
                        {
                            if (move >= 1 && move <= 4)
                                location = 20 + move;
                            else if (move == -1)
                                location = 5;
                            else if (move == 5)
                                location = 15;
                        }
                        else if (location == 21)
                        {
                            if (move >= 1 && move <= 3)
                                location = 21 + move;
                            else if (move == -1)
                                location--;
                            else if (move == 4)
                                location = 15;
                            else if (move == 5)
                                location = 16;
                        }
                        else if (location == 22)
                            if (move == 1 || move == 2 )
                            {
                                location = 26 + move;

                                int[] secondMove = new int[2];
                                secondMove[0] = 22 + move;
                                secondMove[1] = move;
                                moveList.AddRange(secondMove);

                            }
                            else if (move == 3)
                            {
                                location = 29;

                                int[] secondMove = new int[2];
                                secondMove[0] = 15;
                                secondMove[1] = move;
                                moveList.AddRange(secondMove);
                            }
                                
                            else if (move > 3)
                            {
                                location = 32;

                                int[] secondMove = new int[2];
                                secondMove[0] = 12 +  move;
                                secondMove[1] = move;
                                moveList.AddRange(secondMove);
                            }
                                
                            else
                            {
                                location = 21;

                                int[] secondMove = new int[2];
                                secondMove[0] = 26;
                                secondMove[1] = move;
                                moveList.AddRange(secondMove);
                            }
                        else if (location == 23)
                        {
                            if (move == 1)
                                location++;
                            else if (move == -1)
                                location--;
                            else if (move >= 2 && move <= 4)
                                location = 13 + move;
                            else if (move == 5)
                                location = 18;
                        }
                        else if (location == 24)
                        {
                            if (move >= 1)
                                location = 14 + move;
                            else if (move == -1)
                                location--;
                        }
                        else if (location == 25)
                        {
                            if (move == 1)
                                location = 26;
                            else if (move == 2)
                                location = 22;
                            else if (move == 3 || move == 4)
                                location = 24 + move;
                            else if (move == 5)
                                location = 0;
                            else
                                location = 10;
                        }
                        else if (location == 26)
                        {
                            if (move == 1)
                                location = 22;
                            else if (move == 2 || move == 3)
                                location = 25 + move;
                            else if (move == 4)
                                location = 0;
                            else if (move == 5)
                                location = 32;
                            else
                                location--;
                        }
                        else if (location == 27)
                        {
                            if (move >= 1)
                                location += move;
                            else
                                location = 22;
                        }
                    }
                    else if (location >= 15 && location <= 19)
                    {
                        if (location + move == 20)
                            location = 0;
                        else if (location + move < 20)
                            location += move;
                        else
                            location = 32;

                    }
                    else if (location == -1)
                    {
                        if (move >= 1)
                            location = move;
                    }
                    else
                        location += move;

                }
                else
                {
                    location = -1;
                }

                if (location == 29)
                    location = 0;
                else if (location > 29)
                    location = 32;

                int[] possibleMoves = new int[2];
                possibleMoves[0] = location;
                possibleMoves[1] = move;
                moveList.AddRange(possibleMoves);

                return moveList;

            }

            /*
             * 시작 위치에서 목적지로 윷말의 움직임을 애니메이션에 적용하기 한 경로 계산
             * n 개의 타일을 가로질러 움직이는 롤이라면, n개의 원소를 저장하는 배열을 하나 생성
             * 각 원소는 보드에서 이동할 방향을 나타냄
             * 
             * 방향 규칙:
             * U = 상
             * D = 하
             * L = 좌
             * R = 우
             * A = 대각선 왼쪽 아래
             * E = 대각선 오른쪽 아래
             * 
             * @param 시작 위치
             * @param 도착 위치
             * @param 롤의 수(윷말을 움직일 칸 수)
             * @return character 배열 - 애니메이션 움직임의 방향을 나타내는 배열
             */

            public char[] CalculatePath(int start, int dest, int numMoves)
            {
                char[] array;

                if (numMoves > 0)
                    array = new char[numMoves];
                else
                    array = new char[1];

                int j = 0;

                if (numMoves == -1)
                {
                    //두 방향으로 갈 수 있는 위치일 때
                    if (start == 0 && dest == 20) array[0] = 'E';
                    else if (start == 0 && dest == 19) array[0] = 'L';
                    else if (start == 15 && dest == 14) array[0] = 'U';
                    else if (start == 15 && dest == 24) array[0] = 'A';
                    else if (start == 22 && dest == 26) array[0] = 'E';
                    else if (start == 22 && dest == 21) array[0] = 'A';


                    //한 방향으로 가는 위치 일 때
                    else if (start > 0 && start <= 5) array[0] = 'D';
                    else if (start > 5 && start <= 10) array[0] = 'R';
                    else if (start > 10 && start <= 14) array[0] = 'U';
                    else if (start > 15 && start <= 19) array[0] = 'L';
                    else if (start > 19 && start <= 24) array[0] = 'A';
                    else if (start > 24 && start <= 28) array[0] = 'E';

                }
                else if (start == 0)
                {
                    for (int i = 0; i < numMoves; i++) array[j++] = 'F';
                }
                else if (start == 5)
                {
                    for (int i = 0; i < numMoves; i++) array[j++] = 'C';
                }
                else if (start == 10)
                {
                    for (int i = 0; i < numMoves; i++) array[j++] = 'B';
                }
                else if (start == 20)
                {
                    for (int i = 0; i < numMoves; i++) array[j++] = 'C';
                }
                else if (start == 21)
                {
                    if (numMoves <= 3)
                    {
                        for (int i = 0; i < numMoves; i++) array[j++] = 'C';
                    }
                    else
                    {
                        for (int i = 0; i < numMoves - 1; i++) array[j++] = 'C';
                    }
                }
                else if (start == 22)
                {
                    if (numMoves <= 3)
                    {
                        for (int i = 0; i < numMoves; i++) array[j++] = 'B';
                    }
                    else if (numMoves > 3)
                    {
                        for (int i = 0; i < 3; i++) array[j++] = 'B';
                        for (int i = 3; i < numMoves; i++) array[j++] = 'F';
                    }
                }
                else if (start == 23)
                {
                    if (numMoves <= 2)
                    {
                        for (int i = 0; i < numMoves; i++) array[j++] = 'C';
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++) array[j++] = 'C';
                        for (int i = 2; i < numMoves; i++) array[j++] = 'R';
                    }
                }
                else if (start == 24)
                {
                    array[j++] = 'C';

                    if (numMoves > 1)
                    {
                        for (int i = 1; i < numMoves; i++) array[j++] = 'R';
                    }
                }
                else if (start == 25)
                {
                    for (int i = 0; i < numMoves; i++) array[j++] = 'B';
                }
                else if (start == 26)
                {
                    if (numMoves < 5)
                    {
                        for (int i = 0; i < numMoves; i++) array[j++] = 'B';
                    }
                    else
                    {
                        for (int i = 0; i < numMoves - 1; i++) array[j++] = 'B';
                        array[j] = 'F';
                    }
                }
                else if (start == 27)
                {
                    if (numMoves <= 2)
                    {
                        for (int i = 0; i < numMoves; i++) array[j++] = 'B';
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++) array[j++] = 'B';
                        for (int i = 2; i < numMoves; i++) array[j++] = 'F';
                    }
                }
                else if (start == 28)
                {
                    array[j++] = 'B';
                    if (numMoves > 1)
                    {
                        for (int i = 0; i < numMoves; i++) array[j++] = 'F';
                    }
                }
                else
                {
                    if (start == -1) start++;

                    if (dest == 0 || dest == 32)
                    {
                        if (start == 15)
                        {
                            for (int i = 0; i < numMoves; i++) array[j++] = 'R';
                        }
                        else if (start == 16)
                        {
                            if (numMoves <= 4)
                            {
                                for (int i = 0; i < numMoves; i++) array[j++] = 'R';
                            }
                            else if (numMoves < 4)
                            {
                                for (int i = 0; i < 4; i++) array[j++] = 'R';
                                for (int i = 4; i < numMoves; i++) array[j++] = 'F';
                            }
                        }
                        else if (start == 17)
                        {
                            if (numMoves <= 3)
                            {
                                for (int i = 0; i < numMoves; i++) array[j++] = 'R';
                            }
                            else if (numMoves > 3)
                            {
                                for (int i = 0; i < 3; i++) array[j++] = 'R';
                                for (int i = 3; i < numMoves; i++) array[j++] = 'F';
                            }
                        }
                        else if (start == 18)
                        {
                            if (numMoves <= 2)
                            {
                                for (int i = 0; i < numMoves; i++) array[j++] = 'R';
                            }
                            else if (numMoves > 2)
                            {
                                for (int i = 0; i < 2; i++) array[j++] = 'R';
                                for (int i = 2; i < numMoves; i++) array[j++] = 'F';
                            }
                        }
                        else if (start == 19)
                        {
                            if (numMoves <= 1)
                            {
                                for (int i = 0; i < numMoves; i++) array[j++] = 'R';
                            }
                            else if (numMoves > 1)
                            {
                                for (int i = 0; i < 1; i++) array[j++] = 'R';
                                for (int i = 1; i < numMoves; i++) array[j++] = 'F';
                            }
                        }
                    }
                    else
                    {
                        for (int i = start; i < dest; i++) array[j++] = moves[i];
                    }
                }
                    return array;
            }
           

        }

           



       
    }
}