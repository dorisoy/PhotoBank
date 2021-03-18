import React from 'react'
import { useHistory } from 'react-router-dom'
import Axios from 'axios'
import Moment from 'react-moment';
import SignalR from '../api/signalr'
import UploadPhotos from '../components/UploadPhoto'
import DeletePhotoConfirmModal from '../modals/DeletePhotoConfirmModal'
import PhotoDescriptionModal from '../modals/PhotoDescriptionModal'
import Config from '../config'

const styles = {
    li: {
        display: 'inline-block'
    },
    dateTime: {
        fontSize: 12
    }
}

const controlButtonStyle = {
    width: '16px',
    height: '16px'
}

function Photos() {
    const login = localStorage.getItem('auth-data-login')
    const token = localStorage.getItem('auth-data-token')
    const clientId = localStorage.getItem('auth-data-clientId')
    const history = useHistory()
    const [photos, setPhotos] = React.useState([])
    const [selectedPhotoId, setSelectedPhotoId] = React.useState()
    const [isDeletePhotoConfirmOpen, setIsDeletePhotoConfirmOpen] = React.useState(false)
    const [isPhotoDescriptionOpen, setIsPhotoDescriptionOpen] = React.useState(false)
    const [selectedPhotoDescription, setSelectedPhotoDescription] = React.useState('')

    function loadPhotosId() {
        Axios({
            method: 'post',
            url: Config.getPhotosApiPath,
            data: { login: login, token: token, clientId: clientId }
        })
    }
    
    function loadPhotosContent(photoIds) {
        for (let photoIdIndex in photoIds) { // получаем содержимое каждой фотки
            Axios({
                method: 'post',
                url: Config.getPhotoApiPath,
                data: { photoId: photoIds[photoIdIndex], login: login, token: token, clientId: clientId }
            })
        }
    }

    function confirmDeletePhoto(photoId) {
        setSelectedPhotoId(photoId)
        setIsDeletePhotoConfirmOpen(true)
    }
    
    function deleteSelectedPhoto() {
        Axios({
            method: 'post',
            url: Config.deletePhotoApiPath,
            data: { photoId: selectedPhotoId, login: login, token: token, clientId: clientId }
        })
    }

    function showPhotoDescription(photoId) {
        setSelectedPhotoId(photoId)
        setIsPhotoDescriptionOpen(true)
        Axios({
            method: 'post',
            url: Config.getPhotoAdditionalInfoApiPath,
            data: { photoId: photoId, login: login, token: token, clientId: clientId }
        })
    }

    function savePhotoDescription() {
        var additionalInfo = {
            description: selectedPhotoDescription
        }
        Axios({
            method: 'post',
            url: Config.setPhotoAdditionalInfoApiPath,
            data: { photoId: selectedPhotoId, additionalInfo: additionalInfo, login: login, token: token, clientId: clientId }
        })
    }

    function initSignalRResponses() {
        const signalr = new SignalR();
        signalr.addHandler('GetPhotosResponse', function (response) {
            if (!response || !response.success) {
                history.push('/')
            } else {
                loadPhotosContent(response.photoIds)
            }
        })
        signalr.addHandler('GetPhotoResponse', function (response) {
            if (!response || !response.success) {
                history.push('/')
            } else {
                const photo = { id: response.photoId, image: 'data:image/png;base64,' + response.fileBase64Content, createDate: new Date(Date.parse(response.createDate)) }
                setPhotos(photos => { photos = photos.concat(photo); photos.sort((x,y) => x.createDate - y.createDate); return photos; })
            }
        })
        signalr.addHandler('UploadPhotosResponse', function (response) {
            if (!response || !response.success) {
                history.push('/')
            } else {
                loadPhotosContent([response.photoId])
            }
        })
        signalr.addHandler('DeletePhotoResponse', function (response) {
            if (!response || !response.success) {
                history.push('/')
            } else {
                setPhotos(photos => photos.filter(photo => photo.id !== response.photoId))
            }
        })
        signalr.addHandler('GetPhotoAdditionalInfoResponse', function (response) {
            if (!response || !response.success) {
                history.push('/')
            } else {
                setSelectedPhotoDescription(response.additionalInfo.description)
            }
        })
        signalr.addHandler('SetPhotoAdditionalInfoResponse', function (response) {
            if (!response || !response.success) {
                history.push('/')
            } else {
                alert('сохранено') // bubble popup !
            }
        })
        signalr.start(clientId).then(() => loadPhotosId())
    }

    React.useEffect(() => { initSignalRResponses(); }, [])

    return (
        <div>
            <h1>Photos</h1>
            <UploadPhotos />
            {photos.length > 0 ?
                <ul>
                    {photos.map((photo, index) => {
                        return  <li key={index} style={styles.li}>
                                    <div style={{width: '216px', height: '155px', margin: '8px', background: 'rgb(230,230,230)'}}>
                                        <div style={{ padding: '8px' }}>
                                            <img src={photo.image} style={{width: '200px', display: 'block', margin: 'auto'}} />
                                            <div>
                                                <Moment style={styles.dateTime} format='DD.MM.YYYY HH:mm'>{photo.createDate}</Moment>
                                                <div style={{ float: 'right', marginTop: '4px' }}>
                                                    <a href='#' onClick={() => showPhotoDescription(photo.id)} style={{marginRight: '8px'}}><img src='/edit.png' style={controlButtonStyle} /></a>
                                                    <a href='#' onClick={() => confirmDeletePhoto(photo.id)}><img src='/trash.png' style={controlButtonStyle} /></a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </li>
                    })}
                </ul>
                :'нет фоток'}

            <DeletePhotoConfirmModal
                isOpen={isDeletePhotoConfirmOpen}
                setIsOpen={setIsDeletePhotoConfirmOpen}
                deleteSelectedPhoto={deleteSelectedPhoto} />

            <PhotoDescriptionModal
                isOpen={isPhotoDescriptionOpen}
                setIsOpen={setIsPhotoDescriptionOpen}
                selectedPhotoDescription={selectedPhotoDescription}
                setSelectedPhotoDescription={setSelectedPhotoDescription}
                savePhotoDescription={savePhotoDescription} />
        </div>
    )
}

export default Photos
