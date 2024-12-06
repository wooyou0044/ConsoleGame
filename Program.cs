using System;
using System.Diagnostics;

namespace PracticeGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 가로, 세로의 개수 변수 선언
            int width = 12;
            int height = 20;

            // 게임 테이블 안에 내용을 저장할 배열 선언
            // 가로 첫번쨰와 마지막, 세로 첫번째와 마지막에 ■ 들어가기 때문에 내용을 저장할 때는 -2씩 크기만큼 선언
            string[,] tableArr = new string[height, width];

            int turnNum = 0;
            int turnMaxNum = 10;
            int shapeIndex = 0;

            // 점수 넣을 변수 선언
            int playerScore = 0;

            // 단계 넣을 변수 선언
            int currentStage = 0;

            int maxHeightIndex = 0;

            // 스테이지가 증가할 수 있는 최대 점수 변수 선언
            int stageUpScore = 10;

            // 서로 같은 것을 맞춰서 사라졌을 때 카운트할 변수 선언
            int eliminateNum = 0;

            // ■ 내려온게 몇 줄인지 저장할 변수
            int turnEndBlock = 0;

            // 비어있는 공간의 Y인덱스 저장할 변수 선언
            int spaceYIndex = 0;

            bool isBlock = false;

            string[] playerType = new string[] { "●", "○", "★", "☆" };
            string player = playerType[0];

            Random rand = new Random();

            string space = "　";
            string block = "■";

            // 시간 측정하는 변수 선언
            Stopwatch stopwatch = new Stopwatch();

            // A면 뒤로 이동
            // D면 앞으로 이동
            ConsoleKeyInfo keyInput;
            int posX = 0;
            int posY = 0;

            // 플레이어가 ●, ○, ★, ☆ 이 중에서 랜덤하게 나오게 출력
            shapeIndex = rand.Next(playerType.Length);
            player = playerType[shapeIndex];

            for (int i = 0; i < tableArr.GetLength(0); i++)
            {
                for (int j = 0; j < tableArr.GetLength(1); j++)
                {
                    isBlock = (i <= turnEndBlock) || (j == 0) || (j == width - 1) || (i == height - 1);
                    if (isBlock)
                    {
                        tableArr[i, j] = block;
                    }
                    else if(j == width / 2 - 1 && i == height - 2)
                    {
                        tableArr[i, j] = player;
                    }
                    else
                    {
                        tableArr[i, j] = space;
                    }
                    Console.Write(tableArr[i, j]);

                    // 스테이지, 플레이어 스코어 출력
                    if (i == height - 2 && j == width -1)
                    {
                        Console.Write($"\tStage : {currentStage + 1}");
                    }
                    if(i == height -1 && j== width -1)
                    {
                        Console.Write("\tScore : " + playerScore);
                    }
                }
                Console.WriteLine();
            }

            // 맨 처음 플레이어 위치 설정
            posX = width / 2 + 4;
            posY = height - 2;

            while (playerScore < stageUpScore && spaceYIndex <= height - 4)
            {
                Console.SetCursorPosition(posX, posY);
                while (turnNum < turnMaxNum)
                {
                    keyInput = Console.ReadKey(true);
                    if (keyInput.Key == ConsoleKey.Enter)
                    {
                        // 나중에 함수 사용
                        // 놓은 위치에 도형이 없으면 posY = 1에서 생성
                        spaceYIndex = turnEndBlock + 1;
                        // 비어 있지 않은 공간 확인할 때까지 반복
                        while (tableArr[spaceYIndex, posX / 2] != space)
                        {
                            spaceYIndex++;
                        }
                        Console.WriteLine("spaceYIndex : " + spaceYIndex);
                        if (spaceYIndex > maxHeightIndex)
                        {
                            maxHeightIndex = spaceYIndex;
                        }

                        // 도형이 있는 공간의 Y좌표 인덱스 - 1 즉, 위에 있는 도형과 올린 도형이 같으면
                        if (tableArr[spaceYIndex - 1, posX / 2] == player)
                        {
                            tableArr[spaceYIndex - 1, posX / 2] = space;
                            // 일단 같은 도형 출력하고 나서
                            // 시간이 지나고 없어지게 만들기
                            Console.SetCursorPosition(posX, spaceYIndex);
                            Console.Write(player);
                            // 생성된 블록 바로 아래에서 제거하면 +1씩 증가시키기
                            if (tableArr[spaceYIndex - 2, posX / 2] == block)
                            {
                                eliminateNum++;
                            }
                            while (true)
                            {
                                stopwatch.Start();
                                if (stopwatch.ElapsedMilliseconds / 1000 > 0.8f)
                                {
                                    Console.SetCursorPosition(posX, spaceYIndex - 1);
                                    Console.Write("　");
                                    Console.SetCursorPosition(posX, spaceYIndex);
                                    Console.Write("　");
                                    stopwatch.Reset();
                                    break;
                                }
                            }
                        }
                        // 위에 있는 도형과 올린 도형이 같지 않으면 그 아래에 플레이어 도형 출력
                        else
                        {
                            tableArr[spaceYIndex, posX / 2] = player;
                            Console.SetCursorPosition(posX, spaceYIndex);
                            Console.Write(player);
                        }
                        // 턴이 한 번 넘어갈 수록 
                        turnNum++;

                        // 플레이어 도형 다른 모양으로 랜덤하게 생성
                        shapeIndex = rand.Next(playerType.Length);
                        player = playerType[shapeIndex];
                    }
                    else if (keyInput.Key == ConsoleKey.A)
                    {
                        posX -= 2;
                        Console.SetCursorPosition(posX + 2, posY);
                        Console.WriteLine(space);

                    }
                    else if (keyInput.Key == ConsoleKey.D)
                    {
                        posX += 2;
                        Console.SetCursorPosition(posX - 2, posY);
                        Console.WriteLine(space);

                    }

                    if (posX < 4)
                    {
                        posX = 2;
                    }
                    // ■ 문자표가 2칸을 차지하므로 10 * 2
                    if (posX > 20)
                    {
                        posX = 20;
                    }

                    // A, D로 움직이지 않고도 첫 시작에 바로 나오게 하는 방법
                    Console.SetCursorPosition(posX, posY);
                    Console.Write(player);
                }

                // 턴이 끝나면 ■로 한줄을 채움
                //if ((turnNum >= turnMaxNum) && (eliminateNum < 3))
                if (eliminateNum < 5)
                {
                    maxHeightIndex++;
                    int[] yIndex = new int[maxHeightIndex];

                    for(int i= maxHeightIndex-turnEndBlock + 1; i > turnEndBlock - 1; i--)
                    {
                        for(int j=0; j<tableArr.GetLength(1); j++)
                        {
                            if (tableArr[i,j] != block && tableArr[i,j] != space)
                            {
                                tableArr[i + 1, j] = tableArr[i, j];
                            }
                        }
                    }
                    turnEndBlock++;
                    for (int j = 0; j < tableArr.GetLength(1); j++)
                    {
                        tableArr[turnEndBlock, j] = block;
                    }
                }
                // block이 여러 줄로 쌓여 있을 때 같은 도형으로 몇 번 서로 없애는 것에 성공하면 한 줄 없애기
                else
                {
                    // 턴이 끝나게 되면 플레이어가 이겨서 한 줄 늘어나 있던 블록을 하나씩 없애서 배열에 저장하면 됨

                    int[] yIndex = new int[maxHeightIndex];

                    if (turnEndBlock > 0)
                    {
                        for (int i = 0; i < tableArr.GetLength(1); i++)
                        {
                            if (i == 0 || i == width - 1)
                            {
                                continue;
                            }
                            tableArr[turnEndBlock, i] = space;
                        }
                        for (int i = turnEndBlock; i < maxHeightIndex + turnEndBlock; i++)
                        {
                            for (int j = 0; j < tableArr.GetLength(1); j++)
                            {
                                if (tableArr[i, j] != block && tableArr[i, j] != space)
                                {
                                    tableArr[i - 1, j] = tableArr[i, j];
                                    tableArr[i, j] = space;
                                }
                            }
                        }
                        turnEndBlock--;
                        maxHeightIndex--;
                    }
                    else
                    {
                        turnEndBlock = 0;
                    }

                    // 도형 하나 없앨때마다 플레이어 Score 1씩 증가
                    playerScore += 1;

                    // 플레이어 점수 수정
                    Console.SetCursorPosition(width + 5, height - 1);
                    Console.WriteLine($"\t\t\t{playerScore}");
                }

                eliminateNum = 0;
                turnNum = 0;

                posX = width / 2 + 4;
                posY = height - 2;

                // player가 도형을 쌓는데 블록이 한줄 안 늘어나고 기존과 같다면 동일하게 재생성하고
                // 블록이 한 줄 늘어나면 생긴 블록 뒤에 한 줄이 느는것 뿐만 아니라 그 동안 넣었던 도형들은 다음 줄에 그대로 나와야 함
                //Console.WriteLine();
                Console.Clear();
                for (int i = 0; i < tableArr.GetLength(0); i++)
                {
                    for (int j = 0; j < tableArr.GetLength(1); j++)
                    {
                        if (j == width / 2 - 1 && i == height - 2)
                        {
                            tableArr[i, j] = player;
                        }
                        Console.Write(tableArr[i, j]);
                        // 스테이지, 플레이어 스코어 출력
                        if (i == height - 2 && j == width - 1)
                        {
                            Console.Write($"\tStage : {currentStage + 1}");
                        }
                        if (i == height - 1 && j == width - 1)
                        {
                            Console.Write("\tScore : " + playerScore);
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            // 한 턴이 끝날 때마다 스테이지 1씩 증가
            currentStage += 1;
            // 스테이지 값 수정
            Console.SetCursorPosition(width + 5, height - 2);
            Console.WriteLine($"\t\t\t{currentStage}");
        }
    }
}
