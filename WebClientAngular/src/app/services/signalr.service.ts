import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import Config from 'src/config';

interface EventHandler {
    methodName: string,
    handler: any
}

export class SignalRService {

    private connection: HubConnection
    private handlers: EventHandler[] = []

    constructor() {
    }

    addHandler(methodName: string, handler: any): void {
        var self = this;
        self.handlers.push({ methodName: methodName, handler: handler });
    }

    start(clientId: string): Promise<void> {
        var self = this;
        var registerFunc = function () { self.connection.invoke("Register", clientId); }
        self.connection = new HubConnectionBuilder().withUrl(Config.host + "hub").build();
        for (var i in self.handlers) {
            var handler = self.handlers[i];
            self.connection.on(handler.methodName, handler.handler);
        }

        return self.connection.start().then(registerFunc);
    }
}
