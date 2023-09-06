using Microsoft.AspNetCore.Components.Web;

namespace BlazorWebAsm.Game2048.Pages.Game
{
    public partial class Game
    {
        public int GridSize { get; set; } = 4;
        public int TargetScore = 1024;

        private List<int> Squares { get; set; }

        private int SquareSize { get => GridSize * GridSize; }
        private int Score { get; set; } = 0;

        private bool IsWin { get; set; }
        private bool IsLoose { get; set; }

        private Toast Toast { get; set; } = new Toast();

        private enum EnumArrowKeys
        {
            ArrowUp, ArrowDown, ArrowLeft, ArrowRight
        };

        private void CreateBoard()
        {
            Squares = new();
            Score = 0;
            IsWin = false;
            IsLoose = false;

            foreach (var item in Enumerable.Range(1, SquareSize))
                Squares.Add(0);

            GenerateTwo();
            GenerateTwo();
        }

        private void GenerateTwo()
        {
            var random = new Random().Next(SquareSize);
            if (Squares[random] == 0)
            {
                Squares[random] = 2;
                CheckLose();
            }
            else GenerateTwo();
        }

        private void Play(KeyboardEventArgs e)
        {
            if (IsLoose || IsWin) return;

            if (e.Key == EnumArrowKeys.ArrowLeft.ToString())
            {
                MoveLeft();
                SumRow();
                GenerateTwo();
                MoveLeft();
            }
            else if (e.Key == EnumArrowKeys.ArrowRight.ToString())
            {
                MoveRight();
                SumRow();
                GenerateTwo();
                MoveRight();
            }
            else if (e.Key == EnumArrowKeys.ArrowDown.ToString())
            {
                MoveDown();
                SumColumn();
                GenerateTwo();
                MoveDown();
            }
            else if (e.Key == EnumArrowKeys.ArrowUp.ToString())
            {
                MoveUp();
                SumColumn();
                GenerateTwo();
                MoveUp();
            }
        }

        private void MoveRight()
        {
            var resultList = new List<int>();
            for (int gridIndex = 0; gridIndex < Squares.Count; gridIndex++)
            {
                var row = new List<int>();
                if (gridIndex % GridSize == 0)
                {
                    for (int i = 0; i < GridSize; i++)
                    {
                        row.Add(Squares[gridIndex + i]);
                    }
                }

                var filteredRow = row.Where(x => x != 0).ToList();
                var zeroCount = row.Count(x => x == 0);
                var zeroList = Enumerable.Repeat(0, zeroCount).ToList();

                zeroList.AddRange(filteredRow);

                resultList.AddRange(zeroList);
            }

            Squares = resultList;
        }

        private void MoveLeft()
        {
            var resultList = new List<int>();
            for (int gridIndex = 0; gridIndex < Squares.Count; gridIndex++)
            {
                var row = new List<int>();
                if (gridIndex % GridSize == 0)
                {
                    for (int i = 0; i < GridSize; i++)
                    {
                        row.Add(Squares[gridIndex + i]);
                    }
                }

                var filteredRow = row.Where(x => x != 0).ToList();
                var zeroCount = row.Count(x => x == 0);
                var zeroList = Enumerable.Repeat(0, zeroCount).ToList();

                filteredRow.AddRange(zeroList);

                resultList.AddRange(filteredRow);
            }

            Squares = resultList;
        }

        private void SumRow()
        {
            for (int i = 0; i < Squares.Count - 1; i++)
            {
                if (Squares[i] == Squares[i + 1])
                {
                    var combineResult = Squares[i] + Squares[i + 1];
                    Squares[i] = combineResult;
                    Squares[i + 1] = 0;
                    Score += combineResult;
                }
            }

            CheckWin();
        }

        private void MoveUp()
        {
            for (int gridIndex = 0; gridIndex < GridSize; gridIndex++)
            {
                var column = new List<int>();
                for (int i = 0; i < GridSize; i++)
                {
                    column.Add(Squares[gridIndex + GridSize * i]);
                }

                var filteredColumn = column.Where(x => x != 0).ToList();
                var zeroCount = column.Count(x => x == 0);
                var zeroList = Enumerable.Repeat(0, zeroCount).ToList();
                filteredColumn.AddRange(zeroList);

                for (var i = 0; i < GridSize; i++)
                {
                    Squares[gridIndex + i * GridSize] = filteredColumn[i];
                }
            }

        }

        private void MoveDown()
        {
            for (int gridIndex = 0; gridIndex < GridSize; gridIndex++)
            {
                var column = new List<int>();
                for (int i = 0; i < GridSize; i++)
                {
                    column.Add(Squares[gridIndex + GridSize * i]);
                }

                var filteredColumn = column.Where(x => x != 0).ToList();
                var zeroCount = column.Count(x => x == 0);
                var zeroList = Enumerable.Repeat(0, zeroCount).ToList();
                zeroList.AddRange(filteredColumn);

                for (var i = 0; i < GridSize; i++)
                {
                    Squares[gridIndex + i * GridSize] = zeroList[i];
                }
            }

        }

        private void SumColumn()
        {
            for (var i = 0; i < Squares.Count - GridSize; i++)
            {
                if (Squares[i] == Squares[i + GridSize])
                {
                    var combineResult = Squares[i] + Squares[i + GridSize];
                    Squares[i] = combineResult;
                    Squares[i + GridSize] = 0;
                    Score += combineResult;
                }
            }

            CheckWin();
        }

        private void CheckLose()
        {
            IsLoose = !Squares.Any(x => x == 0);
            if (IsLoose)
            {
                Toast.ToastMessage = "Game over !! You loose.";
                Toast.ToastColor = "bg-danger";
            }
            if (IsWin)
            {
                Toast.ToastMessage = "Congratulation !! You win.";
                Toast.ToastColor = "bg-secondary";
            }
        }

        private void CheckWin()
        {
            IsWin = Squares.Any(x => x >= TargetScore);
        }

        private string GetBgColor(int value)
        {
            if (value >= 2048) return "#EFCB53";
            switch (value)
            {
                case 2:
                    return "#DCD5D3";
                case 4:
                    return "#E3DD8A";
                case 8:
                    return "#F2AC34";
                case 16:
                    return "#F99B19";
                case 32:
                    return "#EE6915";
                case 64:
                    return "#FB4D19";
                case 128:
                    return "#F4D155";
                case 256:
                    return "#EBD045";
                case 512:
                    return "#E5C236";
                case 1024:
                    return "#E3BC16";
                default:
                    return "#A5A5A5";
            }
        }

        private string GetColor(int value)
        {
            if (value == 2) return "#494846";
            else if (value == 4) return "#5E4D3F";
            else if (value == 8) return "#D6D8D6";
            else if (value == 16) return "#E7F8FF";
            else if (value == 32) return "#DCEEFD";
            else if (value == 64) return "#F3E8D4";
            else if (value >= 128) return "#EBEAEA";
            else return "#A5A5A5";
        }

        private void CloseToast()
        {
            CreateBoard();
        }
    }

    public class Toast
    {
        public string ToastMessage { get; set; }
        public string ToastColor { get; set; }
    }
}
