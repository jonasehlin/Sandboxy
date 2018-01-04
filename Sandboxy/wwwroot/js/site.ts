﻿/// <reference path="../lib/jquery/jquery.d.ts" />
module FileManager {
	class ApiFile {
		ID: number;
		Name: string;
		Created: Date;
		Content: string; // Base-64 encoded
		ContentType: string;
		ClientID?: number;
	}

	export function uploadFiles() {
		var file = (<HTMLInputElement>document.getElementById("fileId")).files[0];
		var reader = new FileReader();
		reader.onload = function () {
			var apiFile = new ApiFile();
			apiFile.Name = file.name;
			apiFile.Created = new Date();
			apiFile.Content = reader.result.toString().split(',')[1];
			apiFile.ContentType = file.type;
			apiFile.ClientID = 4;

			$.ajax({
				headers: {
					'Content-Type': 'application/json'
				},
				url: 'http://localhost:53664/api/Files/Add',
				type: 'POST',
				data: JSON.stringify(apiFile),
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
}