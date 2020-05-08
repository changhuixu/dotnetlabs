using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsyncProcessor
{
    public class Processor
    {

    }

    public class AppContext
    {

    }
    public static class UseExtensions
    {
        //public static Processor Use(this Processor processor, Func<AppContext, Func<Task>, Task> middleware)
        //{
        //    return processor.Use(next =>
        //    {
        //        return context =>
        //        {
        //            Func<Task> simpleNext = () => next(context);
        //            return middleware(context, simpleNext);
        //        };
        //    });
        //}
    }
}
