
//var host = "https://localhost:44364/";
var host = "https://localhost:44364/Broker.Api/";

var apiPath = host + "api/";

export default {
    host: host,
    loginApiPath: apiPath + "login",
    getPhotosApiPath: apiPath + "getPhotos",
    getPhotoApiPath: apiPath + "getPhoto",
    uploadPhotosApiPath: apiPath + "uploadPhotos"
}
