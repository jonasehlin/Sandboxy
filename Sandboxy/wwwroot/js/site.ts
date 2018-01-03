/// <reference path="../lib/jquery/jquery.d.ts" />

class ApiFile {
	ID: number;
	Name: string;
	Created: Date;
	Content: any;
	ContentType: string;
	ClientID?: number;
}

function uploadFiles() {
	var file = (<HTMLInputElement>document.getElementById("fileId")).files[0];
	var reader = new FileReader();
	reader.onload = function () {
		var apiFile = new ApiFile();
		apiFile.Name = file.name;
		apiFile.Created = new Date();
		apiFile.Content = reader.result;
		apiFile.ContentType = file.type;

		/*var xhr = new XMLHttpRequest();
		xhr.open("POST", 'http://localhost:53664/api/Files/Add', true);

		//Send the proper header information along with the request
		xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

		//Call a function when the state changes.
		xhr.onreadystatechange = function () {
			if (xhr.readyState == XMLHttpRequest.DONE) {
				// Request finished. Do processing here.
				if (xhr.status == 200) {
					alert('DOne');
				}
			}
		}

		xhr.send(apiFile);*/

		/*
		$.ajax({
			url: 'http://localhost:53664/api/Files/Add',
			type: 'POST',
			data: apiFile,
			success: function (msg) {
				alert('Sent success!');
			},
			error: function () {
				alert('Error');
			}
		});*/
		//var jsonData = JSON.stringify(apiFile);
		$.ajax({
			headers: {
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			},
			type: 'POST',
			url: 'http://localhost:53664/api/Files/Add',
			data: apiFile,
			dataType: 'json',
			success: function (msg) {
				alert('Sent success!');
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