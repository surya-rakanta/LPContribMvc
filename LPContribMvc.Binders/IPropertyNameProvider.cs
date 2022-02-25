namespace LPContribMvc.Binders
{
	public interface IPropertyNameProvider
	{
		string CreatePropertyName(string prefix, string propertyName);
	}
}
