import Config from '@/config';
import { HubConnectionBuilder } from '@microsoft/signalr';

export default {
    start: function (clientId) {
        var self = this;
        self.connection = new HubConnectionBuilder().withUrl(Config.host + "hub").build();
        self.connection.start().then(function () {
            self.connection.invoke("Register", clientId);
        });
    }
}
