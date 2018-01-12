using System;
using System.Runtime.InteropServices;

namespace Windows
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// https://msdn.microsoft.com/en-us/library/windows/desktop/mt633798%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
	/// </remarks>
	public static class BCrypt
    {
		#region Common algorithm identifiers

		public const string BCRYPT_RSA_ALGORITHM = "RSA";
		public const string BCRYPT_RSA_SIGN_ALGORITHM = "RSA_SIGN";
		public const string BCRYPT_DH_ALGORITHM = "DH";
		public const string BCRYPT_DSA_ALGORITHM = "DSA";
		public const string BCRYPT_RC2_ALGORITHM = "RC2";
		public const string BCRYPT_RC4_ALGORITHM = "RC4";
		public const string BCRYPT_AES_ALGORITHM = "AES";
		public const string BCRYPT_DES_ALGORITHM = "DES";
		public const string BCRYPT_DESX_ALGORITHM = "DESX";
		public const string BCRYPT_3DES_ALGORITHM = "3DES";
		public const string BCRYPT_3DES_112_ALGORITHM = "3DES_112";
		public const string BCRYPT_MD2_ALGORITHM = "MD2";
		public const string BCRYPT_MD4_ALGORITHM = "MD4";
		public const string BCRYPT_MD5_ALGORITHM = "MD5";
		public const string BCRYPT_SHA1_ALGORITHM = "SHA1";
		public const string BCRYPT_SHA256_ALGORITHM = "SHA256";
		public const string BCRYPT_SHA384_ALGORITHM = "SHA384";
		public const string BCRYPT_SHA512_ALGORITHM = "SHA512";
		public const string BCRYPT_AES_GMAC_ALGORITHM = "AES-GMAC";
		public const string BCRYPT_AES_CMAC_ALGORITHM = "AES-CMAC";
		public const string BCRYPT_ECDSA_P256_ALGORITHM = "ECDSA_P256";
		public const string BCRYPT_ECDSA_P384_ALGORITHM = "ECDSA_P384";
		public const string BCRYPT_ECDSA_P521_ALGORITHM = "ECDSA_P521";
		public const string BCRYPT_ECDH_P256_ALGORITHM = "ECDH_P256";
		public const string BCRYPT_ECDH_P384_ALGORITHM = "ECDH_P384";
		public const string BCRYPT_ECDH_P521_ALGORITHM = "ECDH_P521";
		public const string BCRYPT_RNG_ALGORITHM = "RNG";
		public const string BCRYPT_RNG_FIPS186_DSA_ALGORITHM = "FIPS186DSARNG";
		public const string BCRYPT_RNG_DUAL_EC_ALGORITHM = "DUALECRNG";

		#endregion

		#region  Microsoft built-in providers.

		public const string MS_PRIMITIVE_PROVIDER = "Microsoft Primitive Provider";
		public const string MS_PLATFORM_CRYPTO_PROVIDER = "Microsoft Platform Crypto Provider";

		#endregion

		/// <summary>
		/// The BCryptOpenAlgorithmProvider function loads and initializes a CNG provider.
		/// </summary>
		/// <remarks>
		/// https://msdn.microsoft.com/en-us/library/windows/desktop/aa375479(v=vs.85).aspx
		/// </remarks>
		[DllImport("Bcrypt.dll", CharSet = CharSet.Unicode)]
		public static extern int BCryptOpenAlgorithmProvider(out IntPtr phAlgorithm, string pszAlgId,
			string pszImplementation, uint dwFlags);

		#region BCryptGetProperty strings

		public const string BCRYPT_OBJECT_LENGTH = "ObjectLength";
		public const string BCRYPT_ALGORITHM_NAME = "AlgorithmName";
		public const string BCRYPT_PROVIDER_HANDLE = "ProviderHandle";
		public const string BCRYPT_CHAINING_MODE = "ChainingMode";
		public const string BCRYPT_BLOCK_LENGTH = "BlockLength";
		public const string BCRYPT_KEY_LENGTH = "KeyLength";
		public const string BCRYPT_KEY_OBJECT_LENGTH = "KeyObjectLength";
		public const string BCRYPT_KEY_STRENGTH = "KeyStrength";
		public const string BCRYPT_KEY_LENGTHS = "KeyLengths";
		public const string BCRYPT_BLOCK_SIZE_LIST = "BlockSizeList";
		public const string BCRYPT_EFFECTIVE_KEY_LENGTH = "EffectiveKeyLength";
		public const string BCRYPT_HASH_LENGTH = "HashDigestLength";
		public const string BCRYPT_HASH_OID_LIST = "HashOIDList";
		public const string BCRYPT_PADDING_SCHEMES = "PaddingSchemes";
		public const string BCRYPT_SIGNATURE_LENGTH = "SignatureLength";
		public const string BCRYPT_HASH_BLOCK_LENGTH = "HashBlockLength";
		public const string BCRYPT_AUTH_TAG_LENGTH = "AuthTagLength";

		#endregion

		[DllImport("Bcrypt.dll", CharSet = CharSet.Unicode)]
		static extern int BCryptGetProperty(IntPtr hObject, string pszProperty, byte[] pbOutput, uint cbOutput,
			out uint pcbResult, uint dwFlags);

		/// <summary>
		/// The BCryptGetProperty function retrieves the value of a named property for a CNG object.
		/// </summary>
		/// <remarks>
		/// https://msdn.microsoft.com/en-us/library/windows/desktop/aa375464%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
		/// </remarks>
		public static int BCryptGetProperty(IntPtr hObject, string pszProperty, byte[] pbOutput,
			out uint pcbResult, uint dwFlags)
		{
			return BCryptGetProperty(hObject, pszProperty, pbOutput, (uint)pbOutput.Length, out pcbResult, dwFlags);
		}

		[DllImport("Bcrypt.dll", CharSet = CharSet.Unicode)]
		static extern int BCryptHash(IntPtr hAlgorithm, byte[] pbSecret, uint cbSecret,
			byte[] pbInput, uint cbInput, byte[] pbOutput, uint cbOutput);

		/// <summary>
		/// Performs a single hash computation. This is a convenience function that wraps calls to
		/// BCryptCreateHash, BCryptHashData, BCryptFinishHash, and BCryptDestroyHash.
		/// </summary>
		/// <remarks>
		/// https://msdn.microsoft.com/en-us/library/windows/desktop/mt633798%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
		/// </remarks>
		public static int BCryptHash(IntPtr hAlgorithm, byte[] pbSecret, byte[] pbInput, byte[] pbOutput)
		{
			return BCryptHash(hAlgorithm, pbSecret, pbSecret == null ? 0 : (uint)pbSecret.Length,
				pbInput, pbInput == null ? 0 : (uint)pbInput.Length, pbOutput, (uint)pbOutput.Length);
		}

		/// <summary>
		/// The BCryptCloseAlgorithmProvider function closes an algorithm provider.
		/// </summary>
		/// <remarks>
		/// https://msdn.microsoft.com/en-us/library/windows/desktop/aa375377%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
		/// </remarks>
		[DllImport("Bcrypt.dll")]
		public static extern int BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, uint dwFlags);
	}
}
