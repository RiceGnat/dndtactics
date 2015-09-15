using RPGLibrary;

namespace DnDEngine
{
	public interface IBuff : IDecorator<IUnit>
	{
		string Name { get; set; }
		string Description { get; set; }
		
		int Duration { get; set; }
		int Remaining { get; set; }

		void Upkeep();
	}
}
