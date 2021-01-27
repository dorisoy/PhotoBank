import React from 'react'
import Axios from 'axios'
import { SignalR } from '../api/signalr'
import Utils from '../api/utils'
import Config from '../config'

const Auth = React.createClass({

    clientId: Utils.getClientId(),
    login: 'vinge',
    password: '12345',

    getInitialState() {
        const self = this
        self.onLoginResponse()

        return null
    },

    send() {
        const self = this
        Axios({
            method: 'post',
            url: Config.loginApiPath,
            data: { clientId: self.clientId, login: self.login, password: self.password }
        })
    },

    onLoginResponse() {
        const self = this
        const signalr = new SignalR()
        signalr.addHandler('LoginResponse', function (response) {
            if (!response || !response.success) {
                self.context.router.push('/')
            } else {
                self.context.router.push('/photos')
                //self.localStorage.setAuthData({ login: self.login, token: response.token, clientId: self.clientId })
            }
        })
        signalr.start(self.clientId)
    },

    render() {
        const self = this

        return (
            <div>
                <div>
                    <label>Login: </label>
                    <input type="text" defaultValue={self.login} />
                </div>
                <div>
                    <label>Password: </label>
                    <input type="password" defaultValue={self.password} />
                </div>
                <button onClick={self.send}>Send</button>
            </div>
        )
    }
})

Auth.contextTypes = {
    router: React.PropTypes.object.isRequired
}

export default Auth
