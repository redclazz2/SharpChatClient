namespace SharpSocket.Domain.Interface.Client
{
    public interface IClientSocket
    {
        public bool Connect();
        public void Write(byte[] data);
        public void Read();
    }
}