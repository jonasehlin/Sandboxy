using System;

namespace Sandboxy.Models
{
	public class File
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public DateTime Created { get; set; }
		public byte[] Content { get; set; }
		public string ContentType { get; set; }
		public int? ClientID { get; set; }

		public override string ToString()
		{
			return $"Name = {Name}, Length = {(Content == null ? "?" : Content.Length.ToString())}";
		}
	}
}
