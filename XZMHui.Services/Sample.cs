namespace XZMHui.Services
{
    public interface ISample
    {
        string Hello();
    }

    public class SampleA : ISample
    {
        public string Hello()
        {
            return "Hello SampleA";
        }
    }

    public class SampleB : ISample
    {
        public string Hello()
        {
            return "Hello SampleB";
        }
    }

    public class SampleC : ISample
    {
        public string Hello()
        {
            return "Hello SampleC";
        }
    }
}