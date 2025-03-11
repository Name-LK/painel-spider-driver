namespace PainelSpider.Core.Interfaces
{
    public interface IDriverSpider
    {
        void Connect();
        void Disconnect();
        void EnsureConencted();
        void SetMessage();
        void GetMessage();
    }
}