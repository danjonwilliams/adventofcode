using System.Xml.Serialization;

namespace AdventOfCode.Y2022.D02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var input = new[] { "A Y", "B X", "C Z" };
            var input = File.ReadAllLines("input.txt");
            var rounds = input.Select(GetRound).ToList();
            Console.WriteLine($"Part One: {rounds.Select(r => r.GetScore()).Sum()}");

            var part2Rounds = input.Select(GetPart2Round);
            Console.WriteLine($"Part Two: {part2Rounds.Select(r => r.GetScore()).Sum()}");
        }

        static Round GetRound(string roundInput)
        {
            string[] picks = roundInput.Split(' ').ToArray();
            return new Round(GetOpponentPick(picks[0]), GetPlayerPick(picks[1]));
        }

        static Round GetPart2Round(string roundInput)
        {
            string[] picks = roundInput.Split(' ').ToArray();
            var opponentPick = GetOpponentPick(picks[0]);
            var requiredResult = GetRequiredResult(picks[1]);
            var playerPick = GetPlayerPick(opponentPick, requiredResult);
            return new Round(opponentPick, playerPick);
        }

        private static Shape GetPlayerPick(string pick)
        {
            return pick switch
            {
                "X" => Shape.Rock,
                "Y" => Shape.Paper,
                "Z" => Shape.Scissors,
                _ => throw new ArgumentOutOfRangeException(nameof(pick))
            };
        }

        private static Shape GetOpponentPick(string pick)
        {
            return pick switch
            {
                "A" => Shape.Rock,
                "B" => Shape.Paper,
                "C" => Shape.Scissors,
                _ => throw new ArgumentOutOfRangeException(nameof(pick))
            };
        }

        private static Result GetRequiredResult(string result)
        {
            return result switch
            {
                "X" => Result.Lose,
                "Y" => Result.Draw,
                "Z" => Result.Win,
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }

        private static Shape GetPlayerPick(Shape opponentPick, Result requiredResult)
        {
            switch (requiredResult)
            {
                case Result.Lose:
                    return opponentPick switch
                    {
                        Shape.Rock => Shape.Scissors,
                        Shape.Paper => Shape.Rock,
                        Shape.Scissors => Shape.Paper,
                        _ => throw new ArgumentOutOfRangeException(nameof(opponentPick))
                    };
                case Result.Draw:
                    return opponentPick;
                case Result.Win:
                    return opponentPick switch
                    {
                        Shape.Rock => Shape.Paper,
                        Shape.Paper => Shape.Scissors,
                        Shape.Scissors => Shape.Rock,
                        _ => throw new ArgumentOutOfRangeException(nameof(opponentPick))
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(requiredResult));
            }
        }
    }

    internal class Round
    {
        private Shape _oppenentPick;
        private Shape _myPick;

        public Round(Shape opponentPick, Shape myPick)
        {
            _oppenentPick = opponentPick;
            _myPick = myPick;
        }

        public int GetScore()
        {
            int score = (int)_myPick;
            score += (int)GetResult();
            return score;
        }

        private Result GetResult()
        {
            if (_myPick == Shape.Rock)
            {
                if (_oppenentPick == Shape.Paper)
                {
                    return Result.Lose;
                }
                if (_oppenentPick == Shape.Scissors)
                {
                    return Result.Win;
                }
                return Result.Draw;
            }
            else if (_myPick == Shape.Paper)
            {
                if (_oppenentPick == Shape.Scissors)
                {
                    return Result.Lose;
                }
                if (_oppenentPick == Shape.Rock)
                {
                    return Result.Win;
                }
                return Result.Draw;
            }
            else
            {
                if (_oppenentPick == Shape.Rock)
                {
                    return Result.Lose;
                }
                if (_oppenentPick == Shape.Paper)
                {
                    return Result.Win;
                }
                return Result.Draw;
            }
        }
    }

    internal enum Shape
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    internal enum Result
    {
        Lose = 0,
        Draw = 3,
        Win = 6
    }
}