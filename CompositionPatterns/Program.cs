using CompositionPatterns;

internal class Program
{
    private static void Main(string[] args)
    {
        new Composing().DoWorkWithStandatdMethod();

        new Pipelining().DoWorkPopeline();
        new ExampleEnumirable().UseEnumirablePipeline();
        MapFuntionExtention.SelectWithNoTransform();
    }
}