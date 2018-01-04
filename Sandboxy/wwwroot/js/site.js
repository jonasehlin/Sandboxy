var FileManager;
(function (FileManager) {
    var BaseUri = 'http://localhost:53664/';
    var ClientID = 4;
    var ApiFile = (function () {
        function ApiFile() {
        }
        return ApiFile;
    }());
    var FileInfo = (function () {
        function FileInfo() {
        }
        return FileInfo;
    }());
    function uploadFiles() {
        var file = document.getElementById("fileId").files[0];
        var reader = new FileReader();
        reader.onloadstart = function (ev) {
            console.log(ev.type);
        };
        reader.onprogress = function (ev) {
            console.log(ev.type);
            if (ev.lengthComputable) {
                console.log('loaded = ' + ev.loaded + ', total = ' + ev.total);
            }
        };
        reader.onloadend = function (ev) {
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
            var xxhr;
            if (window.XMLHttpRequest) {
                xxhr = new XMLHttpRequest();
            }
            else {
                xxhr = new ActiveXObject("Microsoft.XMLHTTP");
            }
            $.ajax({
                xhr: function () {
                    xxhr.upload.addEventListener("progress", function (ev) {
                        console.log(ev.type);
                        if (ev.lengthComputable) {
                            console.log('loaded = ' + ev.loaded + ', total = ' + ev.total);
                        }
                    }, false);
                    xxhr.addEventListener("progress", function (ev) {
                        console.log(ev.type);
                        if (ev.lengthComputable) {
                            console.log('loaded = ' + ev.loaded + ', total = ' + ev.total);
                        }
                    }, false);
                    return xxhr;
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
        return false;
    }
    FileManager.uploadFiles = uploadFiles;
    function createRows(fileInfos) {
        var tableBody = document.getElementById('filesTable').tBodies[0];
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
                return false;
            };
            row.insertCell(3).appendChild(deleteLink);
            totalFileSize += fileInfo.contentLength;
        }
        var row = tableBody.insertRow();
        row.insertCell(0).appendChild(document.createTextNode('Total filstorlek:'));
        var totalSizeCell = row.insertCell(1);
        totalSizeCell.style.textAlign = 'right';
        totalSizeCell.appendChild(document.createTextNode('' + Math.ceil(totalFileSize / 1024.0 + 0.5).toString() + ' kB'));
        row.insertCell(2);
        row.insertCell(3);
    }
    function populateTable() {
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
    FileManager.populateTable = populateTable;
})(FileManager || (FileManager = {}));
//# sourceMappingURL=site.js.map