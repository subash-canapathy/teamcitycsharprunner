namespace CSharpCompiler.Tests
{
	public class Cyclic
	{
		public object Value1 { get; set; }
		public object Value2 { get; set; }

		public Cyclic(object value1, object value2)
		{
			Value1 = value1;
			Value2 = value2;
		}
	}
}