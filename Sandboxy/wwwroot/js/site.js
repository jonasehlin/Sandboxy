var ApiFile = (function () {
    function ApiFile() {
    }
    return ApiFile;
}());
function uploadFiles() {
    var file = document.getElementById("fileId").files[0];
    var reader = new FileReader();
    reader.onload = function () {
        var apiFile = new ApiFile();
        apiFile.Name = file.name;
        apiFile.Created = new Date();
        apiFile.Content = null;
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
    reader.readAsArrayBuffer(file);
    return false;
}
//# sourceMappingURL=site.js.map