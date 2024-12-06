using System;
using System.Diagnostics;
using System.Reflection.Emit;

namespace PracticeGame
{
    internal class Program
    {
        enum GameShape
        {
            block, player, space
        }

        struct PlayerInfo
        {
            public int playerScore;
            public int stageNum;
            public string[] playerType;
            public int score;
            public int turnNum;
            public string player;
        }

        struct TableInfo
        {
            public int width;
            public int height;
            public int turnEndBlock;
            public int spaceYIndex;
            public int maxHeightIndex;
            public int eliminateNum;
            public int stageUpScore;
            public int turnMaxNum;
            public int currentStage;
            public int shapeIndex;
            public string block;
            public string space;
        }

        static void Main(string[] args)
        {
            //// 가로, 세로의 개수 변수 선언
            //int width = 12;
            //int height = 20;

            // 가로, 세로의 개수 변수 선언
            TableInfo gameInfo = new TableInfo();
            gameInfo.width = 12;
            gameInfo.height = 20;

            // 게임 테이블 안에 내용을 저장할 배열 선언
            // 가로 첫번쨰와 마지막, 세로 첫번째와 마지막에 ■ 들어가기 때문에 내용을 저장할 때는 -2씩 크기만큼 선언
            //string[,] tableArr = new string[height, width];
            string[,] tableArr = new string[gameInfo.height, gameInfo.width];

            //int turnNum = 0;
            int shapeIndex = 0;

            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.turnNum = 0;

            //int turnMaxNum = 10;
            gameInfo.turnMaxNum = 10;


            // 점수 넣을 변수 선언
            //int playerScore = 0;
            playerInfo.playerScore = 0;

            // 단계 넣을 변수 선언
            //int currentStage = 1;
            gameInfo.currentStage = 1;

            //int maxHeightIndex = 0;
            gameInfo.maxHeightIndex = 0;

            // 스테이지가 증가할 수 있는 최대 점수 변수 선언
            //int stageUpScore = 50;
            gameInfo.stageUpScore = 50;

            // 서로 같은 것을 맞춰서 사라졌을 때 카운트할 변수 선언
            //int eliminateNum = 0;
            gameInfo.eliminateNum = 0;

            // ■ 내려온게 몇 줄인지 저장할 변수
            //int turnEndBlock = 0;
            gameInfo.turnEndBlock = 0;

            // 비어있는 공간의 Y인덱스 저장할 변수 선언
            //int spaceYIndex = 0;
            gameInfo.spaceYIndex = 0;

            bool isBlock = false;
            // 게임이 끝날때를 판별할 bool형 변수 선언
            bool isStageUp = false;
            bool isFinish = false;

            //string[] playerType = new string[] { "●", "○", "★", "☆" };
            //string player = playerType[0];
            playerInfo.playerType = new string[] { "●", "○", "★", "☆" };
            playerInfo.player = playerInfo.playerType[0];

            Random rand = new Random();

            //string space = "　";
            //string block = "■";
            gameInfo.space = "　";
            gameInfo.block = "■";

            // 시간 측정하는 변수 선언
            //Stopwatch stopwatch = new Stopwatch();

            // A면 뒤로 이동
            // D면 앞으로 이동
            //ConsoleKeyInfo keyInput;
            int posX = 0;
            int posY = 0;

            // 플레이어가 ●, ○, ★, ☆ 이 중에서 랜덤하게 나오게 출력
            shapeIndex = rand.Next(playerInfo.playerType.Length);
            playerInfo.player = playerInfo.playerType[shapeIndex];

            PrintGameBoard(tableArr, gameInfo.block, playerInfo.player, gameInfo.space, gameInfo.currentStage, playerInfo.playerScore);

            // 맨 처음 플레이어 위치 설정
            posX = gameInfo.width / 2 + 4;
            posY = gameInfo.height - 2;

            // 가로로 한줄이 같으면 다음 줄도 같이 없애기


            while (true)
            {
                // 플레이어가 져서 다음 스테이지로 못 가고 다시 시작
                if (isFinish)
                {
                    playerInfo.playerScore = 0;
                    playerInfo.turnNum = 0;
                    Console.Clear();
                    PrintGameBoard(tableArr, gameInfo.block, playerInfo.player, gameInfo.space, gameInfo.currentStage, playerInfo.playerScore);
                }

                // 플레이어가 이겨서 다음 스테이지로 간다면
                if (isStageUp)
                {
                    // 한 턴이 끝날 때마다 스테이지 1씩 증가
                    gameInfo.currentStage += 1;
                    playerInfo.playerScore = 0;
                    playerInfo.turnNum = 0;
                    // 스테이지 값 수정
                    //Console.SetCursorPosition(width + 5, height - 2);
                    //Console.WriteLine($"\t\t\t{currentStage}");
                    Console.Clear();
                    PrintGameBoard(tableArr, gameInfo.block, playerInfo.player, gameInfo.space, gameInfo.currentStage, playerInfo.playerScore);
                }
                //Console.SetCursorPosition(posX, posY);
                //while (playerInfo.turnNum < gameInfo.turnMaxNum)
                //{
                    //keyInput = Console.ReadKey(true);
                    //if (keyInput.Key == ConsoleKey.Enter)
                    //{
                    //    // 나중에 함수 사용
                    //    // 놓은 위치에 도형이 없으면 posY = 1에서 생성
                    //    spaceYIndex = turnEndBlock + 1;
                    //    // 비어 있지 않은 공간 확인할 때까지 반복
                    //    while (tableArr[spaceYIndex, posX / 2] != space)
                    //    {
                    //        spaceYIndex++;
                    //    }
                    //    if (spaceYIndex >= height - 4)
                    //    {
                    //        isFinish = true;
                    //        break;
                    //    }
                    //    if (spaceYIndex > maxHeightIndex)
                    //    {
                    //        maxHeightIndex = spaceYIndex;
                    //    }

                    //    // 도형이 있는 공간의 Y좌표 인덱스 - 1 즉, 위에 있는 도형과 올린 도형이 같으면
                    //    if (tableArr[spaceYIndex - 1, posX / 2] == player)
                    //    {
                    //        tableArr[spaceYIndex - 1, posX / 2] = space;
                    //        // 일단 같은 도형 출력하고 나서
                    //        // 시간이 지나고 없어지게 만들기
                    //        Console.SetCursorPosition(posX, spaceYIndex);
                    //        Console.Write(player);
                    //        // 생성된 블록 바로 아래에서 제거하면 +1씩 증가시키기
                    //        if (tableArr[spaceYIndex - 2, posX / 2] == block)
                    //        {
                    //            eliminateNum++;
                    //        }
                    //        while (true)
                    //        {
                    //            stopwatch.Start();
                    //            if (stopwatch.ElapsedMilliseconds / 1000 > 0.8f)
                    //            {
                    //                Console.SetCursorPosition(posX, spaceYIndex - 1);
                    //                Console.Write("　");
                    //                Console.SetCursorPosition(posX, spaceYIndex);
                    //                Console.Write("　");
                    //                stopwatch.Reset();
                    //                break;
                    //            }
                    //        }

                    //        // 도형 하나 없앨때마다 플레이어 Score 1씩 증가
                    //        playerScore += 1;

                    //        // 플레이어 점수 수정
                    //        Console.SetCursorPosition(width + 5, height - 1);
                    //        Console.WriteLine($"\t\t\t{playerScore}");
                    //        if (playerScore > stageUpScore)
                    //        {
                    //            isStageUp = true;
                    //            break;
                    //        }


                    //    }
                    //    // 위에 있는 도형과 올린 도형이 같지 않으면 그 아래에 플레이어 도형 출력
                    //    else
                    //    {
                    //        tableArr[spaceYIndex, posX / 2] = player;
                    //        Console.SetCursorPosition(posX, spaceYIndex);
                    //        Console.Write(player);
                    //    }
                    //    // 턴이 한 번 넘어갈 수록 
                    //    turnNum++;

                    //    // 플레이어 도형 다른 모양으로 랜덤하게 생성
                    //    shapeIndex = rand.Next(playerType.Length);
                    //    player = playerType[shapeIndex];
                    //}
                    //else if (keyInput.Key == ConsoleKey.A)
                    //{
                    //    posX -= 2;
                    //    Console.SetCursorPosition(posX + 2, posY);
                    //    Console.WriteLine(space);

                    //}
                    //else if (keyInput.Key == ConsoleKey.D)
                    //{
                    //    posX += 2;
                    //    Console.SetCursorPosition(posX - 2, posY);
                    //    Console.WriteLine(space);

                    //}

                    //if (posX < 4)
                    //{
                    //    posX = 2;
                    //}
                    //// ■ 문자표가 2칸을 차지하므로 10 * 2
                    //if (posX > 20)
                    //{
                    //    posX = 20;
                    //}

                    //// A, D로 움직이지 않고도 첫 시작에 바로 나오게 하는 방법
                    //Console.SetCursorPosition(posX, posY);
                    //Console.Write(player);

                    KeyInputPrintOutput(tableArr, gameInfo, playerInfo, isFinish, isStageUp, rand);
                //}

                // 턴이 끝나면 ■로 한줄을 채움
                if (gameInfo.eliminateNum < 3)
                {
                    gameInfo.maxHeightIndex++;
                    int[] yIndex = new int[gameInfo.maxHeightIndex];

                    for(int i= gameInfo.maxHeightIndex-gameInfo.turnEndBlock + 1; i > gameInfo.turnEndBlock - 1; i--)
                    {
                        for(int j=0; j<tableArr.GetLength(1); j++)
                        {
                            if (tableArr[i,j] != gameInfo.block && tableArr[i,j] != gameInfo.space)
                            {
                                tableArr[i + 1, j] = tableArr[i, j];
                            }
                        }
                    }
                    gameInfo.turnEndBlock++;
                    for (int j = 0; j < tableArr.GetLength(1); j++)
                    {
                        tableArr[gameInfo.turnEndBlock, j] = gameInfo.block;
                    }
                }
                // block이 여러 줄로 쌓여 있을 때 같은 도형으로 몇 번 서로 없애는 것에 성공하면 한 줄 없애기
                else
                {
                    // 턴이 끝나게 되면 플레이어가 이겨서 한 줄 늘어나 있던 블록을 하나씩 없애서 배열에 저장하면 됨

                    int[] yIndex = new int[gameInfo.maxHeightIndex];

                    if (gameInfo.turnEndBlock > 0)
                    {
                        for (int i = 0; i < tableArr.GetLength(1); i++)
                        {
                            if (i == 0 || i == gameInfo.width - 1)
                            {
                                continue;
                            }
                            tableArr[gameInfo.turnEndBlock, i] = gameInfo.space;
                        }
                        for (int i = gameInfo.turnEndBlock; i < gameInfo.maxHeightIndex + gameInfo.turnEndBlock; i++)
                        {
                            for (int j = 0; j < tableArr.GetLength(1); j++)
                            {
                                if (tableArr[i, j] != gameInfo.block && tableArr[i, j] != gameInfo.space)
                                {
                                    tableArr[i - 1, j] = tableArr[i, j];
                                    tableArr[i, j] = gameInfo.space;
                                }
                            }
                        }
                        gameInfo.turnEndBlock--;
                        gameInfo.maxHeightIndex--;
                    }
                    else
                    {
                        gameInfo.turnEndBlock = 0;
                    }
                }

                gameInfo.eliminateNum = 0;
                playerInfo.turnNum = 0;

                posX = gameInfo.width / 2 + 4;
                posY = gameInfo.height - 2;

                // player가 도형을 쌓는데 블록이 한줄 안 늘어나고 기존과 같다면 동일하게 재생성하고
                // 블록이 한 줄 늘어나면 생긴 블록 뒤에 한 줄이 느는것 뿐만 아니라 그 동안 넣었던 도형들은 다음 줄에 그대로 나와야 함
                //Console.WriteLine();
                Console.Clear();
                for (int i = 0; i < tableArr.GetLength(0); i++)
                {
                    for (int j = 0; j < tableArr.GetLength(1); j++)
                    {
                        if (j == gameInfo.width / 2 - 1 && i == gameInfo.height - 2)
                        {
                            tableArr[i, j] = playerInfo.player;
                        }
                        Console.Write(tableArr[i, j]);
                        // 스테이지, 플레이어 스코어 출력
                        if (i == gameInfo.height - 2 && j == gameInfo.width - 1)
                        {
                            Console.Write($"\tStage : {gameInfo.currentStage}");
                        }
                        if (i == gameInfo.height - 1 && j == gameInfo.width - 1)
                        {
                            Console.Write("\tScore : " + playerInfo.playerScore);
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
        static void PrintGameBoard(string[,] gameArr, string block, string player, string space, int stage, int score)
        {
            bool isBlock = false;

            for (int i = 0; i < gameArr.GetLength(0); i++)
            {
                for (int j = 0; j < gameArr.GetLength(1); j++)
                {
                    isBlock = (i <= 0) || (j == 0) || (j == gameArr.GetLength(1) - 1) || (i == gameArr.GetLength(0) - 1);
                    if (isBlock)
                    {
                        gameArr[i, j] = block;
                    }
                    else if (j == gameArr.GetLength(1) / 2 - 1 && i == gameArr.GetLength(0) - 2)
                    {
                        gameArr[i, j] = player;
                    }
                    else
                    {
                        gameArr[i, j] = space;
                    }
                    Console.Write(gameArr[i, j]);

                    // 스테이지, 플레이어 스코어 출력
                    if (i == gameArr.GetLength(0) - 2 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write($"\tStage : {stage}");
                    }
                    if (i == gameArr.GetLength(0) - 1 && j == gameArr.GetLength(1) - 1)
                    {
                        Console.Write("\tScore : " + score);
                    }
                }
                Console.WriteLine();
            }
        }

        static void KeyInputPrintOutput(string[,] gameArr, TableInfo infoGame, PlayerInfo infoPlayer, bool isFinish, bool isStageUp, Random random)
        {
            int posX = gameArr.GetLength(1) / 2 + 4;
            int posY = gameArr.GetLength(0) - 2;

            int playerIndex = 0;

            Stopwatch stopwatch = new Stopwatch();
            ConsoleKeyInfo keyInput;

            Console.SetCursorPosition(posX, posY);
            while (infoPlayer.turnNum < infoGame.turnMaxNum)
            {
                keyInput = Console.ReadKey(true);
                if (keyInput.Key == ConsoleKey.Enter)
                {
                    // 나중에 함수 사용
                    // 놓은 위치에 도형이 없으면 posY = 1에서 생성
                    infoGame.spaceYIndex = infoGame.turnEndBlock + 1;
                    // 비어 있지 않은 공간 확인할 때까지 반복
                    while (gameArr[infoGame.spaceYIndex, posX / 2] != infoGame.space)
                    {
                        infoGame.spaceYIndex++;
                    }
                    // while문이 있을 때 유용한거라서 보류
                    if (infoGame.spaceYIndex >= infoGame.height - 4)
                    {
                        isFinish = true;
                        break;
                    }
                    if (infoGame.spaceYIndex > infoGame.maxHeightIndex)
                    {
                        infoGame.maxHeightIndex = infoGame.spaceYIndex;
                    }

                    // 도형이 있는 공간의 Y좌표 인덱스 - 1 즉, 위에 있는 도형과 올린 도형이 같으면
                    if (gameArr[infoGame.spaceYIndex - 1, posX / 2] == infoPlayer.player)
                    {
                        gameArr[infoGame.spaceYIndex - 1, posX / 2] = infoGame.space;
                        // 일단 같은 도형 출력하고 나서
                        // 시간이 지나고 없어지게 만들기
                        Console.SetCursorPosition(posX, infoGame.spaceYIndex);
                        Console.Write(infoPlayer.player);
                        // 생성된 블록 바로 아래에서 제거하면 +1씩 증가시키기
                        if (gameArr[infoGame.spaceYIndex - 2, posX / 2] == infoGame.block)
                        {
                            infoGame.eliminateNum++;
                        }
                        while (true)
                        {
                            stopwatch.Start();
                            if (stopwatch.ElapsedMilliseconds / 1000 > 0.8f)
                            {
                                Console.SetCursorPosition(posX, infoGame.spaceYIndex - 1);
                                Console.Write("　");
                                Console.SetCursorPosition(posX, infoGame.spaceYIndex);
                                Console.Write("　");
                                stopwatch.Reset();
                                break;
                            }
                        }

                        // 도형 하나 없앨때마다 플레이어 Score 1씩 증가
                        infoPlayer.playerScore += 1;

                        // 플레이어 점수 수정
                        Console.SetCursorPosition(infoGame.width + 5, infoGame.height - 1);
                        Console.WriteLine($"\t\t\t{infoPlayer.playerScore}");
                        if (infoPlayer.playerScore > infoGame.stageUpScore)
                        {
                            isStageUp = true;
                            break;
                        }


                    }
                    // 위에 있는 도형과 올린 도형이 같지 않으면 그 아래에 플레이어 도형 출력
                    else
                    {
                        gameArr[infoGame.spaceYIndex, posX / 2] = infoPlayer.player;
                        Console.SetCursorPosition(posX, infoGame.spaceYIndex);
                        Console.Write(infoPlayer.player);
                    }
                    // 턴이 한 번 넘어갈 수록 
                    infoPlayer.turnNum++;

                    // 플레이어 도형 다른 모양으로 랜덤하게 생성
                    playerIndex = random.Next(infoPlayer.playerType.Length);
                    infoPlayer.player = infoPlayer.playerType[playerIndex];
                }
                else if (keyInput.Key == ConsoleKey.A)
                {
                    posX -= 2;
                    Console.SetCursorPosition(posX + 2, posY);
                    Console.WriteLine(infoGame.space);

                }
                else if (keyInput.Key == ConsoleKey.D)
                {
                    posX += 2;
                    Console.SetCursorPosition(posX - 2, posY);
                    Console.WriteLine(infoGame.space);

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
                Console.Write(infoPlayer.player);
            }
        }
    }
}
