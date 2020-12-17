
export default {

  getClientId: function () {
    return Date.now().toString();
  },

  fileToBase64: function (file, callback) {
    var reader = new FileReader();
    reader.onload = function (readerEvt) {
      var binaryData = readerEvt.target.result.toString();
      var base64String = window.btoa(binaryData);
      callback(base64String);
    };
    reader.readAsBinaryString(file);
  },

  filesToBase64: function (files, callback) {
    var filesBase64 = [];
    var fileToBase64Callback = function (fileBase64) {
      filesBase64.push(fileBase64);
      if (files.length == filesBase64.length) {
        callback(filesBase64);
      }
    };
    for (var i = 0; i < files.length; i++) {
      this.fileToBase64(files[i], fileToBase64Callback);
    }
  }
}
