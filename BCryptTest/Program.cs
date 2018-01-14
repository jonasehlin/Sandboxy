using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows;

namespace BCryptTest
{
	class Program
	{
		static void Main(string[] args)
		{
			string valueStr = "Apanson ar ett miffo";

			using (var bcrypt = new BCrypt(BCryptDll.BCRYPT_SHA256_ALGORITHM))
			{
				var hash = bcrypt.Hash(valueStr);
			}

			int status = BCryptDll.BCryptOpenAlgorithmProvider(
				out IntPtr hAlgorithm,
				BCryptDll.BCRYPT_SHA256_ALGORITHM,
				BCryptDll.MS_PRIMITIVE_PROVIDER,
				0);

			byte[] buffer = new byte[256];
			status = BCryptDll.BCryptGetProperty(
				hAlgorithm,
				BCryptDll.BCRYPT_HASH_LENGTH,
				buffer,
				out uint pcbResult,
				0
			);

			var value = Encoding.UTF8.GetBytes(valueStr);
			byte[] output = new byte[buffer[0]];
			status = BCryptDll.BCryptHash(
				hAlgorithm,
				null,
				value,
				output
			);

			status = BCryptDll.BCryptCloseAlgorithmProvider(hAlgorithm, 0);
		}
	}
}
