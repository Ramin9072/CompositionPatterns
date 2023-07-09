using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CompositionPatterns
{
    #region Compose Pattern

    // گروهی از توابع را ساخته و یا ورودی یک تابع که خروجی یک تابع دیگر است به ما بر میگرداند
    internal class Composing
    {

        public void DoWorkWithStandatdMethod()
        {
            int resultA = MakeNegative(ToFourPower(5));
            int resultB = ToFourPower(MakeNegative(5));
            int ResultC = AddTo(ToFourPower((MakeNegative(5))), 3);
        }

        public void DoWorkWithFunc()
        {
            Func<int, int> ToFourPower = x => x * x * x;
            Func<int, int> MakeNegative = x => -1 * x;
            Func<int, int, int> AddTo = (x, y) => x + y;


            int ResultA = AddTo(ToFourPower(MakeNegative(5)), 3);
            var compose = MakeNegative.Compose(ToFourPower);

            int ResultB = compose(5);
        }

        #region Standard Function

        public int ToFourPower(int candidate)
        {
            return candidate * candidate * candidate * candidate;
        }

        public int MakeNegative(int candidate)
        {
            return candidate * -1;
        }

        public int AddTo(int candidate, int adder)
        {
            return candidate * adder;
        }

        #endregion

    }
    internal static class Extentions
    {
        public static Func<T, TReturn2> Compose<T, TReturn1, TReturn2>(this Func<TReturn1, TReturn2> func1, Func<T, TReturn1> func2)
        {
            return x => func1(func2(x));
        }
    }

    #endregion

    #region Pipelining Pattern

    internal class Pipelining
    {
        public void DoWorkPopeline()
        {
            int value = 5;

            int ResultA = value.ToFourPowerExtention().MakeNegativeExtention();
            int ResultB = value.ToFourPowerExtention().MakeNegativeExtention().AddToExtention(10);

            string valueStr = "Ali";
            var Result3 = valueStr.AddNewText("Reza");
        }
    }

    public static class Extention
    {

        // برای اکستنشن متد شدن فقط کافی است که کلمه this اضافه گردد 
        public static int ToFourPowerExtention(this int candidate)
        {
            return candidate * candidate * candidate * candidate;
        }

        public static int MakeNegativeExtention(this int candidate)
        {
            return candidate * -1;
        }

        public static int AddToExtention(this int candidate, int adder)
        {
            return candidate * adder;
        }

        // string

        public static string AddNewText(this string value, string value2)
        {
            return value + value2;
        }
    }


    public class ExampleEnumirable
    {
        public void UseEnumirablePipeline()
        {
            var numbers = Enumerable.Range(1, 100);
            var resultA = numbers.WhereAsPipeline(p => p % 5 == 0).ToList();
            var resultB = numbers.WhereAsPipeline(p => p % 12 == 0).TransformAsPipeline(p => p * 10).ToList();
            var resultC = numbers.SkipByAsPipeline(6).ToList();
        }
    }
    public static class Extentions2
    {
        public static IEnumerable<T> WhereAsPipeline<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            foreach (T item in source)
            {
                // در اینجا برای متد اول 
                //WhereAsPipeline(p => p % 5 == 0)
                // در Predicate بررسی میگردد
                if (predicate(item))

                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> TransformAsPipeline<T>(this IEnumerable<T> source, Func<T, T> transformer)
        {
            foreach (T item in source)
            {
                yield return transformer(item);
            }
        }

        public static IEnumerable<T> SkipByAsPipeline<T>(this IEnumerable<T> source, int numberToSkip)
        {
            using (IEnumerator<T> e = source.GetEnumerator())
            {
                while (numberToSkip > 0 && e.MoveNext())
                {
                    numberToSkip--;
                }

                if (numberToSkip <= 0)
                {
                    while (e.MoveNext())
                    {
                        yield return e.Current;
                    }
                }
            }
        }

      
    }
    #endregion
}


