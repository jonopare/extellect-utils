using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Threading.Tasks.Dataflow;

namespace Extellect.Utilities.Execution
{
    /// <summary>
    /// A silly little class to enable proof-of-concept work with the MapReduce pattern.
    /// Override the Map and Reduce methods, then invoke Run with inputs.
    /// I tried a version that used Dataflow but it was built with .NET 4.5 and doesn't 
    /// work so great with .NET 4.0
    /// </summary>
    public abstract class MapReduce<TInput, TKey, TOutput>
    {
        /// <summary>
        /// Map is called once for each input, and returns zero-to-many key value pairs
        /// that go into the ShuffleSort phase.
        /// </summary>
        protected abstract IEnumerable<KeyValuePair<TKey, TOutput>> Map(TInput input);

        /// <summary>
        /// After the ShuffleSort phase is complete, the Reduce function will be called
        /// once for each Key, with zero-to-many Output values associated with that key.
        /// </summary>
        protected abstract KeyValuePair<TKey, TOutput> Reduce(KeyValuePair<TKey, IEnumerable<TOutput>> intermediate);

        /// <summary>
        /// TODO: rewrite it to be asynchronous without using C# 5.0
        /// </summary>
        public Task Run(IEnumerable<TInput> inputs, Action<KeyValuePair<TKey, TOutput>> result)
        {
            var mapPhase = new TaskCompletionSource<bool>();

            var intermediates = new Dictionary<TKey, List<TOutput>>();
            foreach (var input in inputs)
            {
                foreach (var intermediate in Map(input))
                {
                    List<TOutput> values;
                    if (!intermediates.TryGetValue(intermediate.Key, out values))
                    {
                        intermediates.Add(intermediate.Key, values = new List<TOutput>());
                    }
                    values.Add(intermediate.Value);
                }
            }

            mapPhase.SetResult(true);

            return mapPhase.Task.ContinueWith(_ =>
            {
                var reducePhase = new TaskCompletionSource<bool>();

                foreach (var intermediate in intermediates)
                {
                    result(Reduce(new KeyValuePair<TKey, IEnumerable<TOutput>>(intermediate.Key, intermediate.Value)));
                }

                reducePhase.SetResult(true);

                return reducePhase.Task;
            });

            //throw new NotImplementedException();
            //var map = new TransformBlock<TInput, IEnumerable<KeyValuePair<TKey, TOutput>>>(new Func<TInput, IEnumerable<KeyValuePair<TKey, TOutput>>>(Map));

            //var intermediates = new Dictionary<TKey, IEnumerable<TOutput>>();
            //var shuffle = new ActionBlock<IEnumerable<KeyValuePair<TKey, TOutput>>>(x =>
            //{
            //    foreach (var intermediate in x)
            //    {
            //        IEnumerable<TOutput> values;
            //        if (!intermediates.TryGetValue(intermediate.Key, out values))
            //        {
            //            intermediates.Add(intermediate.Key, values = new List<TOutput>());
            //        }
            //        ((List<TOutput>)values).Add(intermediate.Value);
            //    }
            //});

            //map.LinkTo(shuffle, new DataflowLinkOptions { PropagateCompletion = true });

            //foreach (var input in inputs)
            //{
            //    map.Post(input);
            //}
            //map.Complete();

            //shuffle.Completion.Wait();

            //var reduce = new TransformBlock<KeyValuePair<TKey, IEnumerable<TOutput>>, KeyValuePair<TKey, TOutput>>(new Func<KeyValuePair<TKey, IEnumerable<TOutput>>, KeyValuePair<TKey, TOutput>>(Reduce));
            //var notifyResult = new ActionBlock<KeyValuePair<TKey, TOutput>>(new Action<KeyValuePair<TKey,TOutput>>(result));

            //reduce.LinkTo(notifyResult, new DataflowLinkOptions { PropagateCompletion = true });

            //foreach (var intermediate in intermediates)
            //{
            //    reduce.Post(intermediate);
            //}
            //reduce.Complete();

            //return notifyResult.Completion;
        }
    }
}
