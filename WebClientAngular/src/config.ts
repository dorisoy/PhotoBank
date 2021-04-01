var host = "http://localhost:44364/";
var apiPath = host + "api/";

export default {
  host: host,
  loginApiPath: apiPath + "login",
  getUserInfoApiPath: apiPath + "getUserInfo",
  setUserInfoApiPath: apiPath + "setUserInfo",
  loadUserPictureApiPath: apiPath + "loadUserPicture",
  setUserPictureApiPath: apiPath + "setUserPicture",
  getPhotosApiPath: apiPath + "getPhotos",
  getPhotoApiPath: apiPath + "getPhoto",
  getPhotoAdditionalInfoApiPath: apiPath + "getPhotoAdditionalInfo",
  setPhotoAdditionalInfoApiPath: apiPath + "setPhotoAdditionalInfo",
  uploadPhotosApiPath: apiPath + "uploadPhotos",
  deletePhotoApiPath: apiPath + 'deletePhoto',
}
