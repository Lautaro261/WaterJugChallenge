using System;
using System.Collections.Generic;
namespace WaterJugChallenge.Services
{
    public class WaterJugSolver
    {
        public WaterJugSolution Solve(int jugCapacityX, int jugCapacityY, int targetAmountZ)
        {

            //It is checked whether it is possible to measure the desired amount of water
            if (targetAmountZ > jugCapacityX && targetAmountZ > jugCapacityY)
            {
                return new WaterJugSolution { Steps = new List<WaterJugStep> { new WaterJugStep { Action = "No Solution" } } };
            }
            if (targetAmountZ % CalculateGCD(jugCapacityX, jugCapacityY) != 0)
            {
                return new WaterJugSolution { Steps = new List<WaterJugStep> { new WaterJugStep { Action = "No Solution" } } };
            }

            //It searches for 2 solutions or 2 possible paths
            WaterJugSolution solution1 = BreadthFirstSearch(jugCapacityX, jugCapacityY, targetAmountZ);
            WaterJugSolution solution2 = BreadthFirstSearch(jugCapacityY, jugCapacityX, targetAmountZ);

            //returns the smallest value of the solution
            return solution1.Steps.Count < solution2.Steps.Count ? solution1 : solution2;
        }

        private List<WaterJugStep> GetStepsFrom(WaterJugStep step)
        {
            var steps = new List<WaterJugStep>();
            while (step != null)
            {
                steps.Add(step);
                step = step.PreviousStep;
            }
            steps.Reverse();
            return steps;
        }

        // Calculates the greatest common divisor of 'a' and 'b'
        private int CalculateGCD(int a, int b)
        {
            if (b == 0)
                return a;
            return CalculateGCD(b, a % b);
        }

        // performs a breadth-first search to find the shortest path
        private WaterJugSolution BreadthFirstSearch(int jugCapacityX, int jugCapacityY, int targetAmountZ)
        {

            // Creates a queue to store the states
            Queue<WaterJugStep> queue = new Queue<WaterJugStep>();

            //Creates a set to store the visited states
            HashSet<(int, int)> visited = new HashSet<(int, int)>();

            //adds the initial state to the queue and the set of visited states
            queue.Enqueue(new WaterJugStep { JugXAmount = 0, JugYAmount = 0 });
            visited.Add((0, 0));

            while (queue.Count > 0)
            {
                //gets the next state from the queue
                WaterJugStep currentStep = queue.Dequeue();

                // if this state is the solution, it returns the sequence of steps
                if (currentStep.JugXAmount == targetAmountZ || currentStep.JugYAmount == targetAmountZ)
                {
                    return new WaterJugSolution { Steps = GetStepsFrom(currentStep) };
                }

                //generates all possible next states
                foreach (WaterJugStep nextStep in GenerateNextSteps(currentStep, jugCapacityX, jugCapacityY))
                {
                    // if the next state has not been visited, add it to the queue and the set of visited states
                    if (!visited.Contains((nextStep.JugXAmount, nextStep.JugYAmount)))
                    {
                        queue.Enqueue(nextStep);
                        visited.Add((nextStep.JugXAmount, nextStep.JugYAmount));
                    }
                }
            }
            // if no solution was found, it returns an empty sequence
            return new WaterJugSolution { Steps = new List<WaterJugStep>() };
        }

        private IEnumerable<WaterJugStep> GenerateNextSteps(WaterJugStep currentStep, int jugCapacityX, int jugCapacityY)
        {
            // Fill jug X
            yield return new WaterJugStep { JugXAmount = jugCapacityX, JugYAmount = currentStep.JugYAmount, Action = "FillJugX", PreviousStep = currentStep };
            //yield return new WaterJugStep { JugXAmount = currentStep.JugXAmount, JugYAmount = jugCapacityY, Action = "FillJugX", PreviousStep = currentStep };
            // Fill jug Y
            yield return new WaterJugStep { JugXAmount = currentStep.JugXAmount, JugYAmount = jugCapacityY, Action = "FillJugY", PreviousStep = currentStep };
            //yield return new WaterJugStep { JugXAmount = jugCapacityX, JugYAmount = currentStep.JugYAmount, Action = "FillJugY", PreviousStep = currentStep };
            // Empty jug X.
            yield return new WaterJugStep { JugXAmount = 0, JugYAmount = currentStep.JugYAmount, Action = "EmptyJugX", PreviousStep = currentStep };
            // Empty jug Y
            yield return new WaterJugStep { JugXAmount = currentStep.JugXAmount, JugYAmount = 0, Action = "EmptyJugY", PreviousStep = currentStep };

            // TransferXToY
            int transferAmount = Math.Min(currentStep.JugXAmount, jugCapacityY - currentStep.JugYAmount);
            yield return new WaterJugStep { JugXAmount = currentStep.JugXAmount - transferAmount, JugYAmount = currentStep.JugYAmount + transferAmount, Action = "TransferXToY", PreviousStep = currentStep };
            // TransferYToX
            transferAmount = Math.Min(currentStep.JugYAmount, jugCapacityX - currentStep.JugXAmount);
            yield return new WaterJugStep { JugXAmount = currentStep.JugXAmount + transferAmount, JugYAmount = currentStep.JugYAmount - transferAmount, Action = "TransferYToX", PreviousStep = currentStep };



            // limit the values to the maximum capacities of the jug
            yield return new WaterJugStep { JugXAmount = Math.Min(jugCapacityX, currentStep.JugXAmount + currentStep.JugYAmount), JugYAmount = Math.Max(0, currentStep.JugXAmount + currentStep.JugYAmount - jugCapacityX), Action = "TransferYToX", PreviousStep = currentStep };
            yield return new WaterJugStep { JugXAmount = Math.Max(0, currentStep.JugXAmount + currentStep.JugYAmount - jugCapacityY), JugYAmount = Math.Min(jugCapacityY, currentStep.JugXAmount + currentStep.JugYAmount), Action = "TransferXToY", PreviousStep = currentStep };
        }

    }

    public class WaterJugSolution
    {
        public List<WaterJugStep> Steps { get; set; }
        // property Count that returns the number of steps in the solution
        public int Count
        {
            get
            {
                return Steps != null ? Steps.Count : 0;
            }
        }
    }

    public class WaterJugStep
    {
        public int JugXAmount { get; set; }
        public int JugYAmount { get; set; }
        public string Action { get; set; }
        public WaterJugStep PreviousStep { get; set; }
    }
}
