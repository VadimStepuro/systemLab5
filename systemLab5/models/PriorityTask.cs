using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace systemLab5.models
{
    public class PriorityTask
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public int Duration { get; set; }

        public int WaitingTime { get; set; }
        public int ActivisationCount { get; set; }
        public int LastActiveTime { get; set; }

        public PriorityTask() { }

        public PriorityTask(int id, int priority, int duration) { Id = id; Priority = priority; Duration = duration; }

        public virtual async Task IntitalizeTask(int duration)
        {
            Duration -= duration;
            await Task.Run(() =>
            {
                Thread.Sleep(duration * 1000);
            });
        }

        public override string ToString()
        {
            return $"Priority: Id: {Id}, Priority: {Priority}, Duration: {Duration}, Waiting time: {WaitingTime}, Activisation count: {ActivisationCount}, Last Active Time: {LastActiveTime}";
        }
    }
}
