import React from 'react'

const styles = {
    root: {
        background: 'red',
        float: 'right',
        width: '250px',
        height: '60px'
    }
}

function Notification({isShow, onComplete, message}) {
    const [opacity, setOpacity] = React.useState(1.0)
    //isShow = true
    React.useEffect(() => {
        if (isShow) {
            if (opacity > 0.0) {
                setTimeout(() => { setOpacity(opacity => opacity > 0.01 ? opacity - 0.01 : 0.0) }, 10)
            } else {
                onComplete()
            }
        } else if (opacity <= 0.0) {
            setOpacity(1.0)
        }
    })

    return (
        <div style={{ opacity: opacity, display: isShow && opacity > 0.0 ? 'block' : 'none' }}>
            <div style={styles.root}>
                <p>{message}</p>
            </div>
        </div>
    )
}

export default Notification
