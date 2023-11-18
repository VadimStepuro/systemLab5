using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace systemLab5.models
{
    public class GuaranteeTask
    {
        public int Id { get; set; }
        public int Duration { get; set; }
        public double P { get; set; }
        public int ActiveTime { get; set; }

        public int WaitingTime { get; set; }
        public int ActivisationCount { get; set; }
        public int LastActiveTime { get; set; }

        public GuaranteeTask(int id, int duration) {
            Id = id;
            Duration = duration;
        }

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
            return $"Guarantee Task: Id: {Id}, Duration: {Duration}, P: {P}, Active time: {ActiveTime}, Waiting time: {WaitingTime}, Activisation count: {ActivisationCount}, Last Active Time: {LastActiveTime}";
        }
    }
}
