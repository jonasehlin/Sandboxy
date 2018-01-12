using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Text;
using Windows;

namespace Sandboxy
{
	public class Program
    {
        public static void Main(string[] args)
        {
			int status = BCrypt.BCryptOpenAlgorithmProvider(
				out IntPtr hAlgorithm,
				BCrypt.BCRYPT_SHA256_ALGORITHM,
				BCrypt.MS_PRIMITIVE_PROVIDER,
				0);

			byte[] buffer = new byte[256];
			status = BCrypt.BCryptGetProperty(
				hAlgorithm,
				BCrypt.BCRYPT_HASH_LENGTH,
				buffer,
				out uint pcbResult,
				0
			);

			string valueStr = "Apanson ar ett miffo";
			var value = Encoding.UTF8.GetBytes(valueStr);
			byte[] output = new byte[buffer[0]];
			status = BCrypt.BCryptHash(
				hAlgorithm,
				null,
				value,
				output
			);

			status = BCrypt.BCryptCloseAlgorithmProvider(hAlgorithm, 0);

			BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
