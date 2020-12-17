
var host = "http://localhost:44364/";
//var host = "http://localhost:44364/Broker.Api/";

var apiPath = host + "api/";

export default {
  host: host,
  getClientId: apiPath + "getClientId",
  loginApiPath: apiPath + "login",
  getPhotosApiPath: apiPath + "getPhotos",
  getPhotoApiPath: apiPath + "getPhoto",
  uploadPhotosApiPath: apiPath + "uploadPhotos"
}
