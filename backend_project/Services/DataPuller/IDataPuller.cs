public interface IDataPuller
{
    Task Start();
    Task StoreDataFromSources();
}