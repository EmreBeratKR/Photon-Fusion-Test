namespace Helpers
{
    public static class EventSystem
    {
        public delegate void DefaultHandler();
        public delegate void GenericHandler<T>(T arg);
        public delegate void ParamsGenericHandler<T>(params T[] args);

    }
}
