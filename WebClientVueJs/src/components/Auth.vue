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
    import { HubConnectionBuilder } from '@microsoft/signalr';

    const hubConnection = new HubConnectionBuilder().withUrl(Config.host + "callback").build();
    hubConnection.start();

    export default {
        name: 'Auth',
        props: {
        },
        data() {
            return {
                login: "vinge",
                password: "12345"
            };
        },
        mounted() {
            hubConnection.on("LoginResponse", function (response) {
                alert(response.success);
            });
        },
        methods: {
            send() {
                Axios({
                    method: 'post',
                    url: Config.loginApiPath,
                    data: { login: this.login, password: this.password }
                }).then(response => {
                    if (response.data.success) {
                        this.$cookies.set('login', this.login);
                        this.$cookies.set('token', response.data.token);
                        this.$router.push('photos');
                    }
                });
            }
        }
    };
</script>

<style scoped>
</style>
