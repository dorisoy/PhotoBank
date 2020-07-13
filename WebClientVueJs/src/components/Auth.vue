<template>
    <div class="auth">
        <div>
            <label>Login: </label>
            <input type="text" v-model="login" />
        </div>
        <div>
            <label>Password: </label>
            <input type="text" v-model="password" />
        </div>
        <button v-on:click="send">Send</button>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'Auth',
        props: {
        },
        data() {
            return {
                login: "",
                password: ""
            };
        },
        mounted() {
        },
        methods: {
            send: function () {
                var postData = { login: this.login, password: this.password };
                axios({
                    method: 'post',
                    url: 'https://localhost:44363/api/stub/login',
                    data: postData
                }).then(response => {
                    if (response.data.success) {
                        
                    }
                });
            }
        }
    };
</script>

<style scoped>
</style>
