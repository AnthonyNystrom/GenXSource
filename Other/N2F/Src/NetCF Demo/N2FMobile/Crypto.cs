/*
  Encryption Class From Microsoft Signature Sample
 --
 Uses crypto API functions to encrypt and decrypt data. A passphrase 
 string is used to create a 128-bit hash that is used to create a 
 40-bit crypto key. The same key is required to encrypt and decrypt 
 the data.
*/

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

namespace goAbout.Engine
{
	/// <summary>
	/// Encrypts and decrypts data using the crypto APIs.
	/// </summary>
	public class Crypto
	{
		// API functions
		private class WinApi
		{
			#region Crypto API imports
  
			private const uint ALG_CLASS_HASH = (4 << 13);
			private const uint ALG_TYPE_ANY = (0);
			private const uint ALG_CLASS_DATA_ENCRYPT = (3 << 13);
			private const uint ALG_TYPE_STREAM = (4 << 9);
			private const uint ALG_TYPE_BLOCK = (3 << 9);

			private const uint ALG_SID_DES = 1;
			private const uint ALG_SID_RC4 = 1;
			private const uint ALG_SID_RC2 = 2;
			private const uint ALG_SID_MD5 = 3;

			public const string MS_DEF_PROV = "Microsoft Base Cryptographic Provider v1.0";
   
			public const uint PROV_RSA_FULL = 1;
			public const uint CRYPT_VERIFYCONTEXT = 0xf0000000;
			public const uint CRYPT_EXPORTABLE = 0x00000001;

			public static readonly uint CALG_MD5 = (ALG_CLASS_HASH | ALG_TYPE_ANY | ALG_SID_MD5);
			public static readonly uint CALG_DES = (ALG_CLASS_DATA_ENCRYPT | ALG_TYPE_BLOCK | ALG_SID_DES);
			public static readonly uint CALG_RC2 = (ALG_CLASS_DATA_ENCRYPT | ALG_TYPE_BLOCK | ALG_SID_RC2);
			public static readonly uint CALG_RC4 = (ALG_CLASS_DATA_ENCRYPT | ALG_TYPE_STREAM | ALG_SID_RC4);

			// Use these dlls for mobile devices only

			const string CryptDll = "coredll.dll";
			const string KernelDll = "coredll.dll";

			// Use these dlls for all other pcs and servers

			//  const string CryptDll = "advapi32.dll";
			// const string KernelDll = "kernel32.dll";
   
			[DllImport(CryptDll)] 
			public static extern bool CryptAcquireContext(
				ref IntPtr phProv, string pszContainer, string pszProvider,
				uint dwProvType, uint dwFlags);

			[DllImport(CryptDll)] 
			public static extern bool CryptReleaseContext( 
				IntPtr hProv, uint dwFlags);

			[DllImport(CryptDll)] 
			public static extern bool CryptDeriveKey(
				IntPtr hProv, uint Algid, IntPtr hBaseData, 
				uint dwFlags, ref IntPtr phKey);
    
			[DllImport(CryptDll)] 
			public static extern bool CryptCreateHash(
				IntPtr hProv, uint Algid, IntPtr hKey, 
				uint dwFlags, ref IntPtr phHash);

			[DllImport(CryptDll)] 
			public static extern bool CryptHashData(
				IntPtr hHash, byte[] pbData, 
				uint dwDataLen, uint dwFlags);
    
			[DllImport(CryptDll)] 
			public static extern bool CryptEncrypt(
				IntPtr hKey, IntPtr hHash, bool Final, uint dwFlags, 
				byte[] pbData, ref uint pdwDataLen, uint dwBufLen);

			[DllImport(CryptDll)] 
			public static extern bool CryptDecrypt(
				IntPtr hKey, IntPtr hHash, bool Final, uint dwFlags, 
				byte[] pbData, ref uint pdwDataLen);

			[DllImport(CryptDll)] 
			public static extern bool CryptDestroyHash(IntPtr hHash);

			[DllImport(CryptDll)] 
			public static extern bool CryptDestroyKey(IntPtr hKey);

			#endregion

			#region Error reporting imports
 
			public const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

			[DllImport(KernelDll)]
			public static extern uint GetLastError();

			[DllImport(KernelDll)]
			public static extern uint FormatMessage(
				uint dwFlags, string lpSource, uint dwMessageId,
				uint dwLanguageId, StringBuilder lpBuffer, uint nSize,
				string [] Arguments);

			#endregion    
		}
 
		// all static methods
		private Crypto()
		{
		}
  
