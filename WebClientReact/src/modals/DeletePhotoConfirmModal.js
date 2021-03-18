import React from 'react'
import Modal from 'react-modal'

const confirmDeletePhotoModalStyles = {
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
        width: '450px',
        height: '120px',
        border: '1px solid #ccc',
        background: '#fff',
        overflow: 'auto',
        WebkitOverflowScrolling: 'touch',
        borderRadius: '4px',
        outline: 'none'
    }
}

function DeletePhotoConfirmModal({isOpen, setIsOpen, deleteSelectedPhoto}) {
    return (
        <Modal appElement={document.getElementById('root')}
            isOpen={isOpen}
            onRequestClose={() => setIsOpen(false)}
            shouldCloseOnOverlayClick={false}
            style={confirmDeletePhotoModalStyles}>
            <div>
                <p>Вы действительно хотите удалить выбранную фотографию?</p>
                <div style={{position: 'absolute', bottom: '16px', right: '16px'}}>
                    <button onClick={() => { deleteSelectedPhoto(); setIsOpen(false) }} style={{width: '75px', height: '24px'}}>Да</button>
                    <button onClick={() => setIsOpen(false)} style={{width: '75px', height: '24px', marginLeft: '8px'}}>Нет</button>
                </div>
            </div>
        </Modal>
    )
}

export default DeletePhotoConfirmModal
