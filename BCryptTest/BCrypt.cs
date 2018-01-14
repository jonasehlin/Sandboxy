using System;
using System.Text;

namespace Windows
{
	public class BCrypt : IDisposable
	{
		IntPtr _hAlgorithm;
		int _hashLength;

		public BCrypt()
		{
		}

		public BCrypt(string algorithm)
		{
			Open(algorithm);
		}

		public void Open(string algorithm)
		{
			Close();

			int status = BCryptDll.BCryptOpenAlgorithmProvider(
				out IntPtr hAlgorithm,
				algorithm,
				BCryptDll.MS_PRIMITIVE_PROVIDER,
				0);
			if (status != 0)
				throw new Exception($"Error: Status = {status}");
			_hAlgorithm = hAlgorithm;

			byte[] length = new byte[4];
			status = BCryptDll.BCryptGetProperty(
				_hAlgorithm,
				BCryptDll.BCRYPT_HASH_LENGTH,
				length,
				out uint pcbResult,
				0);
			if (status != 0 || pcbResult != 4)
				throw new Exception($"Error: Status = {status}");

			_hashLength = BitConverter.ToInt32(length, 0);
		}

		public byte[] Hash(byte[] input, byte[] secret)
		{
			byte[] output = new byte[_hashLength];
			int status = BCryptDll.BCryptHash(
				_hAlgorithm,
				secret,
				input,
				output
			);
			if (status != 0)
				throw new Exception($"Error: Status = {status}");
			return output;
		}

		public byte[] Hash(byte[] input)
		{
			return Hash(input, null);
		}

		public byte[] Hash(string input, byte[] secret)
		{
			return Hash(Encoding.UTF8.GetBytes(input), secret);
		}

		public byte[] Hash(string input)
		{
			return Hash(input, null);
		}

		public void Close()
		{
			if (_hAlgorithm == IntPtr.Zero)
				return;

			int status = BCryptDll.BCryptCloseAlgorithmProvider(_hAlgorithm, 0);
			if (status != 0)
				throw new Exception($"Error: Status = {status}");
		}

		public void Dispose()
		{
			Close();
		}
	}
}
