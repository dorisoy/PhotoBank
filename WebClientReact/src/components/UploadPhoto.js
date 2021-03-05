import React from 'react';
import Axios from 'axios';
import Utils from '../api/utils';
import Config from '../config';

function UploadPhotos() {

    const login = localStorage.getItem('auth-data-login')
    const token = localStorage.getItem('auth-data-token')
    const clientId = localStorage.getItem('auth-data-clientId')
    const [selectedFiles, setSelectedFiles] = React.useState()

    function submitFiles() {
        var uploadFunc = function (filesBase64) {
            Axios({
                method: 'post',
                url: Config.uploadPhotosApiPath,
                data: { login: login, token: token, clientId: clientId, files: filesBase64 }
            })
        }
        Utils.filesToBase64(selectedFiles, uploadFunc)
    }

    function handleFilesUpload(event) {
        setSelectedFiles(event.target.files)
    }

    return (
        <div>
            <label>
                Upload photos:
                <input type='file' id='files' multiple onChange={handleFilesUpload} />
            </label>
            <button onClick={submitFiles}>Send</button>
        </div>
    )
}

export default UploadPhotos
