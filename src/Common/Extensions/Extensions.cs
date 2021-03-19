using System;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static partial class Extensions
    {
        public static T Build<T>(this T value, params Action[] actions)
        {
            for (var i = 0; i < actions.Length; i ++) actions[i]();
            return value;
        }

        public static async Task<T> BuildAllAsync<T>(this T value, params Func<Task>[] tasks)
        {
            await Task.WhenAll(tasks.Select(async task => await task()));
            return value;
        }
    }
}