using System;
using System.Collections.Generic;
namespace WaterJugChallenge.Services
{
    public class WaterJugSolver
    {
        public WaterJugSolution Solve(int jugCapacityX, int jugCapacityY, int targetAmountZ)
        {

            //Se comprueba si es posible medir la cantidad de agua deseada
            if (targetAmountZ > jugCapacityX && targetAmountZ > jugCapacityY)
            {
                return new WaterJugSolution { Steps = new List<WaterJugStep> { new WaterJugStep { Action = "No Solution" } } };
            }
            if (targetAmountZ % CalculateGCD(jugCapacityX, jugCapacityY) != 0)
            {
                return new WaterJugSolution { Steps = new List<WaterJugStep> { new WaterJugStep { Action = "No Solution" } } };
            }

            //Busco 2 soluciones o 2 caminos posibles
            WaterJugSolution solution1 = BreadthFirstSearch(jugCapacityX, jugCapacityY, targetAmountZ);
            WaterJugSolution solution2 = BreadthFirstSearch(jugCapacityY, jugCapacityX, targetAmountZ);

            //Retorno la menor solucion o menor camino
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

        // calculo el máximo comun divisor de 'a' y 'b'
        private int CalculateGCD(int a, int b)
        {
            if (b == 0)
                return a;
            return CalculateGCD(b, a % b);
        }

        // Realizo una busqueda en amplitud para encontrar el camino mas corto
        private WaterJugSolution BreadthFirstSearch(int jugCapacityX, int jugCapacityY, int targetAmountZ)
        {

            //se crea una cola para almacenar los estados
            Queue<WaterJugStep> queue = new Queue<WaterJugStep>();

            //se crea un conjunto para almacenar los estados visitados
            HashSet<(int, int)> visited = new HashSet<(int, int)>();

            //se agrega el estado inicial a la cola y al conjunto de visitados
            queue.Enqueue(new WaterJugStep { JugXAmount = 0, JugYAmount = 0 });
            visited.Add((0, 0));

            while (queue.Count > 0)
            {
                //se obtiene el siguiente estado de la cola
                WaterJugStep currentStep = queue.Dequeue();
                //Console.WriteLine("currentStep en X"+ currentStep.JugXAmount);
                //Console.WriteLine("currentStep en Y" + currentStep.JugYAmount);

                // Si este estado es la solucion, devuelve la secuencia de pasos
                if (currentStep.JugXAmount == targetAmountZ || currentStep.JugYAmount == targetAmountZ)
                {
                    return new WaterJugSolution { Steps = GetStepsFrom(currentStep) };
                }

                //se genera todos los posibles siguientes estados
                foreach (WaterJugStep nextStep in GenerateNextSteps(currentStep, jugCapacityX, jugCapacityY))
                {
                // Si el estado siguiente no ha sido visitado, lo agrego a la cola y al conjunto de visitados
                    if (!visited.Contains((nextStep.JugXAmount, nextStep.JugYAmount)))
                    {
                        queue.Enqueue(nextStep);
                        visited.Add((nextStep.JugXAmount, nextStep.JugYAmount));
                    }
                }
            }
                 // Si no se encontró ninguna solucion, retorna una secuencia vacia
            return new WaterJugSolution { Steps = new List<WaterJugStep>() };
        }

        private IEnumerable<WaterJugStep> GenerateNextSteps(WaterJugStep currentStep, int jugCapacityX, int jugCapacityY)
        {
            // Llenar el jarro X
            yield return new WaterJugStep { JugXAmount = jugCapacityX, JugYAmount = currentStep.JugYAmount, Action = "FillJugX", PreviousStep = currentStep };
            //yield return new WaterJugStep { JugXAmount = currentStep.JugXAmount, JugYAmount = jugCapacityY, Action = "FillJugX", PreviousStep = currentStep };
            // Llenar el jarro Y
            yield return new WaterJugStep { JugXAmount = currentStep.JugXAmount, JugYAmount = jugCapacityY, Action = "FillJugY", PreviousStep = currentStep };
            //yield return new WaterJugStep { JugXAmount = jugCapacityX, JugYAmount = currentStep.JugYAmount, Action = "FillJugY", PreviousStep = currentStep };
            // Vaciar el jarro X
            yield return new WaterJugStep { JugXAmount = 0, JugYAmount = currentStep.JugYAmount, Action = "EmptyJugX", PreviousStep = currentStep };
            // Vaciar el jarro Y
            yield return new WaterJugStep { JugXAmount = currentStep.JugXAmount, JugYAmount = 0, Action = "EmptyJugY", PreviousStep = currentStep };
            
            // Transferir del jarro X al jarro Y
            int transferAmount = Math.Min(currentStep.JugXAmount, jugCapacityY - currentStep.JugYAmount);
            yield return new WaterJugStep { JugXAmount = currentStep.JugXAmount - transferAmount, JugYAmount = currentStep.JugYAmount + transferAmount, Action = "TransferXToY", PreviousStep = currentStep };
            // Transferir del jarro Y al jarro X
            transferAmount = Math.Min(currentStep.JugYAmount, jugCapacityX - currentStep.JugXAmount);
            yield return new WaterJugStep { JugXAmount = currentStep.JugXAmount + transferAmount, JugYAmount = currentStep.JugYAmount - transferAmount, Action = "TransferYToX", PreviousStep = currentStep };
            


            // limita los valores a las capacidades maximas de los jarrones
            yield return new WaterJugStep { JugXAmount = Math.Min(jugCapacityX, currentStep.JugXAmount + currentStep.JugYAmount), JugYAmount = Math.Max(0, currentStep.JugXAmount + currentStep.JugYAmount - jugCapacityX), Action = "TransferYToX", PreviousStep = currentStep };
            yield return new WaterJugStep { JugXAmount = Math.Max(0, currentStep.JugXAmount + currentStep.JugYAmount - jugCapacityY), JugYAmount = Math.Min(jugCapacityY, currentStep.JugXAmount + currentStep.JugYAmount), Action = "TransferXToY", PreviousStep = currentStep };
        }

    }

    public class WaterJugSolution
    {
        public List<WaterJugStep> Steps { get; set; }
        // Propiedad Count que devuelve la cantidad de pasos en la solución
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
