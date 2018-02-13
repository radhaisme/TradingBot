
namespace Yobit.Api
{
	using System.Text;
	using System.Security.Cryptography;

	public class HashAlgorithm : IHashAlgorithm
	{
		private readonly HMACSHA512 _hmac;

		public HashAlgorithm(string secret)
		{
			_hmac = new HMACSHA512(Encoding.Default.GetBytes(secret));
		}

		public byte[] ComputeHash(byte[] buffer)
		{
			return _hmac.ComputeHash(buffer);
		}
	}
}