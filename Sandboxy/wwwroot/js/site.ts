/// <reference path="../lib/jquery/jquery.d.ts" />
module FileManager {

	const BaseUri: string = 'http://localhost:53664/';
	const ClientID: number = 4;

	class ApiFile {
		ID: number;
		Name: string;
		Created: Date;
		Content: string; // Base-64 encoded
		ContentType: string;
		ClientID?: number;
	}

	class FileInfo {
		fileID: number;
		name: string;
		created: Date;
		contentLength: number;
		contentType: string;
		clientID?: number;
		contentLengthString: string;
	}

	export function uploadFiles() {
		var file = (<HTMLInputElement>document.getElementById("fileId")).files[0];
		var reader = new FileReader();
		reader.onloadstart = function (ev: Event) {
			console.log(ev.type);
		};
		reader.onprogress = function (ev: ProgressEvent) {
			console.log(ev.type);
			if (ev.lengthComputable) {
				console.log('loaded = ' + ev.loaded + ', total = ' + ev.total);
			}
		};
		reader.onloadend = function (ev: ProgressEvent) {
			console.log(ev.type);
			if (ev.lengthComputable) {
				console.log('loaded = ' + ev.loaded + ', total = ' + ev.total);
			}
		};

		reader.onload = function () {
			var apiFile = new ApiFile();
			apiFile.Name = file.name;
			apiFile.Created = new Date();
			apiFile.Content = reader.result.toString().split(',')[1];
			apiFile.ContentType = file.type;
			apiFile.ClientID = ClientID;

			$.ajax({
				xhr: function () {
					var xhr: XMLHttpRequest;
					if ((<any>window).XMLHttpRequest) {
						xhr = new XMLHttpRequest();
					} else {
						xhr = new ActiveXObject("Microsoft.XMLHTTP");
					}

					xhr.upload.addEventListener("progress", function (ev: ProgressEvent) {
						console.log(ev.type);
						if (ev.lengthComputable) {
							console.log('loaded = ' + ev.loaded + ', total = ' + ev.total);
						}
					}, false);

					xhr.addEventListener("progress", function (ev: ProgressEvent) {
						console.log(ev.type);
						if (ev.lengthComputable) {
							console.log('loaded = ' + ev.loaded + ', total = ' + ev.total);
						}
					}, false);

					return xhr;
				},
				headers: {
					'Content-Type': 'application/json'
				},
				url: BaseUri + 'api/Files/Add',
				type: 'POST',
				data: JSON.stringify(apiFile),
				success: function (msg) {
					populateTable();
				},
				error: function () {
					alert('Error');
				}
			});
		};
		reader.readAsDataURL(file);

		// Prevent reload
		return false;
	}

	function createRows(fileInfos: FileInfo[]) {
		var tableBody = (<HTMLTableElement>document.getElementById('filesTable')).tBodies[0];
		tableBody.innerHTML = '';
		var totalFileSize = 0;
		for (var i = 0; i < fileInfos.length; i++) {
			var fileInfo = fileInfos[i];

			var row = tableBody.insertRow();

			var downloadLink = document.createElement('a');
			downloadLink.appendChild(document.createTextNode(fileInfo.name));
			downloadLink.href = BaseUri + 'api/Files/' + fileInfo.fileID;
			row.insertCell(0).appendChild(downloadLink);

			var sizeCell = row.insertCell(1);
			sizeCell.style.textAlign = 'right';
			sizeCell.appendChild(document.createTextNode(fileInfo.contentLengthString));

			//var created = moment(fileInfo.Created);
			var created = fileInfo.created.toString();
			row.insertCell(2).appendChild(document.createTextNode(created));

			var deleteLink = document.createElement('a');
			deleteLink.appendChild(document.createTextNode('Radera'));
			deleteLink.id = fileInfo.name + '@' + fileInfo.fileID;
			deleteLink.href = '';
			deleteLink.onclick = function () {
				var atSign = this.id.indexOf('@');
				var fileName = this.id.substr(0, atSign);
				var fileID = this.id.substr(atSign + 1);
				//deleteFile(fileID, fileName);
				return false;
			}
			row.insertCell(3).appendChild(deleteLink);

			totalFileSize += fileInfo.contentLength;
		}

		var row = tableBody.insertRow();
		row.insertCell(0).appendChild(document.createTextNode('Total filstorlek:'));
		var totalSizeCell = row.insertCell(1);
		totalSizeCell.style.textAlign = 'right';
		totalSizeCell.appendChild(document.createTextNode('' + (<number>Math.ceil(totalFileSize / 1024.0)).toString() + ' kB'));
		row.insertCell(2);
		row.insertCell(3);
	}

	export function populateTable() {
		// 'filesTable'
		$.ajax({
			headers: {
				'Content-Type': 'application/json'
			},
			url: BaseUri + 'api/Files/Client/' + ClientID,
			type: 'GET',
			success: function (result) {
				createRows(result);
			},
			error: function () {
				alert('Error loading files.');
			}
		});
	}
}