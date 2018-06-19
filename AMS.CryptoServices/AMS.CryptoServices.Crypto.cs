using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace AMS.CriptoServiceProvider
{
	/// <summary>
	/// Descripción breve de Crypto.
	/// </summary>
	public class Crypto
	{
		// Establecemos las variables privadas que contendrán
		// la llave de encripción así como el vector de inicialización.
		private string stringKey;
		private string stringIV; 
		// Acción a ejecutar dentro de los métodos.
		private CryptoProvider algorithm;

		/// <summary>
		/// Proveedores del Servicio de criptografía.
		/// </summary>
		public enum CryptoProvider
		{
			DES,
			TripleDES,
			RC2,
			Rijndael
		}

		/// <summary>
		/// Encripción / Desencripción.
		/// </summary>
		public enum CryptoAction
		{
			Encrypt,
			Desencrypt
		}
		
		/// <summary>
		/// Constructor por defecto.
		/// </summary>
		/// <param name="alg">Establece el algoritmo de Encripción a utilizar.</param>
		public Crypto(CryptoProvider alg)
		{
			this.algorithm = alg;
		}

		/// <summary>
		/// Propiedad que obtiene o establece el valor de la llave de encripción
		/// </summary>
		public string Key
		{
			get
			{
				return stringKey;
			}
			set
			{
				stringKey = value;
			}
		}

		/// <summary>
		/// Propiedad que obtiene o establece el valor del vector de inicialización.
		/// </summary>
		public string IV 
		{
			get
			{
				return stringIV;
			}
			set
			{
				stringIV = value;
			}
		}

		/// <summary>
		/// Convierte los valores de tipo string, de la llave de cifrado
		/// en sus correspondiente byte array.
		/// </summary>
		/// <returns>Devuelve el arreglo de bytes correspondiente a la llave de cifrado.</returns>
		private byte[] MakeKeyByteArray()
		{
			// dependiendo del algoritmo utilizado.
			switch (this.algorithm)
			{
					// para los algoritmos
				case CryptoProvider.DES:
				case CryptoProvider.RC2:
					// verificamos que la longitud no sea menor que 8 bytes,
					if (stringKey.Length < 8)
						// de ser así, completamos la cadena hasta un valor válido
						stringKey = stringKey.PadRight(8);
					else if (stringKey.Length > 8)// si la cadena supera los 8 bytes,
						// truncamos la cadena dejándola en 8 bytes.
						stringKey = stringKey.Substring(0, 8); 
					break;
					// para los algoritmos
				case CryptoProvider.TripleDES:
				case CryptoProvider.Rijndael:
					// verificamos que la longitud no sea menor a 16 bytes
					if (stringKey.Length < 16)
						// de ser así, completamos la cadena hasta esos 16 bytes.
						stringKey = stringKey.PadRight(16); 
					else if (stringKey.Length > 16)//longitud es mayor a 16 bytes,
						// truncamos la cadena dejándola en 16 bytes.
						stringKey = stringKey.Substring(0, 16);
					break;
			}

			// utilizando los métodos del namespace System.Text, 
			// convertimos la cadena de caracteres en un arreglo de bytes
			// mediante el método GetBytes() del sistema de codificación UTF.
			return Encoding.UTF8.GetBytes(stringKey);
		}

		/// <summary>
		/// Convierte los valores de tipo string, del vector de inicialización
		/// en sus correspondiente byte array.
		/// </summary>
		/// <returns>Devuelve el arreglo de bytes correspondiente al VI.</returns>
		private byte[] MakeIVByteArray()
		{
			// dependiendo del algoritmo utilizado.
			switch (this.algorithm)
			{
					// para los algoritmos 
				case CryptoProvider.DES:
				case CryptoProvider.RC2:
				case CryptoProvider.TripleDES:
					// verificamos que la longitud no sea menor que 8 bytes, 
					if (stringIV.Length < 8)
						// de ser así, completamos la cadena hasta un valor válido
						stringIV = stringIV.PadRight(8);
					else if (stringIV.Length > 8)//si la cadena supera los 8 bytes,
						// truncamos la cadena dejándola en 8 bytes.
						stringIV = stringIV.Substring(0, 8);
					break;
				case CryptoProvider.Rijndael:
					// verificamos que la longitud no sea menor que 16 bytes,
					if (stringIV.Length < 16)
						// de ser así, completamos la cadena hasta un valor válido
						stringIV = stringIV.PadRight(16);
					else if (stringIV.Length > 16)//si la cadena supera los 16 bytes,
						// truncamos la cadena dejándola en 16 bytes.
						stringIV = stringIV.Substring(0, 16);
					break;
			}

			// utilizando los métodos del namespace System.Text, 
			// convertimos la cadena de caracteres en un arreglo de bytes
			// mediante el método GetBytes() del sistema de codificación UTF.
			return Encoding.UTF8.GetBytes(stringIV);
		}

		/// <summary>
		/// Cifra la cadena usando el proveedor especificado.
		/// </summary>
		/// <param name="CadenaOriginal">Cadena que será cifrada.</param>
		/// <returns>Devuelve la cadena cifrada.</returns>
		public string CifrarCadena(string CadenaOriginal)
		{ 
			// creamos el flujo tomando la memoria como respaldo.
			MemoryStream memStream = null;
			try
			{
				// verificamos que la llave y el VI han sido proporcionados.
				if (stringKey != null && stringIV != null)
				{
					// obtenemos el arreglo de bytes correspondiente a la llave
					// y al vector de inicialización.
					byte[] key = MakeKeyByteArray(); 
					byte[] IV = MakeIVByteArray(); 
					// convertimos el mensaje original en sus correspondiente
					// arreglo de bytes.
					byte[] textoPlano = Encoding.UTF8.GetBytes(CadenaOriginal);
					// creamos el flujo 
					memStream = new MemoryStream(CadenaOriginal.Length * 2);
					// obtenemos nuestro objeto cifrador, usando la clase 
					// CryptoServiceProvider codificada anteriormente.
					CryptoServiceProvider cryptoProvider = new CryptoServiceProvider((CryptoServiceProvider.CryptoProvider) this.algorithm, 
						CryptoServiceProvider.CryptoAction.Encrypt);
					ICryptoTransform transform = cryptoProvider.GetServiceProvider(key, IV);
					// creamos el flujo de cifrado, usando el objeto cifrador creado y almancenando
					// el resultado en el flujo MemoryStream.
					CryptoStream cs = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
					// ciframos el mensaje.
					cs.Write(textoPlano, 0, textoPlano.Length);
					// cerramos el flujo de cifrado.
					cs.Close();
				}
				else
					// si los valores de la llave y/o del VI no se han establecido
					// informamos mendiante una excepción.
					throw new Exception("Error al inicializar la clave y el vector");
			}
			catch 
			{
				throw; 
			}
			// la conversión se ha realizado con éxito,
			// convertimos el arreglo de bytes en cadena de caracteres, 
			// usando la clase Convert. Debido al formato UTF8 utilizado nos valemos
			// del método ToBase64String para tal fin.
			return Convert.ToBase64String(memStream.ToArray());
		}

		/// <summary>
		/// Descifra la cadena usando al proveedor específicado.
		/// </summary>
		/// <param name="CadenaCifrada">Cadena con el mensaje cifrado.</param>
		/// <returns>Devuelve una cadena con el mensaje original.</returns>
		public string DescifrarCadena(string CadenaCifrada)
		{
			// creamos el flujo tomando la memoria como respaldo.
			MemoryStream memStream = null;
			try
			{
				// verificamos que la llave y el VI han sido proporcionados.
				if (stringKey != null && stringIV != null)
				{
					// obtenemos el arreglo de bytes correspondiente a la llave
					// y al vector de inicialización.
					byte[] key = MakeKeyByteArray();
					byte[] IV = MakeIVByteArray();
					// obtenemos el arreglo de bytes de la cadena cifrada.
					byte[] textoCifrado = Convert.FromBase64String(CadenaCifrada);

					// creamos el flujo
					memStream = new MemoryStream(CadenaCifrada.Length);
					// obtenemos nuestro objeto cifrador, usando la clase 
					// CryptoServiceProvider codificada anteriormente.
					CryptoServiceProvider cryptoProvider = new CryptoServiceProvider((CryptoServiceProvider.CryptoProvider) this.algorithm, 
						CryptoServiceProvider.CryptoAction.Desencrypt);
					ICryptoTransform transform = cryptoProvider.GetServiceProvider(key, IV);
					// creamos el flujo de cifrado, usando el objeto cifrador creado y almancenando
					// el resultado en el flujo MemoryStream.
					CryptoStream cs = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
					cs.Write(textoCifrado, 0, textoCifrado.Length); // ciframos
					cs.Close(); // cerramos el flujo.
				}
				else
					// si los valores dela llave y/o del VI no se han establecido
					// informamos mendiante una excepción.
					throw new Exception("Error al inicializar la clave y el vector.");
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message,ex.InnerException);
			}

			// utilizamos el mismo sistema de codificación (UTF8) para obtener 
			// la cadena a partir del byte array.
			return Encoding.UTF8.GetString(memStream.ToArray());
		}

		/// <summary>
		/// Cifrar o Descifrar el archivo especificado y almacenada la información
		/// cifrada en el archivo destino proporcionado.
		/// </summary>
		/// <param name="InFileName">Nombre del archivo original.</param>
		/// <param name="OutFileName">Nombre del archivo destino.</param>
		/// <param name="Action">Acción a realizar sobre el archivo.</param>
		public void CifrarDescifrarArchivo(string InFileName, string OutFileName, CryptoAction Action)
		{
			// si el archivo especificado no existe,
			if (! File.Exists(InFileName))
			{
				// generamos una excepción informándolo.
				throw new Exception("No se ha encontrado el archivo.");
			}

			try
			{
				// si la llave de cifrado y el VI están establecidos
				if (stringKey != null && stringIV != null)
				{
					// creamos el flujo de entrada, desde el archivo original.
					FileStream fsIn = new FileStream(InFileName, FileMode.Open, FileAccess.Read);
					// creamos el flujo de salida, hacía el archivo de salida especificado.
					FileStream fsOut = new FileStream(OutFileName, FileMode.OpenOrCreate, FileAccess.Write);
					// establecemos la capacidad del archivo de salida a 0.
					fsOut.SetLength(0);

					byte[] key = MakeKeyByteArray(); // creamos la llave de cifrado.
					byte[] IV = MakeIVByteArray(); // y el vector de inicialización.
					byte[] byteBuffer = new byte[4096]; // creamos un buffer de bytes.
					long largoArchivo = fsIn.Length; // establecemos variables de control
					long bytesProcesados = 0; // para la lectura y escritura de los archivos.
					int bloqueBytes = 0;
					// creamos nuestro objeto cifrador, desde la clase CryptoServiceProvider.
					CryptoServiceProvider cryptoProvider = new CryptoServiceProvider((CryptoServiceProvider.CryptoProvider)         this.algorithm,
						(CryptoServiceProvider.CryptoAction) Action);
					ICryptoTransform transform = cryptoProvider.GetServiceProvider(key, IV);
					CryptoStream cryptoStream = null;

					// dependiendo de la acción especificada,
					switch (Action)
					{
						case CryptoAction.Encrypt:
							// creamos el cifrador, almacenando el resultado en el flujo de salida.
							cryptoStream = new CryptoStream(fsOut, transform, CryptoStreamMode.Write);
							break;
							// ó el descifrador, almacenando el resultado en el flujo de salida.
						case CryptoAction.Desencrypt:
							cryptoStream = new CryptoStream(fsOut, transform, CryptoStreamMode.Write);
							break;
					}

					// mientras el número de bytes procesados es menor que el
					// largo del archivo
					while (bytesProcesados < largoArchivo)
					{
						// leemos un bloque de datos y lo almacenamos en el buffer.
						bloqueBytes = fsIn.Read(byteBuffer, 0, 4096);
						// ciframos los datos del buffer
						cryptoStream.Write(byteBuffer, 0, bloqueBytes);
						// incrementamos el contador de bytes procesados.
						bytesProcesados += (long) bloqueBytes;
					}

					// cerramos los flujos de datos.
					if (cryptoStream != null)
						cryptoStream.Close();
					fsIn.Close();
					fsOut.Close();
				}
				else
					// si los valores de la llave de cifrado y/o el VI no se han
					// asignado, lo informámos mediante una excepción.
					throw new Exception("Error al inicializar la clave y el vector.");
			}
			catch 
			{
				throw;
			}
		}
	}
}
