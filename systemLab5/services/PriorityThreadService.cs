using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using systemLab5.models;

namespace systemLab5.services
{
    public delegate void Logging(string message);


    public static class PriorityThreadService
    {
        private static Logging logger;
        private static int quantum;
        private static bool isStarted;

        private static List<PriorityTask> priorityTasks = new();
        private static List<PriorityTask> endedTasks = new();


        public static void SetQuantum(int _quantum)
        {
            quantum = _quantum;
        }

        public static void SetLogger(Logging logging)
        {
            logger += logging;
        }

        public static void ChangeState() { isStarted= !isStarted; }

        public static void SetPriorityTasks(List<(int,int)> values)
        {
            for(int i = 0; i < values.Count; i++)
            {
                priorityTasks.Add(new PriorityTask(i, values[i].Item1, values[i].Item2));
            }
            
        }

        private static void LogStatistics()
        {
            foreach(PriorityTask task in endedTasks) {
                logger.Invoke(task.ToString() + '\n');
            }
        }

        public static async void StartTasks()
        {
            int currentTime = 0;

            while(priorityTasks.Count > 0)
            {
                if (isStarted)
                {
                    PriorityTask nextTask = priorityTasks.OrderByDescending(task => task.Priority).ThenBy(task => task.LastActiveTime).First();
                    if (nextTask != null)
                    {
                        int duration = Math.Min(quantum, nextTask.Duration);

                        nextTask.ActivisationCount++;

                        if (nextTask.ActivisationCount > 1)
                        {
                            nextTask.WaitingTime += currentTime - nextTask.LastActiveTime;
                        }
                        else
                        {
                            nextTask.WaitingTime = currentTime;
                        }

                        currentTime += duration;

                        nextTask.LastActiveTime = currentTime;

                        logger.Invoke($"Starting task {nextTask.Id}\n");

                        await nextTask.IntitalizeTask(duration);

                        logger.Invoke($"Ending task {nextTask.Id} current time: {currentTime}\n\n");

                        if (nextTask.Duration <= 0)
                        {
                            endedTasks.Add(nextTask);
                            priorityTasks.Remove(nextTask);
                        }
                    }
                }
            }

            LogStatistics();

        }
    }
}
