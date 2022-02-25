namespace LPContribMvc.PortableAreas
{
	public interface ICommandMessage<TResult> : IEventMessage where TResult : ICommandResult
	{
		TResult Result
		{
			get;
		}
	}
}
