﻿using CompositionPatterns;

internal class Program
{
    private static void Main(string[] args)
    {
        new Composing().DoWorkWithStandatdMethod();

        new Pipelining().DoWorkPopeline();
        new ExampleEnumerable().UseEnumirablePipeline();
        MapFuntionExtention.SelectWithNoTransform();
        new FilterClass().FilterForPrimeNumberes();
        new FilterClass().FilterSimple();
        new Flatening().FlattenSelectSelectMany();
        new Flatening().JoinExample();
        new FOLD().AggregateExample();
    }
}