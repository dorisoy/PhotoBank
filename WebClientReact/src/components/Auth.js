import React from 'react';
import { useHistory } from 'react-router-dom';
import Axios from 'axios';
import SignalR from '../api/signalr';
import Utils from '../api/utils';
import Config from '../config';

function Auth() {
    const clientId = Utils.getClientId()
    const login = 'vinge'
    const password = '12345'
    const history = useHistory()
    
    function send() {
        Axios({
            method: 'post',
            url: Config.loginApiPath,
            data: { clientId: clientId, login: login, password: password }
        })
    }

    const signalr = new SignalR();
    signalr.addHandler('LoginResponse', function (response) {
        if (!response || !response.success) {
            history.push('/')
        } else {
            localStorage.setItem('auth-data-login', login)
            localStorage.setItem('auth-data-token', response.token)
            localStorage.setItem('auth-data-clientId', clientId)
            history.push('/photos')
        }
    })
    signalr.start(clientId)

    return (
        <div>
            <div>
                <label>Login: </label>
                <input type="text" defaultValue={login} />
            </div>
            <div>
                <label>Password: </label>
                <input type="password" defaultValue={password} />
            </div>
            <button onClick={send}>Send</button>
        </div>
    );
}

export default Auth
