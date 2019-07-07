using Ease.NUnit.DryIoc.PrismForms;

namespace EasePrismDemos.Tests
{
	public class AppTestBase : PrismFormsTestBase
	{
		public AppTestBase()
		{
			RegisterTypeFactory(AutoMapperConfig.ConfigureMapper);
		}

		protected AutoMapper.IMapper Mapper()
		{
			return ResolveType<AutoMapper.IMapper>();
		}
	}
}
