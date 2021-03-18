import React from 'react'
import Modal from 'react-modal'

const showPhotoDescriptionModalStyles = {
    overlay: {
        position: 'fixed',
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        backgroundColor: 'rgba(255, 255, 255, 0.8)'
    },
    content: {
        position: 'absolute',
        margin: 'auto',
        width: '500px',
        height: '300px',
        border: '1px solid #ccc',
        background: '#fff',
        overflow: 'auto',
        WebkitOverflowScrolling: 'touch',
        borderRadius: '4px',
        outline: 'none'
    }
}

function PhotoDescriptionModal({isOpen, setIsOpen, selectedPhotoDescription, setSelectedPhotoDescription, savePhotoDescription}) {
    selectedPhotoDescription = selectedPhotoDescription ? selectedPhotoDescription : ''
    return (
        <Modal appElement={document.getElementById('root')}
            isOpen={isOpen}
            onRequestClose={() => setIsOpen(false)}
            shouldCloseOnOverlayClick={false}
            style={showPhotoDescriptionModalStyles}>
            <div>
                <p>Описание фотографии</p>
                <textarea value={selectedPhotoDescription} onChange={(e) => setSelectedPhotoDescription(e.target.value)} maxLength='500' style={{width: '100%', height: '200px'}} />
                <div style={{position: 'absolute', bottom: '16px', right: '16px'}}>
                    <button onClick={() => { savePhotoDescription(); setIsOpen(false) }} style={{width: '100px', height: '24px'}}>Сохранить</button>
                    <button onClick={() => setIsOpen(false)} style={{width: '75px', height: '24px', marginLeft: '8px'}}>Отмена</button>
                </div>
            </div>
        </Modal>
    )
}

export default PhotoDescriptionModal
