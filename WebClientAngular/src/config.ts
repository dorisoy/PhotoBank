
var host = "http://localhost:44364/";
//var host = "http://localhost:44364/Broker.Api/";

var apiPath = host + "api/";

export default {
  host: host,
  getClientId: apiPath + "getClientId",
  loginApiPath: apiPath + "login",
  getUserInfo: apiPath + "getUserInfo",
  setUserInfo: apiPath + "setUserInfo",
  loadUserPicture: apiPath + "loadUserPicture",
  setUserPicture: apiPath + "setUserPicture",
  getPhotosApiPath: apiPath + "getPhotos",
  getPhotoApiPath: apiPath + "getPhoto",
  getPhotoAdditionalInfo: apiPath + "getPhotoAdditionalInfo",
  setPhotoAdditionalInfo: apiPath + "setPhotoAdditionalInfo",
  uploadPhotosApiPath: apiPath + "uploadPhotos",
  deletePhotoApiPath: apiPath + 'deletePhoto',
}
