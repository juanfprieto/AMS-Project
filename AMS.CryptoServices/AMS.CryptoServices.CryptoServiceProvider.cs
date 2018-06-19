using System;
using System.Security.Cryptography;

namespace AMS.CriptoServiceProvider
{
	/// <summary>
	/// Descripci�n breve de CriptoServiceProvider.
	/// </summary>
	internal class CryptoServiceProvider
	{
		// proveedor de cifrado.
		private CryptoProvider algorithm;
		// acci�n a realizar
		private CryptoAction cAction;

		// Lista con las posibles acciones a realizar dentro de la clase.
		internal enum CryptoAction
		{
			Encrypt,  // Cifrar
			Desencrypt  // Descifrar
		}

		// Lista con los proveedores de cifrado que proporciona la clase.
		internal enum CryptoProvider
		{
			DES,
			TripleDES,
			RC2,
			Rijndael
		}
		
		internal CryptoServiceProvider(CryptoProvider alg, CryptoAction action)
		{
			// asignamos las opciones seleccionadas.
			// proveedor (algoritmo) de encripci�n y acci�n a realizar.
			this.algorithm = alg;
			this.cAction = action;
		}

		/// <summary>
		/// Define un objeto para la operaciones b�sicas de transformaciones
		/// criptogr�ficas.
		/// </summary>
		/// <param name="Key">Clave de encripci�n.</param>
		/// <param name="IV">Vector de inicializaci�n.</param>
		/// <returns>Devuelve el objeto que implementa la interfaz ICryptoTransform.
		/// </returns>
		internal ICryptoTransform GetServiceProvider(byte[] Key, byte[] IV)
		{
			// creamos la variable que contendr� al objeto ICryptoTransform.
			ICryptoTransform transform = null;

			// dependiendo del algoritmo seleccionado, se devuelve el objeto adecuado.
			switch (this.algorithm)
			{
					// Algoritmo DES.
				case CryptoProvider.DES:
					DESCryptoServiceProvider des = new DESCryptoServiceProvider();
					// dependiendo de la acci�n a realizar.
					// creamos el objeto adecuado.
				switch (cAction)
				{
					case CryptoAction.Encrypt:  // si estamos cifrando,
						// creamos el objeto cifrador. 
						transform = des.CreateEncryptor(Key, IV);
						break;
					case CryptoAction.Desencrypt: // s� estamos descifrando,
						// creamos el objeto descifrador.
						transform = des.CreateDecryptor(Key, IV);
						break;
				}
					return transform;  // devolvemos el objeto transform correspondiente.
					// algoritmo TripleDES.
				case CryptoProvider.TripleDES:
					TripleDESCryptoServiceProvider des3 = new TripleDESCryptoServiceProvider();
				switch (cAction)
				{
					case CryptoAction.Encrypt:
						transform = des3.CreateEncryptor(Key, IV);
						break;
					case CryptoAction.Desencrypt:
						transform = des3.CreateDecryptor(Key, IV);
						break;
				}
					return transform;
					// algoritmo RC2.
				case CryptoProvider.RC2:
					RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
				switch (cAction)
				{
					case CryptoAction.Encrypt:
						transform = rc2.CreateEncryptor(Key, IV);
						break;
					case CryptoAction.Desencrypt:
						transform = rc2.CreateDecryptor(Key, IV);
						break;
				}
					return transform;
					// algoritmo Rijndael.
				case CryptoProvider.Rijndael:
					Rijndael rijndael = new RijndaelManaged();
				switch (cAction)
				{
					case CryptoAction.Encrypt:
						transform = rijndael.CreateEncryptor(Key, IV);
						break;
					case CryptoAction.Desencrypt:
						transform = rijndael.CreateDecryptor(Key, IV);
						break;
				}
					return transform;
				default:
					// en caso que no exista el proveedor seleccionado, generamos
					// una excepci�n para informarlo.
					throw new CryptographicException("Error al inicializar al proveedor de cifrado");
			}
		}
	}
}
