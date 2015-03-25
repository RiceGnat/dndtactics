using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using RPGEngine;
using DnDEngine;
using DnDEngine.Items;

namespace DnDTactics
{
	public sealed class DataManager : MonoBehaviour
	{
		#region Inspector fields
		[SerializeField]
		private ClassData[] classData;
		#endregion

		private static DataManager instance;

		#region Class data
		private Dictionary<DnDUnit.ClassType, ClassData> classDict;

		public static string GetClassDisplayName(DnDUnit.ClassType @class)
		{
			return instance.classDict.ContainsKey(@class) && !String.IsNullOrEmpty(instance.classDict[@class].className) ? instance.classDict[@class].className : @class.ToString();
		}

		public static Sprite GetClassArtwork(DnDUnit.ClassType @class)
		{
			return instance.classDict.ContainsKey(@class) ? instance.classDict[@class].classArtwork : null;
		}

		public static Sprite GetClassPortrait(DnDUnit.ClassType @class)
		{
			return instance.classDict.ContainsKey(@class) ? instance.classDict[@class].classPortrait : null;
		}

		public static bool IsClassUnique(DnDUnit.ClassType @class)
		{
			return instance.classDict.ContainsKey(@class) ? instance.classDict[@class].unique : false;
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
				}
				stream.Close();
			}
			catch (IOException)
			{
			}
		}

		#endregion

		#region Unity events
		void Reset()
		{
			// Initialize class data
			var classList = Enum.GetValues(typeof(DnDUnit.ClassType));
			classData = new ClassData[classList.Length];

			for (int i = 0; i < classList.Length; i++)
			{
				classData[i] = new ClassData();
				classData[i].@class = (DnDUnit.ClassType)classList.GetValue(i);
			}
		}

		void Awake()
		{
			if (instance != null)
			{
				// Selfdestruct if there is already a DataManager present
				Destroy(gameObject);
			}
			else
			{
				instance = this;

				// Load class data
				classDict = new Dictionary<DnDUnit.ClassType, ClassData>(classData.Length);
				foreach (var cd in classData)
				{
					classDict.Add(cd.@class, cd);
				}

				// Load saved data
				Load();

				if (CurrentFile == null)
				{
					CurrentFile = new SaveData();

					var party = new List<DnDUnit>();
					DnDUnit unit = new DnDUnit("Kaleina", DnDUnit.ClassType.Spellblade, new CoreStats(3, 10, 9, 12, 11, 10, 13, 5, 10), DnDUnit.BodyType.Humanoid, DnDUnit.GenderType.Female);
					(unit.Extensions as IEquipped).Equip(Equipment.Armory.GetNewEquipment(3), Equipment.Type.Weapon);
					(unit.Extensions as IEquipped).Equip(Equipment.Armory.GetNewEquipment(5), Equipment.Type.Body);
					unit.Spells.All.Add(0);
					party.Add(unit);

					unit = new DnDUnit("Davfalcon", DnDUnit.ClassType.BirdKnight, new CoreStats(2, 14, 12, 11, 10, 10, 11, 5, 10), DnDUnit.BodyType.Humanoid, DnDUnit.GenderType.Male);
					unit.Equipped.Equip(Equipment.Armory.GetNewEquipment(2), Equipment.Type.Weapon);
					party.Add(unit);

					unit = new DnDUnit("Avery", DnDUnit.ClassType.Paladin, new CoreStats(2, 12, 13, 10, 10, 13, 10, 4, 10), DnDUnit.BodyType.Humanoid, DnDUnit.GenderType.Female);
					unit.Equipped.Equip(Equipment.Armory.GetNewEquipment(4), Equipment.Type.Weapon);
					unit.Equipped.Equip(Equipment.Armory.GetNewEquipment(6), Equipment.Type.Body);
					party.Add(unit);

					unit = new DnDUnit("Vallaera", DnDUnit.ClassType.Tumblr, new CoreStats(2, 12, 10, 16, 10, 10, 12, 6, 10), DnDUnit.BodyType.Humanoid, DnDUnit.GenderType.Female);
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
			}
		}
		#endregion
	}
}