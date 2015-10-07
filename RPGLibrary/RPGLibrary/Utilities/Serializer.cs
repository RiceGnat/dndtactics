using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RPGLibrary.Utilities
{
	public static class Serializer
	{
		private static BinaryFormatter formatter = new BinaryFormatter();

		private static FileStream fs;
		private static MemoryStream ms;

		public static T DeepClone<T>(T obj)
		{
			ms = new MemoryStream();
			formatter.Serialize(ms, obj);
			ms.Position = 0;
			T clone = (T)formatter.Deserialize(ms);
			ms.Close();
			return clone;
		}

		public static void WriteToFile(string path, object obj)
		{
			fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
			formatter.Serialize(fs, obj);
			fs.Close();
		}

		public static T ReadFromFile<T>(string path)
		{
			fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			T obj = (T)formatter.Deserialize(fs);
			fs.Close();
			return obj;
		}
	}
}
