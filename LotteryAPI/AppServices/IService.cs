namespace LotteryAPI.AppServices
{
    public interface IService
    {
        Guid GetSingleton();
    }

    /*public class SingletonService : IService
    {
        private readonly ServiceSingleton serviceSingleton;

        public Guid GetSingleton() { return serviceSingleton.guid; }

    }*/
}
