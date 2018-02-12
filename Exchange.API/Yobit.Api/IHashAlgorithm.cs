
namespace Yobit.Api
{
    public interface IHashAlgorithm
    {
	    byte[] ComputeHash(byte[] buffer);
    }
}