import { HubConnectionBuilder } from '@microsoft/signalr'
import Config from '../config'

class SignalR {

    handlers = [];

    addHandler(methodName, handler) {
        const self = this;
        self.handlers.push({ methodName: methodName, handler: handler });
    }

    start(clientId) {
        const self = this;
        const registerFunc = function () { self.connection.invoke('Register', clientId) };
        self.connection = new HubConnectionBuilder().withUrl(Config.host + 'hub').build();
        for (let i in self.handlers) {
            let handler = self.handlers[i];
            self.connection.on(handler.methodName, handler.handler);
        }

        return self.connection.start().then(registerFunc);
    }
}

export default SignalR;
