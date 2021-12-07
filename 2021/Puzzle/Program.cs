using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puzzle
{

    class Program
    {
        static void Main(string[] args)
        {
            var result = 0.0;

            var inputs = ReadFile<string>();

            // day 1
            // var newList = groupBy3(inputs);
            // var result = doWork(newList);

            // day 2
            // var finalPosition = calculateFinalPosition(inputs);

            // day 3 #1
            // result = calulatePowerConsumption(inputs);
            // Console.WriteLine($"Answer: {result}");

            // day 3 #2            
            //Console.WriteLine("Calculate Oxygen: ");
            //var oxygen = calulateValues(inputs, 1, inputs[0].Length - 1, true);
            //Console.WriteLine("Calculate CO2: ");
            //var co2 = calulateValues(inputs, 1, inputs[0].Length - 1, false);
            //Console.WriteLine($"Answer: {oxygen} * {co2} = {oxygen * co2}  ");

            // day 4 #1 #2
            // result = getFinalScore(inputs);


            // day 5 #1 #2
            // result = CalculateIntersections(inputs);

            // day 6 #1 #2
            // result = spawnLanternFish(inputs, 256);

            // day 7 #1 #2
            result = calulateFuel(inputs);


            Console.WriteLine($"Answer: {result}");
        }

        // day 7
        private static long calulateFuel(List<string> inputs)
        {
            var pos = inputs[0].Split(",").Select(x => Convert.ToInt32(x)).ToList();
            var max = pos.Max();

            int sum = 0, minSum = int.MaxValue;
            for (int i = 1; i <= max; i++)
            {
                foreach (var p in pos)
                {
                    var n = Math.Abs(p - i);
                    // #1
                    //sum += n;

                    // #2
                    sum += (n * (n + 1)) / 2;
                }

                if (minSum > sum)
                    minSum = sum;
                sum = 0;
            }

            return minSum;
        }



        // day 6
        private static long spawnLanternFish(List<string> inputs, int daysCount)
        {
            // get initial school 
            var fishList = inputs[0].Split(",").Select(x => Convert.ToByte(x)).ToList();
            Console.WriteLine($"Initial state: {inputs[0]}");

            List<long> days = new List<long>(new long[10]);
            foreach (var f in fishList)
            {
                days[f]++;
            }

            for (int i = 1; i <= daysCount; i++)
            {
                List<long> extra = new List<long>(new long[10]);

                // substract 1 day
                for (int x = 1; x < 10; x++)
                {
                    extra[x - 1] += days[x];
                }

                extra[6] += days[0];
                extra[8] += days[0];

                for (int x = 0; x < 10; x++)
                {
                    days[x] = extra[x];
                }
            }
            return days.Sum();
        }

        // day 5
        struct Point
        {
            public int x, y;
        }

        struct Line
        {
            public Point Start, End;
        }

        private static int CalculateIntersections(List<string> inputs)
        {

            var res = DecodeInput(inputs);
            var board = new int[res.Item2][];
            InitArray(res.Item2, ref board);

            foreach (var line in res.Item1)
            {
                if (line.Start.x == line.End.x)  // straight vertical lines
                {
                    for (int i = Math.Min(line.Start.y, line.End.y); i <= Math.Max(line.Start.y, line.End.y); i++)
                        board[i][line.Start.x]++;
                }
                else if (line.Start.y == line.End.y)  // straight horizontal lines
                {
                    for (int i = Math.Min(line.Start.x, line.End.x); i <= Math.Max(line.Start.x, line.End.x); i++)
                        board[line.Start.y][i]++;
                }

                // day 5 #2
                // Diagonal lines
                else if (Math.Abs(line.Start.x - line.End.x) == Math.Abs(line.Start.y - line.End.y))
                {
                    board[line.Start.y][line.Start.x]++;
                    board[line.End.y][line.End.x]++;

                    // calculate in-between points
                    var distance = Math.Abs(line.Start.x - line.End.x);

                    var diff_X = line.End.x - line.Start.x;
                    var diff_Y = line.End.y - line.Start.y;

                    var interval_X = diff_X / distance;
                    var interval_Y = diff_Y / distance;

                    for (int i = 1; i <= distance - 1; i++)
                    {
                        var x = line.Start.x + interval_X * i;
                        var y = line.Start.y + interval_Y * i;
                        board[y][x]++;
                    }
                }
            }

            return WriteOutput(res.Item2, board);
        }
        private static void InitArray(int dimension, ref int[][] board)
        {
            for (int i = 0; i < dimension; i++)
            {
                var arr = new int[dimension];
                for (int j = 0; j < dimension; j++)
                {
                    arr[j] = 0;
                }
                board[i] = arr;
            }
        }
        private static int WriteOutput(int dimension, int[][] board)
        {
            int intersections = 0;
            for (int i = 0; i < dimension; i++)
            {
                //oldM = max;
                for (int j = 0; j < dimension; j++)
                {
                    if (dimension < 20)
                    {
                        var printChar = (board[i][j] == 0 ? "." : board[i][j].ToString());
                        Console.Write(printChar);
                    }

                    if (board[i][j] >= 2)
                    {
                        intersections++;
                    }
                }
                Console.WriteLine($"   -> I:{intersections}");
            }
            return intersections;
        }

        private static Tuple<List<Line>, int> DecodeInput(List<string> inputs)
        {
            // 0,9 -> 5,9
            List<Line> lines = new List<Line>();
            int max = 0;
            foreach (var inp in inputs)
            {
                var line = inp.Split("->");
                var start = line[0];
                var end = line[1];

                Point stPoint = new Point
                {
                    x = Convert.ToInt32(start.Split(',')[0]),
                    y = Convert.ToInt32(start.Split(',')[1])
                };

                Point endPoint = new Point
                {
                    x = Convert.ToInt32(end.Split(',')[0]),
                    y = Convert.ToInt32(end.Split(',')[1])
                };

                max = Math.Max(max, Math.Max(Math.Max(stPoint.x, stPoint.y), Math.Max(endPoint.x, endPoint.y)));

                lines.Add(new Line { Start = stPoint, End = endPoint });
            }
            return new Tuple<List<Line>, int>(lines, max + 1);
        }

        // day 4 #1 #2

        struct LineStruct
        {
            public bool LineCompleted()
            {
                if (Line == null)
                    return false;
                return Line.Count == 0;
            }

            public bool MarkNumber(int nr)
            {
                return Line.Remove(nr);
            }

            public List<int> Line;
        }

        struct BoardStruct
        {
            public int Index;

            public int Dimension;

            public int Sum()
            {
                if (Lines == null)
                    return 0;

                int sum = 0;
                for (int b = 0; b < Dimension; b++)
                    foreach (var nr in Lines[b].Line)
                    {
                        sum += nr;
                    }
                return sum;
            }

            public bool IsBingo(int nr)
            {
                bool lineComplete = false;
                foreach (var line in Lines)
                {
                    line.MarkNumber(nr);
                    lineComplete = lineComplete || line.LineCompleted();
                }
                return lineComplete;
            }

            // Lines
            public List<LineStruct> Lines;
        }

        private static int getFinalScore(List<string> inputs)
        {
            if (inputs.Count == 0) return 0;

            // Read first line with selected numbers
            var drawNrs = inputs[0].Split(",").Select(x => Convert.ToInt32(x)).ToList();
            inputs.RemoveAt(0);

            List<BoardStruct> boards = new List<BoardStruct>();

            var count = inputs.Count;
            int i = 0, boardIndex = 0;

            // read boards. each board is separated by an empty line.
            while (i < count)
            {
                // skip empty rows
                if (string.IsNullOrEmpty(inputs[i])) { i++; continue; }


                var board = new BoardStruct();

                board.Index = boardIndex++;
                board.Lines = new List<LineStruct>();

                while (true)    // read board lines
                {
                    var lineStr = new LineStruct() { Line = new List<int>() };
                    lineStr.Line = inputs[i].Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList();

                    board.Lines.Add(lineStr);

                    i++;
                    if (i == count || string.IsNullOrEmpty(inputs[i]))
                        break;
                }

                // Add columns lines
                var cols = board.Lines[0].Line.Count;
                board.Dimension = cols;

                for (int j = 0; j < cols; j++)
                {
                    var lineStr = new LineStruct() { Line = new List<int>() };
                    for (int k = 0; k < cols; k++)
                    {
                        lineStr.Line.Add(board.Lines[k].Line[j]);
                    }
                    board.Lines.Add(lineStr);
                }
                boards.Add(board);
            }

            List<Tuple<BoardStruct, int>> wins = new List<Tuple<BoardStruct, int>>();
            foreach (var nr in drawNrs)
            {
                // check if it is bingo
                foreach (var board in boards)
                {
                    if (board.IsBingo(nr))
                    {
                        // #1
                        //return board.Sum() * nr;

                        // #2
                        // Add boards only once...
                        if (!wins.Exists(x => x.Item1.Index == board.Index))
                            wins.Add(new Tuple<BoardStruct, int>(board, board.Sum() * nr));
                    }
                }
            }
            //#1
            // return 0;

            // #2
            return wins.Last().Item2;
        }


        // day 3 #2
        private static int calulateValues(List<string> inputs, int selector, int position, bool isOxygen)
        {
            if (inputs.Count == 1)
                return Convert.ToInt32(inputs[0], 2);

            // determine selector:
            selector = determineSelector(inputs, position, isOxygen);

            List<string> newInputs = new List<string>();
            foreach (var inp in inputs)
            {
                int bin = Convert.ToInt32(inp, 2);
                int mask = (int)Math.Pow(2, position);
                if ((bin & mask) >> position == selector)
                {
                    Console.WriteLine($"{inp} - pos {position}");
                    newInputs.Add(inp);
                }
            }
            position--;

            return calulateValues(newInputs, selector, position, isOxygen);
        }

        private static int determineSelector(List<string> inputs, int position, bool isOxygen)
        {
            int selector = 0;
            foreach (var inp in inputs)
            {
                int bin = Convert.ToInt32(inp, 2);
                int mask = (int)Math.Pow(2, position);
                selector += (bin & mask) >> position;
            }
            if (isOxygen)
                return (selector >= inputs.Count - selector) ? 1 : 0;
            else
                return (selector >= inputs.Count - selector) ? 0 : 1;
        }

        // day 3 #1
        private static int calulatePowerConsumption(List<string> inputs)
        {
            // do the gama rate                              
            var gammaRate = 0b0;
            var epsilonRate = 0b0;

            var infolen = inputs[0].Length;
            List<int> bits = new List<int>(new int[infolen]);

            var length = inputs.Count;
            foreach (var inp in inputs)
            {
                int bin = Convert.ToInt32(inp, 2);
                for (int i = 0; i < infolen; i++)
                {
                    int shift = infolen - i - 1;
                    int mask = (int)Math.Pow(2, shift);

                    bits[i] = bits[i] + ((bin & mask) >> shift);
                }
            }

            var gammaStr = string.Empty;
            var gammaStrMask = string.Empty;
            for (int i = 0; i < bits.Count; i++)
            {
                bits[i] = (bits[i] > length / 2) ? 1 : 0;
                gammaStr += bits[i].ToString();
                gammaStrMask += "1";
            }

            //var gammaStr = $"{bit0}{bit1}{bit2}{bit3}{bit4}";
            gammaRate = Convert.ToInt32(gammaStr, 2);
            Console.WriteLine($"Gamma: {gammaRate}");

            var gammaMask = Convert.ToInt32(gammaStrMask, 2);
            epsilonRate = gammaRate ^ gammaMask;
            Console.WriteLine($"Epsilon: {epsilonRate}");

            return gammaRate * epsilonRate;
        }

        // day 2
        private static int calculateFinalPosition(List<string> inputs)
        {
            var horiz = 0;
            var depth = 0;
            var aim = 0;
            foreach (var mvInstr in inputs)
            {
                var instr = mvInstr.Split(' ');
                var move = instr[0];
                var value = int.Parse(instr[1]);

                switch (move)
                {
                    case "forward":
                        {
                            horiz += value;
                            depth += (aim * value);
                            break;
                        }
                    case "down":
                        {
                            aim += value;
                            break;
                        }
                    case "up":
                        {
                            aim -= value;
                            break;
                        }
                }
            }
            return horiz * depth;
        }

        // day 1
        private static List<int> groupBy3(List<int> inputs)
        {
            List<int> newList = new List<int>();
            var len = inputs.Count;
            Console.WriteLine("Sum numbers: ");
            for (var i = 0; i < len - 2; i++)
            {
                var sum = inputs[i] + inputs[i + 1] + inputs[i + 2];
                newList.Add(sum);
                Console.WriteLine(sum);
            }
            return newList;
        }
        private static string doWork(List<int> inputs)
        {
            var count = 0;
            var len = inputs.Count;
            for (var i = 0; i < len - 1; i++)
            {
                var currentDepth = inputs[i];
                var nextDepth = inputs[i + 1];

                if (nextDepth > currentDepth)
                    count++;
            }
            return count.ToString();
        }

        // tools
        private static List<T> ReadFile<T>() where T : IConvertible
        {
            List<T> inputs = new List<T>();
            var count = 0;
            try
            {
                foreach (object line in File.ReadLines("input.txt"))
                {
                    Console.WriteLine($"{count++}. " + line);
                    if (typeof(T) == typeof(int))
                    {
                        inputs.Add((T)Convert.ChangeType(line, typeof(T)));
                    }
                    else
                        inputs.Add((T)line);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return inputs;
        }

    }
}
