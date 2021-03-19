import React from 'react'

const styles = {
    root: {
        float: 'right',
        width: '250px',
        height: '60px',
        background: 'rgb(156 222 105)',
        borderRadius: '6px',
        boxShadow: '0 0 10px rgb(0 0 0 / 50%)'
    },
    text: {
        display: 'table',
        margin: 'auto',
        verticalAlign: 'middle',
        lineHeight: '50px',
        textAlign: 'center'
    }
}

function Notification({isShow, onComplete, message}) {
    const [opacity, setOpacity] = React.useState(1.0)

    React.useEffect(() => {
        if (isShow) {
            if (opacity > 0.0) {
                setTimeout(() => { setOpacity(opacity => opacity > 0.01 ? opacity - 0.01 : 0.0) }, 25)
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
                <span style={styles.text}>{message}</span>
            </div>
        </div>
    )
}

export default Notification
