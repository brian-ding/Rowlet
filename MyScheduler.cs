using DotnetSpider.Downloader;
using DotnetSpider.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rowlet
{
    public class MyScheduler : IScheduler
    {
        private Queue<Request> _queue = new Queue<Request>();

        public int Total => _queue.Count;

        public Request[] Dequeue(string ownerId, int count = 1)
        {
            if (Total < count)
            {
                return null;
            }

            Request[] requests = new Request[count];
            for (int i = 0; i < count; i++)
            {
                requests[i] = _queue.Dequeue();
            }

            return requests;
        }

        public int Enqueue(IEnumerable<Request> requests)
        {
            foreach (var item in requests)
            {
                item.ComputeHash();
                _queue.Enqueue(item);
            }

            return Total;
        }
    }
}
