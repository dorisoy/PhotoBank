export default {

    getClientId: function () {
        return Date.now().toString()
    },

    fileToBase64: function (file, callback) {
        let reader = new FileReader()
        reader.onload = function (readerEvt) {
            let binaryData = readerEvt.target.result.toString()
            let base64String = window.btoa(binaryData)
            callback(base64String)
        }
        reader.readAsBinaryString(file)
    },

    filesToBase64: function (files, callback) {
        let filesBase64 = []
        let fileToBase64Callback = function (fileBase64) {
            filesBase64.push(fileBase64)
            if (files.length == filesBase64.length) {
                callback(filesBase64)
            }
        }
        for (let i = 0; i < files.length; i++) {
            this.fileToBase64(files[i], fileToBase64Callback)
        }
    }
}
