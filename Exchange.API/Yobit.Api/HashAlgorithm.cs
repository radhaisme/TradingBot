
namespace Yobit.Api
{
	using System.Text;
	using System.Security.Cryptography;

	public class HashAlgorithm : IHashAlgorithm
	{
		private readonly HMACSHA1 _hmac;

		public HashAlgorithm(string secret)
		{
			_hmac = new HMACSHA1(Encoding.Default.GetBytes(secret));
		}

		public byte[] ComputeHash(byte[] buffer)
		{
			return _hmac.ComputeHash(buffer);
		}
	}
}