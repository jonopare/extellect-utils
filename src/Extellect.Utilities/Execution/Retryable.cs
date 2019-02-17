#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Execution
{
    public class Retryable
    {
        public static void Retry(Action action, int maxRetries, Action<Exception, int> onRetry, params Func<Exception, bool>[] canRetryPredicates)
        {
            var retries = 0;
            do
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception e)
                {
                    if (retries++ < maxRetries)
                    {
                        var canRetry = false;
                        foreach (var canRetryPredicate in canRetryPredicates)
                        {
                            if (canRetryPredicate(e))
                            {
                                canRetry = true;
                                onRetry(e, retries - 1);
                                break;
                            }
                        }
                        if (canRetry)
                        {
                            continue;
                        }
                    }
                    throw;
                }
            } while (true);
        }
    }
}
