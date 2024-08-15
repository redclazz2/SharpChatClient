namespace SharpSocket.Domain.Interface.Client
{
    public interface IClientProcess
    {
        public bool Init();
        public void Close();
        //public byte[] Read();
        public void Write(byte[] data);
    }
}