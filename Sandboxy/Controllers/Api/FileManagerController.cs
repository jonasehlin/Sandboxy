using Microsoft.AspNetCore.Mvc;
using Sandboxy.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Sandboxy.Controllers.Api
{
	[Produces("application/json")]
    [Route("api/Files")]
    public class FileManagerController : Controller
    {
		IDbConnection _db;
		FileManager _fileManager;

		public FileManagerController()
		{
			_db = new SqlConnection("Data Source=10.140.4.10;Initial Catalog=MK_DbTests;Integrated Security=True");
			_fileManager = new FileManager(_db)
			{
				MaxClientFileCount = 10
				// TODO: Add more restrictions? Config?
			};
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				_db.Dispose();
			}
		}

		[HttpGet("{fileID}")]
		public async Task<IActionResult> GetFile(int fileID)
		{
			File file = await _fileManager.GetFile(fileID);
			if (file == null)
			{
				return NotFound();
			}
			else
			{
				return File(file.Content, file.ContentType, file.Name);
			}
		}

		[HttpPost("Add")]
		public async Task<FileInfo> AddFileToClient([FromBody]File file)
		{
			return await _fileManager.AddFileToClient(file);
		}

		[HttpGet("Client/{clientID}")]
		public async Task<IEnumerable<FileInfo>> GetFileInfosForClient(int clientID)
		{
			return await _fileManager.GetFileInfosForClient(clientID);
		}

		[HttpDelete("Remove/{fileID}")]
		public async Task<bool> RemoveFile(int fileID)
		{
			return await _fileManager.RemoveFile(fileID);
		}
	}
}