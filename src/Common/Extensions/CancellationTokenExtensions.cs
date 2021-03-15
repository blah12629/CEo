using System.Threading.Tasks;

namespace System.Threading
{
    public static partial class CancellationTokenExtensions
    {
        public static void ThrowIfTaskCancellationRequested(
            this CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();
        }
    }
}