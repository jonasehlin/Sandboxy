using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sandboxy.Models
{
	class FileManager
	{
		readonly IDbConnection db;

		/// <summary>
		/// Maximum file size in bytes for clients. Default is 0 = no maximum file size.
		/// </summary>
		public int MaxClientFileSize { get; set; }

		/// <summary>
		/// Maximum file size in bytes of all files connected to a client. Default is 0 = no total maximum file size.
		/// </summary>
		public int MaxClientTotalFileSize { get; set; }

		/// <summary>
		/// Maximum number of files connected to a client. Default is 0 = no maximum file count.
		/// </summary>
		public int MaxClientFileCount { get; set; }

		public FileManager(IDbConnection db)
		{
			Debug.Assert(db != null);
			this.db = db;
		}

		private async Task<int> GetFileCountForClient(int clientID)
		{
			int? fileCount = await db.ExecuteScalarAsync<int?>(
				"SELECT COUNT(*) FROM [dbo].[File] WHERE [ClientID] IS NOT NULL AND [ClientID] = @clientID",
				new { clientID });
			return fileCount.HasValue ? fileCount.Value : 0;
		}

		private async Task<int> GetTotalFileSizeForClient(int clientID)
		{
			// NOTE: DATALENGTH() will return bigint if expression is of the varchar(max), nvarchar(max) or varbinary(max) data types; otherwise int
			// https://docs.microsoft.com/en-us/sql/t-sql/functions/datalength-transact-sql
			var totalFileSize = await db.ExecuteScalarAsync<long?>(
				"SELECT SUM(DATALENGTH([Content])) FROM [dbo].[File] WHERE [ClientID] = @clientID",
				new { clientID });
			return (int)(totalFileSize.HasValue ? totalFileSize.Value : 0);
		}

		private async Task ValidateAddFileToClient(File file)
		{
			if (file == null)
				throw new NullReferenceException("File is empty (null).");

			if (!file.ClientID.HasValue)
				throw new ArgumentException("Need a client ID to connect the file to.", "file.ClientID");

			if (MaxClientFileSize > 0)
			{
				if (file.Content.Length > MaxClientFileSize)
					throw new ArgumentOutOfRangeException("fileSize", $"File is too big, maximum file size for a client is {MaxClientFileSize} bytes.");
			}

			if (MaxClientFileCount > 0)
			{
				var fileCount = await GetFileCountForClient(file.ClientID.Value);
				if ((fileCount + 1) > MaxClientFileCount)
					throw new ArgumentOutOfRangeException("fileSize", $"Client has too many files, maximum number of files is {MaxClientFileCount}.");
			}

			if (MaxClientTotalFileSize > 0)
			{
				var totalFileSize = await GetTotalFileSizeForClient(file.ClientID.Value);
				if ((totalFileSize + file.Content.Length) > MaxClientTotalFileSize)
					throw new ArgumentOutOfRangeException("fileSize", $"The total size of all client files and this file is too big. Maximum total file size for a client is {MaxClientTotalFileSize} bytes.");
			}
		}

		public async Task<FileInfo> AddFileToClient(File file)
		{
			await ValidateAddFileToClient(file);

			file.ID = await db.ExecuteScalarAsync<int>(@"
INSERT INTO [dbo].[File] ([Name], [Created], [Content], [ContentType], [ClientID])
VALUES (@Name, @Created, @Content, @ContentType, @ClientID)
SELECT CAST(SCOPE_IDENTITY() AS int)",
				new { file.Name, file.Created, file.Content, file.ContentType, file.ClientID });

			return FileInfo.CreateFromFile(file);
		}

		public async Task<FileInfo> AddFileToClient(string name, byte[] content, string contentType, int clientID)
		{
			return await AddFileToClient(new File()
			{
				Name = name,
				Created = DateTime.Now,
				Content = content,
				ContentType = contentType,
				ClientID = clientID
			});
		}

		public async Task<File> GetFile(int fileID)
		{
			return await db.QuerySingleOrDefaultAsync<File>(
				"SELECT * FROM [dbo].[File] WHERE [ID] = @fileID",
				new { fileID });
		}

		public async Task<IEnumerable<File>> GetFilesForClient(int clientID)
		{
			return await db.QueryAsync<File>(
				"SELECT * FROM [dbo].[File] WHERE [ClientID] = @clientID",
				new { clientID});
		}

		public async Task<IEnumerable<FileInfo>> GetFileInfosForClient(int clientID)
		{
			return await db.QueryAsync<FileInfo>(@"
SELECT
	[ID] AS [FileID],
	[Name],
	[Created],
	CASE WHEN [Content] IS NULL THEN NULL ELSE CAST(DATALENGTH([Content]) AS INT) END AS [ContentLength],
	[ContentType],
	[ClientID]
FROM
	[dbo].[File]
WHERE
	[ClientID] IS NOT NULL AND
	[ClientID] = @clientID
ORDER BY
	[Name], [Created]",
				new { clientID });
		}

		public async Task<bool> RemoveFile(File file)
		{
			return await RemoveFile(file.ID);
		}

		public async Task<bool> RemoveFile(int fileID)
		{
			var result = await db.ExecuteAsync(
				"DELETE FROM [dbo].[File] WHERE [ID] = @fileID",
				new { fileID });

			return result > 0;
		}

		public async Task<bool> RemoveFiles(IEnumerable<File> files)
		{
			bool result = false;
			foreach (var file in files)
			{
				if (await RemoveFile(file))
					result = true;
			}

			return result;
		}

		public async Task<bool> RemoveAllFilesForClient(int clientID)
		{
			return await RemoveFiles(await GetFilesForClient(clientID));
		}
	}
}
