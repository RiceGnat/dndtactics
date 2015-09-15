using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using RPGEngine;
using DnDEngine;
using DnDEngine.Items;
using Universal;

namespace DnDTactics.Data
{
	public sealed class DataManager : SingletonMonoBehaviour
	{
		#region Singleton
		private static DataManager instance;

		protected override SingletonMonoBehaviour Instance
		{
			get { return instance; }
			set { instance = value as DataManager; }
		}
		#endregion

		#region Inspector fields
		[SerializeField]
		private ClassData[] classData;
		#endregion

		#region Class data
		private Dictionary<string, ClassData> classDict;

		public static IDictionary<string, ClassData> Classes
		{
			get { return instance.classDict; }
		}
		#endregion

		#region Save data
		private static IFormatter formatter = new BinaryFormatter();
		private static Stream stream;

		public static SaveData CurrentFile { get; private set; }

		public static IList<DnDUnit> Party
		{
			get
			{
				if (CurrentFile.Party == null)
					CurrentFile.Party = new List<DnDUnit>();
				return CurrentFile.Party;
			}
		}

		public static IList<ICatalogable> Caravan
		{
			get
			{
				if (CurrentFile.Caravan == null)
					CurrentFile.Caravan = new List<ICatalogable>();
				return CurrentFile.Caravan;
			}
		}

		public static SettingsData Settings
		{
			get
			{
				if (CurrentFile.Settings == null)
					CurrentFile.Settings = new SettingsData();
				return CurrentFile.Settings;
			}
		}

		public static void Save()
		{
			CurrentFile.Timestamp = DateTime.Now;
			stream = new FileStream("save.bin", FileMode.Create, FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, CurrentFile);
			stream.Close();
		}

		public static void Load()
		{
			try
			{
				stream = new FileStream("save.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
				try
				{
					CurrentFile = formatter.Deserialize(stream) as SaveData;
				}
				catch (SerializationException)
				{
					Debug.LogWarning("Could not deserialize data.");
				}
				stream.Close();
			}
			catch (IOException)
			{
				Debug.LogWarning("Could not read save file.");
			}
		}

		#endregion

		#region Unity events
		protected override void Init()
		{
			// Load class data
			classDict = new Dictionary<string, ClassData>(classData.Length);
			foreach (ClassData cd in classData)
			{
				try
				{
					classDict.Add(cd.name, cd);
				}
				catch (ArgumentException)
				{
					if (Debug.isDebugBuild) Debug.LogWarning(cd.name + " already exists in the class dictionary. Entry ignored.");
				}
			}
		}

		void Start()
		{
			// Load saved data
			Load();

			if (CurrentFile == null)
			{
				CurrentFile = new SaveData();

				var party = new List<DnDUnit>();
				DnDUnit unit = new DnDUnit("Kaleina", DnDUnit.ClassType.Spellblade.ToString(), new CoreStats(3, 10, 9, 12, 11, 10, 13, 5, 10), DnDUnit.BodyType.Humanoid, DnDUnit.GenderType.Female);
				(unit.Extensions as IEquipped).Equip(Equipment.Armory.GetNewEquipment(3), Equipment.Type.Weapon);
				(unit.Extensions as IEquipped).Equip(Equipment.Armory.GetNewEquipment(5), Equipment.Type.Body);
				unit.Spells.All.Add(0);
				party.Add(unit);

				unit = new DnDUnit("Davfalcon", "Bird Knight", new CoreStats(2, 14, 12, 11, 10, 10, 11, 5, 10), DnDUnit.BodyType.Humanoid, DnDUnit.GenderType.Male);
				unit.Equipped.Equip(Equipment.Armory.GetNewEquipment(2), Equipment.Type.Weapon);
				party.Add(unit);

				unit = new DnDUnit("Avery", DnDUnit.ClassType.Paladin.ToString(), new CoreStats(2, 12, 13, 10, 10, 13, 10, 4, 10), DnDUnit.BodyType.Humanoid, DnDUnit.GenderType.Female);
				unit.Equipped.Equip(Equipment.Armory.GetNewEquipment(4), Equipment.Type.Weapon);
				unit.Equipped.Equip(Equipment.Armory.GetNewEquipment(6), Equipment.Type.Body);
				party.Add(unit);

				unit = new DnDUnit("Vallaera", DnDUnit.ClassType.Tumblr.ToString(), new CoreStats(2, 12, 10, 16, 10, 10, 12, 6, 10), DnDUnit.BodyType.Humanoid, DnDUnit.GenderType.Female);
				unit.Equipped.Equip(Equipment.Armory.GetNewEquipment(1), Equipment.Type.Weapon);
				party.Add(unit);

				CurrentFile.Party = party;

				CurrentFile.Caravan = new List<ICatalogable>();
			}
			else
			{
				foreach (DnDUnit u in CurrentFile.Party)
				{
					u.Relink();
					//u.Core.LVL++;
				}
			}
			foreach (IUnit u in Party)
			{
				u.Evaluate();
				if (Debug.isDebugBuild) Debug.Log(DnDEngine.DnDUnit.Describe(u).Full);
			}

			foreach (ICatalogable i in Caravan)
			{
				if (Debug.isDebugBuild) Debug.Log(i.Name);
			}

			Settings.Apply();
		}
		#endregion
	}
}