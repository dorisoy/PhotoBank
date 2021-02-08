const host = 'http://localhost:44364/'

const apiPath = host + 'api/'

export default {
    host: host,
    getClientId: apiPath + 'getClientId',
    loginApiPath: apiPath + 'login',
    getPhotosApiPath: apiPath + 'getPhotos',
    getPhotoApiPath: apiPath + 'getPhoto',
    uploadPhotosApiPath: apiPath + 'uploadPhotos'
}
