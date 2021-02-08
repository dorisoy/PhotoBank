import React from 'react';
import { useHistory } from 'react-router-dom';
import Axios from 'axios';
import SignalR from '../api/signalr';
import Utils from '../api/utils';
import Config from '../config';

const styles = {
    li: {
        display: 'inline-block'
    }
}

function Photos() {
    const login = localStorage.getItem('auth-data-login');
    const token = localStorage.getItem('auth-data-token');
    const clientId = localStorage.getItem('auth-data-clientId');
    const history = useHistory();
    const [photos, setPhotos] = React.useState([]);

    React.useEffect(() => {
        function loadPhotosId() {
            Axios({
                method: 'post',
                url: Config.getPhotosApiPath,
                data: { login: login, token: token, clientId: clientId }
            })
        };

        function loadPhotosContent(photoIds) {
            for (let photoIdIndex in photoIds) {
                // получаем содержимое каждой фотки
                Axios({
                    method: 'post',
                    url: Config.getPhotoApiPath,
                    data: { photoId: photoIds[photoIdIndex], login: login, token: token, clientId: clientId }
                });
            }
        };

        const signalr = new SignalR();
        signalr.addHandler('GetPhotosResponse', function (response) {
            if (!response || !response.success) {
                history.push('/');
            } else {
                loadPhotosContent(response.photoIds);
            }
        });
        signalr.addHandler('GetPhotoResponse', function (response) {
            if (!response || !response.success) {
                history.push('/');
            } else {
                const photo = 'data:image/png;base64,' + response.fileBase64Content;
                setPhotos(photos => photos.concat(photo));
            }
        });
        signalr.start(clientId).then(() => loadPhotosId());
    }, []);

    return (
        <div>
            <h1>Photos</h1>
            {photos.length > 0 ?
                <ul>
                    {photos.map((photo, index) => { return <li key={index} style={styles.li}><img src={photo} width='200' /></li> })}
                </ul>
            :'нет фоток'}
        </div >
    )
}

export default Photos;
