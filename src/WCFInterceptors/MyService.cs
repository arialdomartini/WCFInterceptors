
namespace WCFInterceptors
{
    public class MyService : IMyService
    {
        public string SayHello(string name)
        {
            return string.Format("Hello, {0}!", name);
        }
    }
}