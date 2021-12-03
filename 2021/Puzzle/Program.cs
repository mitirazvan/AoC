using System;
using System.Collections.Generic;
using System.IO;

namespace Puzzle
{

    class Program
    {
        static void Main(string[] args)
        {
            var result = 0;

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
            Console.WriteLine("Calculate Oxygen: ");
            var oxygen = calulateValues(inputs, 1, inputs[0].Length - 1, true);
            Console.WriteLine("Calculate CO2: ");
            var co2 = calulateValues(inputs, 1, inputs[0].Length - 1, false);
            Console.WriteLine($"Answer: {oxygen} * {co2} = {oxygen * co2}  ");
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