		/// <summary>
		/// Encrypt data. Use passphrase to generate the encryption key. 
		/// Returns a byte array that contains the encrypted data.
		/// </summary>
		static public byte[] Encrypt(string passphrase, byte[] data)
		{
			// holds encrypted data
			byte[] buffer = null;  

			// crypto handles
			IntPtr hProv = IntPtr.Zero;
			IntPtr hKey = IntPtr.Zero;

			try
			{
				// get crypto provider, specify the provider (3rd argument)
				// instead of using default to ensure the same provider is 
				// used on client and server
				if (!WinApi.CryptAcquireContext(ref hProv, null, WinApi.MS_DEF_PROV, 
					WinApi.PROV_RSA_FULL, WinApi.CRYPT_VERIFYCONTEXT))
					Failed("CryptAcquireContext");
    
				// generate encryption key from passphrase
				hKey = GetCryptoKey(hProv, passphrase);

				// determine how large of a buffer is required
				// to hold the encrypted data
				uint dataLength = (uint)data.Length;
				uint bufLength = (uint)data.Length;
				if (!WinApi.CryptEncrypt(hKey, IntPtr.Zero, true, 
					0, null, ref dataLength, bufLength))
					Failed("CryptEncrypt");
    
				// allocate and fill buffer with encrypted data
				buffer = new byte[dataLength];
				Buffer.BlockCopy(data, 0, buffer, 0, data.Length);
    
				dataLength = (uint)data.Length;
				bufLength = (uint)buffer.Length;
				if (!WinApi.CryptEncrypt(hKey, IntPtr.Zero, true, 
					0, buffer, ref dataLength, bufLength))
					Failed("CryptEncrypt");
			}
   
			finally
			{
				// release crypto handles
				if (hKey != IntPtr.Zero)
					WinApi.CryptDestroyKey(hKey);

				if (hProv != IntPtr.Zero)
					WinApi.CryptReleaseContext(hProv, 0);
			}
   
			return buffer;
		}


		/// <summary>
		/// Decrypt data. Use passphrase to generate the encryption key. 
		/// Returns a byte array that contains the decrypted data.
		/// </summary>
		static public byte[] Decrypt(string passphrase, byte[] data)
		{
			// make a copy of the encrypted data
			byte[] dataCopy = data.Clone() as byte[];
   
			// holds the decrypted data
			byte[] buffer = null;
   
			// crypto handles
			IntPtr hProv = IntPtr.Zero;
			IntPtr hKey = IntPtr.Zero;
   
			try
			{
				// get crypto provider, specify the provider (3rd argument)
				// instead of using default to ensure the same provider is 
				// used on client and server
				if (!WinApi.CryptAcquireContext(ref hProv, null, WinApi.MS_DEF_PROV, 
					WinApi.PROV_RSA_FULL, WinApi.CRYPT_VERIFYCONTEXT))
					Failed("CryptAcquireContext");
   
				// generate encryption key from the passphrase
				hKey = GetCryptoKey(hProv, passphrase);

				// decrypt the data
				uint dataLength = (uint)dataCopy.Length;
				if (!WinApi.CryptDecrypt(hKey, IntPtr.Zero, true, 
					0, dataCopy, ref dataLength))
					Failed("CryptDecrypt");
    
				// copy to a buffer that is returned to the caller
				// the decrypted data size might be less then
				// the encrypted size
				buffer = new byte[dataLength];
				Buffer.BlockCopy(dataCopy, 0, buffer, 0, (int)dataLength);
			}
   
			finally
			{
				// release crypto handles
				if (hKey != IntPtr.Zero)
					WinApi.CryptDestroyKey(hKey);

				if (hProv != IntPtr.Zero)
					WinApi.CryptReleaseContext(hProv, 0);
			}
   
			return buffer;
		}
  

		/// <summary>
		/// Create a crypto key form a passphrase. This key is 
		/// used to encrypt and decrypt data.
		/// </summary>
		static private IntPtr GetCryptoKey(IntPtr hProv, string passphrase)
		{
			// crypto handles
			IntPtr hHash = IntPtr.Zero;
			IntPtr hKey = IntPtr.Zero;
   
			try
			{
				// create 128 bit hash object
				if (!WinApi.CryptCreateHash(hProv, 
					WinApi.CALG_MD5, IntPtr.Zero, 0, ref hHash))
					Failed("CryptCreateHash");
    
				// add passphrase to hash
				byte[] keyData = ASCIIEncoding.ASCII.GetBytes(passphrase);
				if (!WinApi.CryptHashData(hHash, keyData, (uint)keyData.Length, 0))
					Failed("CryptHashData");
     
				// create 40 bit crypto key from passphrase hash
				if (!WinApi.CryptDeriveKey(hProv, WinApi.CALG_RC2, 
					hHash, WinApi.CRYPT_EXPORTABLE, ref hKey))
					Failed("CryptDeriveKey");
			}
   
			finally
			{
				// release hash object
				if (hHash != IntPtr.Zero)
					WinApi.CryptDestroyHash(hHash);
			}
   
			return hKey;
		}


		/// <summary>
		/// Throws SystemException with GetLastError information.
		/// </summary>
		static private void Failed(string command)
		{
			uint lastError = WinApi.GetLastError();
			StringBuilder sb = new StringBuilder(500);

			try
			{
				// get message for last error
				WinApi.FormatMessage(WinApi.FORMAT_MESSAGE_FROM_SYSTEM, 
					null, lastError, 0, sb, 500, null);
			}
			catch
			{
				// error calling FormatMessage
				sb.Append("N/A.");
			}
     
			throw new SystemException(
				string.Format("{0} failed.\r\nLast error - 0x{1:x}.\r\nError message - {2}",
				command, lastError, sb.ToString()));
		}
	}
}