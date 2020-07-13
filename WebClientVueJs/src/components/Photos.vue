<template>
    <div>
        <h1>Photos</h1>
    </div>
</template>

<script>
    import Axios from 'axios';
    import Config from '@/config';
    import '@/cookies';

    export default {
        name: 'Photos',
        props: {
        },
        data() {
            return {};
        },
        mounted() {
            Axios({
                method: 'post',
                url: Config.getPhotosApiPath,
                data: { login: this.$cookies.get('login'), token: this.$cookies.get('token') }
            }).then(response => {
                if (response.data.success) {
                    alert("get photos");
                }
            });
        },
        methods: {
            send() {
            }
        }
    };
</script>

<style scoped>
</style>
