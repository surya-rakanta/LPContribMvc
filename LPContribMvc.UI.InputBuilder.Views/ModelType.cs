namespace LPContribMvc.UI.InputBuilder.Views
{
	public class ModelType<T> : TypeViewModel
	{
		public new T Value
		{
			get
			{
				return (T)base.Value;
			}
			set
			{
				base.Value = value;
			}
		}
	}
}
