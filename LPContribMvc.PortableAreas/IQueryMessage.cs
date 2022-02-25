namespace LPContribMvc.PortableAreas
{
	public interface IQueryMessage<TResult> : IEventMessage
	{
		TResult Result
		{
			get;
		}
	}
}
