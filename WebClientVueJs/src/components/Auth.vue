<template>
    <div class="auth">
        <div>
            <label>Login: </label>
            <input type="text" v-model="login" />
        </div>
        <div>
            <label>Password: </label>
            <input type="password" v-model="password" />
        </div>
        <button v-on:click="send">Send</button>
    </div>
</template>

<script>
    import Axios from 'axios';
    import Config from '@/config';
    import '@/cookies';
    import SignalR from '@/signalr';
    import Utils from '@/utils';

    export default {
        name: 'Auth',
        data() {
            return {
                login: "vinge",
                password: "12345"
            };
        },
        mounted() {
            var self = this;
            self.clientId = Utils.getClientId();
            SignalR.start(self.clientId);
            SignalR.connection.on("LoginResponse", function (response) {
                if (response.success) {
                    self.$cookies.set('login', self.login);
                    self.$cookies.set('token', response.token);
                    self.$router.push('photos');
                }
            });
        },
        methods: {
            send() {
                Axios({
                    method: 'post',
                    url: Config.loginApiPath,
                    data: { clientId: this.clientId, login: this.login, password: this.password }
                });
            }
        }
    };
</script>

<style scoped>
</style>
