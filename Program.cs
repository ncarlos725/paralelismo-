using System;
using System.Threading;
  
namespace EncontrarMayor
{
    class Program
    {
        static int[] numeros = new int[30000]; // array de entero destinado almacenar N° aleatorios
        static int numeroDeHilos = 2;// Numero de hilos que se utilizarán.
        static int NumeroMax = int.MinValue; // Se le asigna con int.MinValue el N° mas pequeño para luego compararlo y así obtener el N° Mas grande
        static object lockObject = new object(); // Objeto de bloqueo para la actualización segura de maxNumber.
        static void Main(string[] args)
        {
            Random random = new Random();

            // Llenar el array con números aleatorios.
            for (int i = 0; i < numeros.Length; i++)
            {
                numeros[i] = random.Next(45000);
            }

            // Crear y lanzar los hilos.
            Thread[] threads = new Thread[numeroDeHilos];

            for (int i = 0; i < numeroDeHilos; i++)
            {
                int IndiceInicio = i * (numeros.Length / numeroDeHilos); // Calcular el índice de inicio del array.
                int indiceFinal = (i+1) * (numeros.Length/ numeroDeHilos); // Calcular el índice de final del array.

                threads[i] = new Thread(() => BuscarMax(IndiceInicio, indiceFinal)); // Crear un hilo para procesar una porción del array.
                threads[i].Start();
            }

            // Esperar a que todos los hilos finalicen.
            foreach(Thread thread in threads)
            {
                thread.Join();
            }

            // El número máximo se encuentra en la varible "maxNumber".
            Console.WriteLine("El número máximo es: " + NumeroMax);
        }

        static void BuscarMax (int startIndex, int endIndex)
        {
            int localMax = int.MinValue; // Inicializar la variable local para el número máximo.

            for (int i = startIndex; i < endIndex; i++)
            {
                if (numeros[i] > localMax)
                {
                    localMax = numeros[i]; // Actualizar localMax si se encuentra un numero mayor.
                }
            }

            // Utilizar lock para actulizar la variable compartida "maxNumber" de manera segura.
            lock (lockObject)
            {
                if(localMax > NumeroMax)
                {
                    NumeroMax = localMax; // Actulizar numeroMax si localMax es mayor.
                }
            }
        }
    }
}