namespace CsNetLib2
{
	public delegate void DataAvailabe(string data, long clientId);
	public delegate void BytesAvailable(byte[] bytes, long clientId);
}