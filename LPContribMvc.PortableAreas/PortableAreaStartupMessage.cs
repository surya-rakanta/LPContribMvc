namespace LPContribMvc.PortableAreas
{
	public class PortableAreaStartupMessage : IEventMessage
	{
		private readonly string _areaName;

		public PortableAreaStartupMessage(string areaName)
		{
			_areaName = areaName;
		}

		public override string ToString()
		{
			return $"The {_areaName} Portable Area is regsitering";
		}
	}
}
