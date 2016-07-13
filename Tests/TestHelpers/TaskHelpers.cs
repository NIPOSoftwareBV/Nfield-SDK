using System.Threading.Tasks;

namespace Nfield.TestHelpers
{
    public class NfieldTaskHelpers
    {
        /// <summary>
        /// Create a completed Task from a result. .Net 4.5 and higher have nice helpers for these built in.
        /// </summary>
        public static Task<T> FromResult<T>(T result)
        {
            var taskSource = new TaskCompletionSource<T>();
            taskSource.SetResult(result);
            return taskSource.Task;
        }
    }
}
