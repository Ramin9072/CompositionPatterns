using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

    #endregion

    #region Enumirable

    public class ExampleEnumerable
    {
        public void UseEnumirablePipeline()
        {
            var numbers = Enumerable.Range(1, 100);
            var resultA = numbers.WhereAsPipeline(p => p % 5 == 0).ToList();
            var resultB = numbers.WhereAsPipeline(p => p % 12 == 0).TransformAsPipeline(p => p * 10).ToList();
            var resultC = numbers.SkipByAsPipeline(6).ToList();
            var resultD = numbers.TestMethod(p => p % 10 == 0, p => p * 20).ToList();
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

        public static IEnumerable<T> TestMethod<T>(this IEnumerable<T> source, Predicate<T> predicate, Func<T, T> transformer)
        {
            foreach (T item in source)
            {
                if (predicate(item))
                {
                    yield return transformer(item);
                }
            }
        }
    }
    #endregion

    #region Mapp

    // dar Linq item Select haman mapp ast

    // Funtion   .net

    //map =>    select 
    //filter =>  where 
    //bind =>   select Many
    //bind =>   continueWith
    //fold =>    sum 
    //fold =>   Aggregate 

    public static class MapFuntionExtention
    {
        public class MappClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public static void SelectWithNoTransform()
        {

            var numbers = Enumerable.Range(1, 50);

            var quearyA = numbers.Select(p => new MappClass
            {
                Id = p
            });
            var queryB = from n in numbers
                         select n;
            var queryB1 = from n in numbers
                          select new MappClass
                          {
                              Id = n
                          };

            List<int> resultA = new List<int>();
            foreach (var item in quearyA)
            {
                resultA.Add(item.Id);
            }

            var resultB = queryB.ToString();
        }

        public static void SelectProjectToAnotherType()
        {
            var xValues = Enumerable.Range(1, 30);
            var yValues = Enumerable.Range(100, 200);

            var qA = xValues.Select(p => new Raypoint(p, 0));
            var qB = from n in yValues
                     select new Raypoint(0, n);
        }

    }

    public class Raypoint
    {
        public Raypoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }
    #endregion

    #region Filter 

    public class FilterClass
    {
        public void FilterSimple()
        {
            var numbers = Enumerable.Range(1, 200);

            var resultA = numbers.Where(p => p < 20).Select(p => p);
            var resultB = from n in numbers
                          where n < 20
                          select n;

            var resultC = resultA.ToList();
            var resultD = resultB.ToList();
        }
        // عدد اول
        public void FilterForPrimeNumberes()
        {

            // تشخیص عدد اول
            Func<int, bool> prime = p => Enumerable.Range(2, (int)Math.Sqrt(p) - 1)
            .All(divisor => p % divisor != 0);
            // عدد های مد نظر
            var primes = Enumerable.Range(2, 1000 * 1000).Where(prime);

        }

    }

    #endregion

    #region Flatening 
    // Select many 
    // Join

    public class Flatening
    {
        public void FlattenSelectSelectMany()
        {
            var brandA = new Brand() { Name = "Fancy", Colors = new List<string>() { "Red", "Green" } };
            var brandB = new Brand() { Name = "Lux", Colors = new List<string>() { "Silver", "Gold" } };
            var brandC = new Brand() { Name = "Matt", Colors = new List<string>() { "Black", "White" } };

            List<Brand> brands = new List<Brand>();
            brands.Add(brandA);
            brands.Add(brandB);
            brands.Add(brandC);

            var resultA = brands.Select(p => p.Colors).ToList(); // nested result لایه ای شد

            // FLATEN vvvvvv
            var resultB = brands.SelectMany(p => p.Colors).ToList(); // result لایه ای نیست

        }

        public void JoinExample()
        {
            var numA = Enumerable.Range(2, 3);
            var numB = Enumerable.Range(5, 7);

            var basicSelectJoin = numA.Select(p => numB.Select(b => $"A: {p} B: {b}"));
            var selectManyJoin = numA.SelectMany(p => numB.Select(b => $"A: {p} B: {b}"));
            var resultA = basicSelectJoin.ToList();
            var resultB = selectManyJoin.ToList();
        }

    }

    public class Brand
    {
        public string Name { get; set; }
        public List<string> Colors { get; set; }

    }
    #endregion

    #region FOLD OR Aggregate Max Min Sum Count .... 

    public class FOLD
    {

        public void AggregateExample()
        {
            var numberA = Enumerable.Range(1, 100).ToArray();
            ImmutableList<int> SetA = ImmutableList.Create(numberA);
            ImmutableList<int> SetB = ImmutableList.Create(numberA.Where(p => p % 6 == 0).ToArray());

            // Fold ha yek SingleResult ast 
            var total = SetA.Sum();
            var max = SetA.Max();
            var count = SetA.Count();
            // ..... 

            // Custom aggregate 

            var multipleOf1 = SetA.Aggregate((total, count) => total * count);
            var multipleOf2 = SetA.Aggregate(100,(total, count) => total * count);


        }
    }

    #endregion
}


