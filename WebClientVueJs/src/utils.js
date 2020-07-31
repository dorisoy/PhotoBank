
export default {
    fileToBase64: function (file, callback) {
        var reader = new FileReader();
        reader.onload = function (readerEvt) {
            var binaryData = readerEvt.target.result;
            var base64String = window.btoa(binaryData);
            callback(base64String);
        };
        reader.readAsBinaryString(file);
    }
}
