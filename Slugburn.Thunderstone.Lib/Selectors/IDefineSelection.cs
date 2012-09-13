using System;

namespace Slugburn.Thunderstone.Lib.Selectors
{
    public interface IDefineSelection : IDefineSelectionOrCallback
    {
    }

    public interface IDefineSelectionOrCallback
    {
    }

    public static class DefineSelectionExtensions
    {
        public static IDefineSelection Caption(this IDefineSelection context, string caption)
        {
            var c = (SelectionContext)context; 
            c.Caption = caption;
            return c;
        }

        public static IDefineSelection Message(this IDefineSelection context, string message)
        {
            var c = (SelectionContext)context;
            c.Message = message;
            return c;
        }

        public static IDefineSelection Min(this IDefineSelection context, int min)
        {
            var c = (SelectionContext)context;
            c.Min = min;
            return c;
        }

        public static IDefineSelection Max(this IDefineSelection context, int max)
        {
            var c = (SelectionContext)context;
            c.Max = max;
            return c;
        }

        public static IDefineSelection Filter(this IDefineSelection context, Func<Card, bool> filter)
        {
            var c = (SelectionContext)context;
            c.Filter = filter;
            return c;
        }

        
    }
}