using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using systemLab5.models;

namespace systemLab5.services
{ 
    public static class GuaranteeTaskService
    {
        private static int quant;
        private static Logging logger;
        private static bool isStarted;
        private static int numOfTasks;

        private static List<GuaranteeTask> tasks = new();
        private static List<GuaranteeTask> endedTasks = new();

        public static void SetQuant(int _quant)
        {
            quant = _quant;
        }

        public static void SetLogger(Logging _logger)
        {
            logger = _logger;
        }

        public static void ChangeState()
        {
            isStarted = !isStarted;
        }

        public static void SetTasks(List<int> times)
        {
            numOfTasks = times.Count;
            for(int i = 0; i < times.Count; i++)
            {
                tasks.Add(new GuaranteeTask(i, times[i]));
            }
        }

        private static void LogStatistics()
        {
            foreach (GuaranteeTask task in endedTasks)
            {
                logger.Invoke(task.ToString() + '\n');
            }
        }

        public static async void StartTasks()
        {
            int currentTime = 0;

            while(tasks.Count> 0)
            {
                if(isStarted)
                {
                    GuaranteeTask nextTask = tasks.OrderBy(task => task.P).First();

                    if(nextTask != null)
                    {
                        int duration = Math.Min(quant, nextTask.Duration);

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

                        nextTask.ActiveTime += duration;



                        foreach(GuaranteeTask task in tasks)
                        {
                            logger.Invoke(task.ActiveTime.ToString() + " " + currentTime + " " + numOfTasks + '\n');

                            task.P = task.ActiveTime/ ((double)currentTime / numOfTasks);
                        }
                        logger.Invoke($"Starting task {nextTask.Id}\n");

                        await nextTask.IntitalizeTask(duration);

                        logger.Invoke($"Ending task {nextTask.Id}, current P: {nextTask.P} current time: {currentTime}\n\n");

                        if (nextTask.Duration <= 0)
                        {
                            endedTasks.Add(nextTask);
                            tasks.Remove(nextTask);
                        }

                    }
                }
            }
            LogStatistics();
        }
    }
}
