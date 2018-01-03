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
        apiFile.Content = reader.result;
        apiFile.ContentType = file.type;
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
    return false;
}
//# sourceMappingURL=site.js.map