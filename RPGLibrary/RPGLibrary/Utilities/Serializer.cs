using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RPGLibrary.Utilities
{
	/// <summary>
	/// Performs functions relating to serialization.
	/// </summary>
	public static class Serializer
	{
		private static BinaryFormatter formatter = new BinaryFormatter();

		/// <summary>
		/// Makes a deep clone of an object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be cloned.</typeparam>
		/// <param name="obj">The object to be cloned.</param>
		/// <returns>A deep clone of the object.</returns>
		public static T DeepClone<T>(T obj)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				formatter.Serialize(ms, obj);
				ms.Position = 0;
				T clone = (T)formatter.Deserialize(ms);
				return clone;
			}
		}

		/// <summary>
		/// Writes an object to a file.
		/// </summary>
		/// <param name="path">The location and filename of the file.</param>
		/// <param name="obj">The object to be serialized.</param>
		public static void WriteToFile(string path, object obj)
		{
			using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				formatter.Serialize(fs, obj);
			}
		}

		/// <summary>
		/// Write an object to a file. A return value indicates whether the serialization succeeded.
		/// </summary>
		/// <param name="path">The location and filename of the file.</param>
		/// <param name="obj">The object to be serialized.</param>
		/// <returns><c>true</c> if the serialization succeeded; otherwise, <c>false</c>.</returns>
		public static bool TryWriteToFile(string path, object obj)
		{
			try
			{
				WriteToFile(path, obj);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Reads an object from a file.
		/// </summary>
		/// <typeparam name="T">The type of the object being read.</typeparam>
		/// <param name="path">The location and filename of the file.</param>
		/// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
		public static T ReadFromFile<T>(string path)
		{
			using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				T obj = (T)formatter.Deserialize(fs);
				return obj;
			}
		}

		/// <summary>
		/// Reads an object from a file. A return value indicates whether the deserialization succeeded.
		/// </summary>
		/// <typeparam name="T">The type of the object being read.</typeparam>
		/// <param name="path">The location and filename of the file.</param>
		/// <param name="obj">When this method returns, contains the deserialized object of type <typeparamref name="T"/>
		/// if the deserialization succeeded, or the default value of <typeparamref name="T"/> if it failed.</param>
		/// <returns><c>true</c> if the serialization succeeded; otherwise, <c>false</c>.</returns>
		public static bool TryReadFromFile<T>(string path, out T obj)
		{
			try
			{
				obj = ReadFromFile<T>(path);
				return true;
			}
			catch
			{
				obj = default(T);
				return false;
			}
		}

		/// <summary>
		/// Checks if a file exists and can be read.
		/// </summary>
		/// <param name="path">The location and filename of the file.</param>
		/// <returns>True if the file was successfully accessed, false otherwise.</returns>
		public static bool CheckFile(string path)
		{
			try
			{
				using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					return true;
				}
			}
			catch
			{
				return false;
			}
		}
	}
}
