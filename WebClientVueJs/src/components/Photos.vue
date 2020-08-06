<template>
    <div id="photos">
        <h1>Photos</h1>
        <uploadPhoto />
        <ul>
            <li v-for="photoUrl in photoUrls" v-bind:key="photoUrl">
                <img v-bind:src="photoUrl" width="250" />
            </li>
        </ul>
    </div>
</template>

<script>
    import Axios from 'axios';
    import Config from '@/config';
    import '@/cookies';
    import UploadPhoto from '@/components/UploadPhoto.vue';

    export default {
        name: 'Photos',
        props: {
        },
        components: {
            'uploadPhoto': UploadPhoto
        },
        data() {
            return {
                photoUrls: []
            }
        },
        mounted() {
            // получаем список id всех фоток
            Axios({
                method: 'post',
                url: Config.getPhotosApiPath,
                data: { login: this.$cookies.get('login'), token: this.$cookies.get('token') }
            }).then(response => {
                if (response.data.isAuthenticated == false) {
                    this.$router.push('/');
                } else if (response.data.success) {
                    // формируем адрес для получения содержимого каждой фотки
                    var photoIds = response.data.photoIds;
                    for (var photoIdIndex in photoIds) {
                        var photoUrl = Config.getPhotoApiPath + '?login=' + this.$cookies.get('login') + '&token=' + this.$cookies.get('token') + '&photoId=' + photoIds[photoIdIndex];
                        this.photoUrls.push(photoUrl);
                    }
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
