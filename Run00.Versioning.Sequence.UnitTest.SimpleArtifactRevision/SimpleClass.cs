namespace Run00.Versioning.Sequence.UnitTest.SimpleArtifact
{
	public class SimpleClass
	{
		public string PropertyOne { get; set; }

		public string HelloWorld(string value)
		{
			var result = "Hello World! I am a modified result.";
			return result;
		}
	}
}