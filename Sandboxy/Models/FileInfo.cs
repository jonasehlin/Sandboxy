using System;

namespace Sandboxy.Models
{
	public class FileInfo
	{
		public int FileID { get; set; }
		public string Name { get; set; }
		public DateTime Created { get; set; }
		public int? ContentLength { get; set; }
		public string ContentType { get; set; }
		public int? ClientID { get; set; }

		public string ContentLengthString
		{
			get
			{
				return ToSizeString(ContentLength.HasValue ? ContentLength.Value : 0);
			}
		}

		public static string ToSizeString(long bytes)
		{
			// NOTE: {0:N} Swedish by default separate digit groups with a non-breaking space, dec 160 (HTML: &#160;).
			return string.Format("{0:N0} kB", (long)Math.Ceiling(bytes / 1024.0));
		}

		public static FileInfo CreateFromFile(File file)
		{
			return new FileInfo()
			{
				FileID = file.ID,
				Name = file.Name,
				Created = file.Created,
				ContentLength = file.Content == null ? default(int?) : file.Content.Length,
				ContentType = file.ContentType,
				ClientID = file.ClientID
			};
		}

		public override string ToString()
		{
			return $"Name = {Name}, Created = {Created}, ContentLength = {ContentLength}, ContentType = {ContentType}";
		}
	}
}
